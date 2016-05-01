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
using System;

namespace Pinterest.Sdk
{
    [Flags]
    public enum PermissionScope
    {
        /// <summary>Use GET method on a user’s profile, board and Pin details,
        ///  and the Pins on a board.</summary>
        /// <remarks>Must know the identifier.</remarks>
        /// <remarks>>It is the same as passing an empty string.</remarks>
        /// <value></value>
        [Description("")]
        None = 0x0,
        /// <summary>Use GET method on a user’s Pins, boards and likes.</summary>
        /// <value>read_public</value>
        [Description("read_public")]
        ReadPublic = 0x01,
        /// <summary>Use PATCH, POST and DELETE methods on a user’s Pins and boards.</summary>
        /// <value>write_public</value>
        [Description("write_public")]
        WritePublic = 0x02,
        /// <summary>Use GET method on a user’s follows and followers (on boards, users and interests).</summary>
        /// <value>read_relationships</value>
        [Description("read_relationships")]
        ReadRelationships = 0x04,
        /// <summary>Use PATCH, POST and DELETE methods on a user’s follows and followers (on boards, users and interests).</summary>
        /// <value>write_relationships</value>
        [Description("write_relationships")]
        WriteRelationships = 0x08,
        /// <summary>All read permissions.</summary>
        /// <value>read_public,read_relationships</value>
        [Description("read_public,read_relationships")]
        ReadAll = ReadPublic | ReadRelationships,
        /// <summary>All write permissions.</summary>
        /// <value>write_public,write_relationships</value>
        [Description("write_public,write_relationships")]
        WriteAll = WritePublic | WriteRelationships,
        /// <summary>All public scopes.</summary>
        /// <value>read_public,write_public</value>
        [Description("read_public,write_public")]
        AllPublic = ReadPublic | ReadRelationships,
        /// <summary>All relationship scopes.</summary>
        /// <value>read_relationships,write_relationships</value>
        [Description("read_relationships,write_relationships")]
        AllRelationships = ReadRelationships | WriteRelationships,
        /// <summary>All possible scopes.</summary>
        /// <value>read_public,write_public,read_relationships,write_relationships</value>
        [Description("read_public,write_public,read_relationships,write_relationships")]
        All = AllPublic | AllRelationships
    }
}

