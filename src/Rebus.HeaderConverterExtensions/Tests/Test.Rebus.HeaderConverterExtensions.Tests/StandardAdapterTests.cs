using System;
using System.Collections.Generic;
using FluentAssertions;
using Rebus.HeaderConverterExtensions.HeaderStandard;
using Rebus.HeaderConverterExtensions.Utilities;
using Rebus.Messages;
using Rebus.Serialization;
using Xunit;

namespace Test.Rebus.HeaderConverterExtensions.Tests
{
    public static class StandardAdapterTests
    {
        public const string JsonContentType = "application/json;charset=utf-8";
        
        private static byte[] SerializeToJson(object @object)
        {
            var objectSerializer = new ObjectSerializer();
            var convertedBody = objectSerializer.Serialize(@object);
            return convertedBody;
        }
        
        private static TransportMessage GetTransportMessage(
            string headerPrefix = null,
            string intent = DefaultStandardIntentOptions.Event)
        {
            headerPrefix = headerPrefix ?? DefaultStandardHeaderOptions.DefaultHeaderPrefix;
            var testType = new TestType();
            byte[] body = SerializeToJson(testType);
            
            var headers = new Dictionary<string, string>();
            headers[$"{headerPrefix}.{DefaultStandardHeaderOptions.MessageId}"] = Guid.NewGuid().ToString();
            headers[$"{headerPrefix}.{DefaultStandardHeaderOptions.CorrelationId}"] = Guid.NewGuid().ToString();
            headers[$"{headerPrefix}.{DefaultStandardHeaderOptions.ReplyToAddress}"] = "replyAddress";
            headers[$"{headerPrefix}.{DefaultStandardHeaderOptions.OriginatingAddress}"] = "originatingAddress";
            headers[$"{headerPrefix}.{DefaultStandardHeaderOptions.ContentType}"] = JsonContentType;
            headers[$"{headerPrefix}.{DefaultStandardHeaderOptions.MessageType}"] = typeof(TestType).AssemblyQualifiedName;
            headers[$"{headerPrefix}.{DefaultStandardHeaderOptions.Intent}"] = intent;
            headers[$"{headerPrefix}.{DefaultStandardHeaderOptions.TimeSent}"] = 
                new DefaultStandardDateTimeConverter($"{headerPrefix}.{DefaultStandardHeaderOptions.TimeSent}").ToStandardHeaderValidString(DateTime.Now);
            
            var transportMessage = new TransportMessage(headers, body);
            return transportMessage;
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("myHeader")]
        public static void IsUsableOnIncoming_ShouldBeTrue(string headerPrefix)
        {
            var standardHeaderOptions = new DefaultStandardHeaderOptions(headerPrefix);
            var transportMessage = GetTransportMessage(headerPrefix);
            StandardAdapter.IsUsableOnIncoming(transportMessage, standardHeaderOptions).Should().BeTrue();
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("myHeader")]
        public static void IsUsableOnIncoming_ShouldBeFalse(string headerPrefix)
        {
            var standardHeaderOptions = new DefaultStandardHeaderOptions(headerPrefix);
            var rebusHeaders = new Dictionary<string, string>();
            rebusHeaders[Headers.MessageId] = Guid.NewGuid().ToString();
            var transportMessage = new TransportMessage(rebusHeaders, new byte[0]);
            StandardAdapter.IsUsableOnIncoming(transportMessage, standardHeaderOptions).Should().BeFalse();
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("myHeader")]
        public static void ConvertIncomingTransportMessage_ShouldConvertToRebusHeaders(string headerPrefix)
        {
            var standardHeaderOptions = new DefaultStandardHeaderOptions(headerPrefix);
            var transportMessage = GetTransportMessage(headerPrefix);
            var rebusTransportMessage = StandardAdapter.ConvertIncomingTransportMessage(transportMessage, standardHeaderOptions);
            rebusTransportMessage.Headers[Headers.Intent].Should().Be(Headers.IntentOptions.PublishSubscribe);
            rebusTransportMessage.Headers[Headers.MessageId].Should().Be(
                transportMessage.Headers[$"{standardHeaderOptions.StandardHeaderPrefix}.{DefaultStandardHeaderOptions.MessageId}"]);
            rebusTransportMessage.Headers[Headers.ContentType].Should().Be(JsonContentType);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("myHeader")]
        public static void ConvertOutgoingTransportMessage_ShouldConvertToStandardHeaders(string headerPrefix)
        {
            var standardHeaderOptions = new DefaultStandardHeaderOptions(headerPrefix);
            var transportMessage = GetTransportMessage(headerPrefix);
            var rebusTransportMessage = StandardAdapter.ConvertIncomingTransportMessage(transportMessage, standardHeaderOptions);
            rebusTransportMessage.Headers[Headers.Intent].Should().Be(Headers.IntentOptions.PublishSubscribe);
            rebusTransportMessage.Headers[Headers.MessageId].Should().Be(transportMessage.Headers[
                $"{standardHeaderOptions.StandardHeaderPrefix}.{DefaultStandardHeaderOptions.MessageId}"]);
            rebusTransportMessage.Headers[Headers.ContentType].Should().Be(JsonContentType);

            var standardTransportMessage = StandardAdapter.ConvertOutgoingTransportMessage(rebusTransportMessage, standardHeaderOptions);
            standardTransportMessage.Headers[$"{standardHeaderOptions.StandardHeaderPrefix}.{DefaultStandardHeaderOptions.Intent}"]
                .Should().Be(DefaultStandardIntentOptions.Event);
            standardTransportMessage.Headers[$"{standardHeaderOptions.StandardHeaderPrefix}.{DefaultStandardHeaderOptions.MessageId}"]
                .Should().Be(transportMessage.Headers[$"{standardHeaderOptions.StandardHeaderPrefix}.{DefaultStandardHeaderOptions.MessageId}"]);
            standardTransportMessage.Headers[$"{standardHeaderOptions.StandardHeaderPrefix}.{DefaultStandardHeaderOptions.ContentType}"]
                .Should().Be(JsonContentType);
        }
    }
}