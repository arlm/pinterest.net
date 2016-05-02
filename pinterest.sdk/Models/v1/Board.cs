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
namespace Pinterest.Sdk.Models.v1
{

    public class Board
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public Creator creator { get; set; }
        public DateTime created_at { get; set; }
        public BoardCounters counts { get; set; }
        public Image image { get; set; }
        public string privacy { get; set; }
        public string reason { get; set; }
    }
}

