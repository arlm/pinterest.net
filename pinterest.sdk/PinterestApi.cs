//
//  Copyright 2016 Alexandre Rocha Lima e Marcondes 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Pinterest.Sdk.Models;
using Pinterest.Sdk.Models.v1;
using RestSharp;
using System.Text;
using RestSharp.Extensions.MonoHttp;

namespace Pinterest.Sdk
{
    public class PinterestApi
    {
        private const string BASE_URL = "https://api.pinterest.com/";

        private RestClient client;

        public ApiV1 v1 { get; }

        public int RateLimit { get; protected set; }
        public int RemainingLimit { get; protected set; }
        public DateTime RateLimitDateStamp { get; protected set; }

        public PinterestApi ()
        {
            v1 = new ApiV1(this);

            client = new RestClient();
            client.BaseUrl = new Uri(BASE_URL);

            if (ConfigurationManager.AppSettings["USE_STORED_TOKEN"]?.ToLower() == "true")
            {
                var accessToken = ConfigurationManager.AppSettings["ACCESS_TOKEN"];

                if (!string.IsNullOrEmpty(accessToken))
                    client.Authenticator = new OAuth2PinterestAuthenticator(accessToken);
            }
        }

        public PinterestApi (string accessToken) : this()
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException(nameof(accessToken));

            client.Authenticator = new OAuth2PinterestAuthenticator(accessToken);
        }

        internal string GetRequestString (Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var endpoint = new Uri(BASE_URL);
            var resource = new StringBuilder();

            for (int index = 0; index < uri.Segments.Length; index++)
            {
                if (index >= endpoint.Segments.Length ||
                    uri.Segments[index] != endpoint.Segments[index])
                {
                    resource.Append(uri.Segments[index]);
                }
            }

            return resource.ToString();
        }

        internal RestRequest GetRequest (Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var request = new RestRequest();

            request.Resource = GetRequestString(uri);

            var queries = HttpUtility.ParseQueryString(uri.Query);
            foreach (var key in queries.AllKeys)
            {
                request.AddParameter(key, queries[key], ParameterType.QueryString);
            }

            return request;
        }

        internal T Execute<T> (RestRequest request) where T : new()
        {
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var pinterestException = new ApplicationException(message, response.ErrorException);
                throw pinterestException;
            }

            RateLimit = int.Parse(response.Headers.ToList()
                                  .Find(x => x.Name == "X-Ratelimit-Limit")
                                  .Value.ToString());
            RemainingLimit = int.Parse(response.Headers.ToList()
                                  .Find(x => x.Name == "X-Ratelimit-Remaining")
                                  .Value.ToString());
            RateLimitDateStamp = DateTime.Parse(response.Headers.ToList()
                                  .Find(x => x.Name == "Date")
                                  .Value.ToString()); ;

            return response.Data;
        }

        public bool AuthenticateFirstStep (Uri redirectUri,
                                           PermissionScope scope = PermissionScope.None,
                                           string state = null)
        {
            if (redirectUri == null)
                throw new ArgumentNullException(nameof(redirectUri));

            if (ConfigurationManager.AppSettings["USE_STORED_TOKEN"].ToLower() == "true")
                return false;

            var clientId = ConfigurationManager.AppSettings["APP_ID"];

            if (string.IsNullOrEmpty(clientId))
                return false;

            var request = new RestRequest();
            request.Resource = "oauth/";
            request.RootElement = "data";
            request.Method = Method.GET;

            request.AddParameter("response_type", "code", ParameterType.UrlSegment);
            request.AddParameter("client_id", clientId, ParameterType.UrlSegment);
            if (state != null)
                request.AddParameter("scope", scope.GetDescription(), ParameterType.UrlSegment);
            request.AddParameter("redirect_uri", redirectUri.ToString(), ParameterType.UrlSegment);

            var response = client.Execute(request);

            return response.ResponseStatus == ResponseStatus.Completed;
        }

        public bool AuthenticateSecondStep (string code)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));

            if (ConfigurationManager.AppSettings["USE_STORED_TOKEN"].ToLower() == "true")
                return false;

            var clientId = ConfigurationManager.AppSettings["APP_ID"];

            if (string.IsNullOrEmpty(clientId))
                return false;

            var clientSecret = ConfigurationManager.AppSettings["APP_SECRET"];

            if (string.IsNullOrEmpty(clientSecret))
                return false;

            var request = new RestRequest();
            request.Resource = "v1/oauth/token";
            request.RootElement = "data";
            request.Method = Method.GET;

            request.AddParameter("grant_type", "authorization_code", ParameterType.UrlSegment);
            request.AddParameter("client_id", clientId, ParameterType.UrlSegment);
            request.AddParameter("client_secret", clientSecret, ParameterType.UrlSegment);
            request.AddParameter("code", code, ParameterType.UrlSegment);

            var response = client.Execute(request);

            return response.ResponseStatus == ResponseStatus.Completed;
        }

        public bool AuthenticateFirstStep (string clientId, Uri redirectUri,
                                           PermissionScope scope = PermissionScope.None,
                                           string state = null)
        {
            var request = new RestRequest();
            request.Resource = "oauth/";
            request.RootElement = "data";
            request.Method = Method.GET;

            request.AddParameter("response_type", "code", ParameterType.UrlSegment);
            request.AddParameter("client_id", clientId, ParameterType.UrlSegment);
            if (state != null)
                request.AddParameter("scope", scope.GetDescription(), ParameterType.UrlSegment);
            request.AddParameter("redirect_uri", redirectUri.ToString(), ParameterType.UrlSegment);

            var response = client.Execute(request);

            return response.ResponseStatus == ResponseStatus.Completed;
        }

        public class ApiV1
        {
            private PinterestApi api;

            internal ApiV1 (PinterestApi api)
            {
                if (api == null)
                    throw new ArgumentNullException(nameof(api));

                this.api = api;
            }

            public User GetMyData ()
            {
                var request = new RestRequest();
                request.Resource = "v1/me/";
                request.RootElement = "data";

                return api.Execute<User>(request);
            }

            public List<Board> GetMyBoards ()
            {
                var request = new RestRequest();
                request.Resource = "v1/me/boards/";
                request.RootElement = "data";

                return api.Execute<List<Board>>(request);
            }

            public Board GetBoardInfo (string boardId)
            {
                var request = new RestRequest();
                request.Resource = "v1/boards/{boardId}/";
                request.RootElement = "data";

                request.AddParameter("boardId", boardId, ParameterType.UrlSegment);

                return api.Execute<Board>(request);
            }

            public Pin GetPinInfo (string pinId)
            {
                var request = new RestRequest();
                request.Resource = "v1/pins/{pinId}/";
                request.RootElement = "data";

                request.AddParameter("pinId", pinId, ParameterType.UrlSegment);

                return api.Execute<Pin>(request);
            }

            public PaginatedResponse<Pin> GetPinsFromBoard (string boardId)
            {
                var request = new RestRequest();
                request.Resource = "v1/pins/{pinId}/";

                request.AddParameter("boardId", boardId, ParameterType.UrlSegment);

                return api.Execute<PaginatedResponse<Pin>>(request);
            }

            public PaginatedResponse<Pin> GetNextPinsFromBoardPage (Uri uri)
            {
                var request = api.GetRequest(uri);

                return api.Execute<PaginatedResponse<Pin>>(request);
            }
        }
    }
}

