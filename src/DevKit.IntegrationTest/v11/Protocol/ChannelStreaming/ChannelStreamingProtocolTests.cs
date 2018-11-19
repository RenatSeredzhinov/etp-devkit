﻿//----------------------------------------------------------------------- 
// ETP DevKit, 1.2
//
// Copyright 2018 Energistics
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

using System.Linq;
using System.Threading.Tasks;
using Energistics.Etp.Common;
using Energistics.Etp.v11.Protocol.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energistics.Etp.v11.Protocol.ChannelStreaming
{
    [TestClass]
    public class ChannelStreamingProtocolTests : IntegrationTestBase
    {
        [TestInitialize]
        public void TestSetUp()
        {
            SetUp(TestSettings.WebSocketType, EtpSettings.Etp11SubProtocol);

            // Register protocol handler
            _server.Register<IChannelStreamingProducer, ChannelStreamingProducer11MockHandler>();

            _server.Start();
        }

        [TestCleanup]
        public void TestTearDown()
        {
            CleanUp();
        }

        [TestMethod]
        public async Task IChannelStreamingConsumer_v11_Start_Connected_To_Simple_Producer()
        {
            _client.Register<IChannelStreamingConsumer, ChannelStreamingConsumer11MockHandler>();
            var handler = _client.Handler<IChannelStreamingConsumer>() as ChannelStreamingConsumer11MockHandler;

            // Register event handlers
            var onChannelMetadata = HandleAsync<ChannelMetadata>(x => handler.OnChannelMetadata += x);
            var onChannelData = HandleAsync<ChannelData>(x => handler.OnChannelData += x);
            var onOpenSession = HandleAsync<OpenSession>(x => handler.OnOpenSession += x);

            // Wait for Open connection
            var isOpen = await _client.OpenAsyncWithTimeout();
            Assert.IsTrue(isOpen);

            // Wait for OpenSession to check if the producer is a simple streamer
            await onOpenSession.WaitAsync();
            Assert.IsTrue(handler.ProducerIsSimpleStreamer);

            // Send Start message
            handler.Start();

            // Wait for ChannelMetadata message
            var argsMetadata = await onChannelMetadata.WaitAsync();

            Assert.IsNotNull(argsMetadata);
            Assert.IsNotNull(argsMetadata.Message.Channels);
            Assert.IsTrue(argsMetadata.Message.Channels.Any());

            // Wait for ChannelData message
            var argsData = await onChannelData.WaitAsync();

            Assert.IsNotNull(argsData);
            Assert.IsNotNull(argsData.Message.Data);
            Assert.IsTrue(argsData.Message.Data.Any());
        }
    }
}
