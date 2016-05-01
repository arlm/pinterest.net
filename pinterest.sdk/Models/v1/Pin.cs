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
    public class Pin
    {
        public string id { get; set; }
        public string link { get; set; }
        public string url { get; set; }
        public Creator creator { get; set; }
        public Board board { get; set; }
        public DateTime created_at { get; set; }
        public string note { get; set; }
        public string color { get; set; }
        public Media media { get; set; }
        public object attribution { get; set; }
        public Image image { get; set; }
        public Metadata metadata { get; set; }
        public string original_link { get; set; }
        public PinCounters counts { get; set; }
    }
}

