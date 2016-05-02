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

using System.Collections.Generic;
using System;

namespace Pinterest.console
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            var boards = new Dictionary<string, string>();
            var allPins = new Dictionary<string, Sdk.Models.v1.Pin>();

            var api = new Sdk.PinterestApi();

            foreach (var board in api.v1.GetMyBoards())
            {
                boards.Add(board.id, board.name);
            }

            foreach (var id in boards.Keys)
            {
                Sdk.Models.PaginatedResponse<Sdk.Models.v1.Pin> pins;

                do
                {
                    pins = api.v1.GetPinsFromBoard(id);

                    foreach (var pin in pins.data)
                    {
                        allPins.Add(pin.id, pin);
                    }
                } while (pins.page?.next != null);
            }

            var me = api.v1.GetMyData();
            Console.WriteLine("Pins {0}x{1}", me.counts.pins, allPins.Count);
            Console.WriteLine("Boards {0}x{1}", me.counts.boards, boards.Count);
        }
    }
}
