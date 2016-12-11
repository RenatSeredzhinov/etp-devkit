﻿//----------------------------------------------------------------------- 
// ETP DevKit, 1.1
//
// Copyright 2016 Energistics
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using Energistics.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Energistics.Datatypes.ChannelData
{
    [TestClass]
    public class StreamingStartIndexConverterTests
    {
        [TestMethod]
        public void StreamingStartIndexConverter_WriteJson_serializes_int_value()
        {
            var index = new StreamingStartIndex
            {
                Item = 2
            };

            const string expected = "{\"item\":{\"int\":2}}";
            var json = EtpExtensions.Serialize(null, index);

            Assert.AreEqual(expected, json);
        }

        [TestMethod]
        public void StreamingStartIndexConverter_ReadJson_deserializes_int_value()
        {
            const string json = "{\"item\":{\"int\":2}}";
            var index = EtpExtensions.Deserialize<StreamingStartIndex>(null, json);

            Assert.IsTrue(index.Item is int);
            Assert.AreEqual(2, (int) index.Item);
        }

        [TestMethod]
        public void StreamingStartIndexConverter_WriteJson_serializes_long_value()
        {
            var index = new StreamingStartIndex
            {
                Item = 10L
            };

            var expected = "{\"item\":{\"long\":10}}";
            var json = EtpExtensions.Serialize(null, index);

            Assert.AreEqual(expected, json);
        }

        [TestMethod]
        public void StreamingStartIndexConverter_ReadJson_deserializes_long_value()
        {
            const string json = "{\"item\":{\"long\":10}}";
            var index = EtpExtensions.Deserialize<StreamingStartIndex>(null, json);

            Assert.IsTrue(index.Item is long);
            Assert.AreEqual(10L, (long) index.Item);
        }

        [TestMethod]
        public void StreamingStartIndexConverter_WriteJson_serializes_null_value()
        {
            var index = new StreamingStartIndex
            {
                Item = null
            };

            const string expected = "{\"item\":null}";
            var json = EtpExtensions.Serialize(null, index);

            Assert.AreEqual(expected, json);
        }

        [TestMethod]
        public void StreamingStartIndexConverter_ReadJson_deserializes_null_value()
        {
            const string json = "{\"item\":null}";
            var index = EtpExtensions.Deserialize<StreamingStartIndex>(null, json);

            Assert.IsNull(index.Item);
        }

        [TestMethod]
        public void StreamingStartIndexConverter_ReadJson_deserializes_default_value()
        {
            const string json = "{\"item\":1}";
            var index = EtpExtensions.Deserialize<StreamingStartIndex>(null, json);

            Assert.IsTrue(index.Item is long);
            Assert.AreEqual(1L, (long) index.Item);
        }

        [TestMethod]
        public void StreamingStartIndexConverter_ReadJson_deserializes_string_value()
        {
            const string json = "{\"item\":\"5\"}";
            var index = EtpExtensions.Deserialize<StreamingStartIndex>(null, json);

            Assert.IsTrue(index.Item is long);
            Assert.AreEqual(5L, (long) index.Item);
        }

        [TestMethod]
        public void StreamingStartIndexConverter_WriteJson_raises_error_for_unsupported_data_type()
        {
            var index = new StreamingStartIndex
            {
                Item = "abc"
            };

            var pass = false;

            try
            {
                EtpExtensions.Serialize(null, index);
            }
            catch (JsonSerializationException)
            {
                pass = true;
            }

            Assert.IsTrue(pass);
        }

        [TestMethod]
        public void StreamingStartIndexConverter_ReadJson_raises_error_for_unsupported_data_type()
        {
            const string json = "{\"item\":\"abc\"}";
            var pass = false;

            try
            {
                EtpExtensions.Deserialize<StreamingStartIndex>(null, json);
            }
            catch (JsonSerializationException)
            {
                pass = true;
            }

            Assert.IsTrue(pass);
        }
    }
}
