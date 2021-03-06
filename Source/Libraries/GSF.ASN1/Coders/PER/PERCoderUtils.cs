//******************************************************************************************************
//  PERCoderUtils.cs - Gbtc
//
//  Copyright � 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  09/24/2013 - J. Ritchie Carroll
//       Derived original version of source code from BinaryNotes (http://bnotes.sourceforge.net).
//
//******************************************************************************************************

#region [ Contributor License Agreements ]

/*
    Copyright 2006-2011 Abdulla Abdurakhmanov (abdulla@latestbit.com)
    Original sources are available at www.latestbit.com

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

            http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/

#endregion

namespace GSF.ASN1.Coders.PER
{
    public class PERCoderUtils
    {
        public static int getMaxBitLength(long val)
        {
            int bitCnt = 0;
            while (val != 0)
            {
                if (val >= 0)
                    val = val >> 1;
                else
                    val = (val >> 1) + (2 << ~1);
                bitCnt++;
            }
            return bitCnt;
        }

        public static bool is7BitEncodedString(ElementInfo info)
        {
            bool is7Bit = false;
            int stringType = CoderUtils.getStringTagForElement(info);
            is7Bit = (
                         stringType == UniversalTags.PrintableString
                         || stringType == UniversalTags.VisibleString
                     );
            return is7Bit;
        }
    }
}