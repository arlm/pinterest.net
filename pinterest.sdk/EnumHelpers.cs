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
using System.Text;

namespace Pinterest.Sdk
{
    public static class EnumHelpers
    {
        public const char ENUM_FLAGGED_VALUE_SEPERATOR_CHARACTER = ',';

        public static string GetDescription (this Enum enumValue)
        {
            var enumValueAsString = new StringBuilder();

            var enumType = enumValue.GetType();

            var enumToStringParts = enumValue.ToString().Split(ENUM_FLAGGED_VALUE_SEPERATOR_CHARACTER);

            foreach (string enumValueStringPart in enumToStringParts)
            {
                var enumValueField = enumType.GetField(enumValueStringPart.Trim());

                var enumDesc = enumValueField.GetCustomAttributes(typeof(DescriptionAttribute), false) as
                                             DescriptionAttribute[];
                var enumExclusive = enumValueField.GetCustomAttributes(typeof(ExclusiveValueAttribute), false) as
                                                  ExclusiveValueAttribute[];
                var enumFieldValue = enumValueField.GetRawConstantValue() as int?;

                if ((enumExclusive?.Length == 1 && enumExclusive[0].IsExclusive) ||
                    (enumFieldValue.HasValue && enumFieldValue == 0))
                {
                    enumValueAsString.Clear();

                    if (enumValueAsString?.Length > 0)
                    {
                        enumValueAsString.Append(ENUM_FLAGGED_VALUE_SEPERATOR_CHARACTER);
                    }

                    break;
                }

                if (enumValueAsString?.Length > 0)
                {
                    enumValueAsString.Append(ENUM_FLAGGED_VALUE_SEPERATOR_CHARACTER);
                }

                if (enumDesc?.Length == 1)
                {
                    enumValueAsString.Append(enumDesc[0].Description);
                } else {
                    enumValueAsString.Append(enumValueStringPart);
                }
            }

            return enumValueAsString.ToString();
        }
    }
}

