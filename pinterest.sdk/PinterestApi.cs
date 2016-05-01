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
using System.Linq;
using Pinterest.Sdk.Models;
using Pinterest.Sdk.Models.v1;
using RestSharp;

namespace Pinterest.Sdk
{
    public class PinterestApi
    {
        const string BASE_URL = "https://api.pinterest.com/";

        readonly string CLIENT_ID;
        string accessToken;

        public ApiV1 v1 { get; }

        public int RateLimit { get; protected set; }
        public int RemainingLimit { get; protected set; }
        public DateTime RateLimitDateStamp { get; protected set; }

        public PinterestApi ()
        {
            v1 = new ApiV1(this);
        }

        public PinterestApi (string clientId) : this()
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentNullException(nameof(clientId));

            this.CLIENT_ID = clientId;
        }

        internal T Execute<T> (RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BASE_URL);
            client.Authenticator = new OAuth2PinterestAuthenticator(accessToken);
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
                request.RootElement = "data";

                request.AddParameter("boardId", boardId, ParameterType.UrlSegment);

                return api.Execute<PaginatedResponse<Pin>>(request);
            }
        }
    }
}

