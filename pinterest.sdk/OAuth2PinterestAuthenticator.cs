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
using RestSharp;
using RestSharp.Authenticators;

namespace Pinterest.Sdk
{
    public class OAuth2PinterestAuthenticator : OAuth2Authenticator
    {

        public OAuth2PinterestAuthenticator (string accessToken) : base(accessToken)
        {
        }

        public override void Authenticate (IRestClient client, IRestRequest request)
        {
            request.AddParameter("access_token", AccessToken, ParameterType.GetOrPost);
        }
    }
}