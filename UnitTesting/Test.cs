//
//  Copyright 2016  Alexandre Rocha Lima e Marcondes
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
using NUnit.Framework;
using Pinterest.Sdk;
using System;
using System.Linq;

namespace UnitTesting
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void TestEnumDescription ()
        {
            var scope = PermissionScope.ReadAll;
            Assert.AreEqual("read_public,read_relationships", scope.GetDescription(), "ReadAll test failed");

            scope = PermissionScope.None;
            Assert.AreEqual(string.Empty, scope.GetDescription(), "None test failed");
        }

        [Test()]
        public void TestGetRequestString ()
        {
            var api = new PinterestApi();
            var uri = new Uri("https://api.pinterest.com/v1/boards/anapinskywalker/wanderlust/pins/?access_token=abcde&fields=id%2Clink%2Ccounts&2Cnote&limit=2&cursor=abcde1234");

            var request = api.GetRequestString(uri);

            Assert.AreEqual("v1/boards/anapinskywalker/wanderlust/pins/", request);
        }

        [Test()]
        public void TestGetRequest ()
        {
            var api = new PinterestApi();
            var uri = new Uri("https://api.pinterest.com/v1/boards/anapinskywalker/wanderlust/pins/?access_token=abcde&fields=id%2Clink%2Ccounts%2Cnote&limit=2&cursor=abcde1234");

            var request = api.GetRequest(uri);

            Assert.AreEqual("v1/boards/anapinskywalker/wanderlust/pins/", request.Resource);
            Console.Write(string.Join(";", request.Parameters.ConvertAll<string>(p => $"{p.Name}={p.Value}")));
            Assert.AreEqual(4, request.Parameters.Count);
            Assert.AreEqual("access_token", request.Parameters[0].Name);
            Assert.AreEqual("abcde", request.Parameters[0].Value);
            Assert.AreEqual("fields", request.Parameters[1].Name);
            Assert.AreEqual("id,link,counts,note", request.Parameters[1].Value);
            Assert.AreEqual("limit", request.Parameters[2].Name);
            Assert.AreEqual("2", request.Parameters[2].Value);
            Assert.AreEqual("cursor", request.Parameters[3].Name);
            Assert.AreEqual("abcde1234", request.Parameters[3].Value);
        }
    }
}

