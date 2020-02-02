using System.Collections.Generic;
using Rebus.Messages;

namespace Rebus.HeaderConverterExtensions.HeaderStandard
{
    public class DefaultStandardHeaderOptions : IStandardHeaderOptions
    {
        public DefaultStandardHeaderOptions(
            string standardHeaderPrefix = null, 
            Dictionary<string, string> rebusToStandardMap = null,
            IStandardIntentOptions standardIntentOptions = null,
            IStandardDateTimeConverter standardDateTimeConverter = null)
        {
            StandardHeaderPrefix = standardHeaderPrefix ?? DefaultHeaderPrefix;
            RebusToStandardMap = rebusToStandardMap ?? GetDefaultRebusToStandardMap(StandardHeaderPrefix);
            StandardIntentOptions = standardIntentOptions ?? new DefaultStandardIntentOptions($"{StandardHeaderPrefix}.{Intent}");
            StandardDateTimeConverter = standardDateTimeConverter ?? new DefaultStandardDateTimeConverter($"{StandardHeaderPrefix}.{TimeSent}");
        }
        
        /// <summary>
        /// string - Default header prefix.
        /// </summary>
        public const string DefaultHeaderPrefix = "STANDARD";
        
        /// <summary>
        /// GUID - Unique message id.
        /// </summary>
        public static readonly string MessageId = "MessageId";
        
        /// <summary>
        /// GUID - Unique message correlation id.
        /// </summary>
        public static readonly string CorrelationId = "CorrelationId";
        
        /// <summary>
        /// string - Which address/queue to send replies.
        /// </summary>
        public static readonly string ReplyToAddress = "ReplyToAddress";
        
        /// <summary>
        /// string - Originating address/queue.
        /// </summary>
        public static readonly string OriginatingAddress = "OriginatingAddress";
        
        /// <summary>
        /// string - Content type, commonly `application/json;charset=utf8`.
        /// </summary>
        public static readonly string ContentType = "ContentType";
        
        /// <summary>
        /// string - Optional contract namespace of message type, commonly `CONTRACT_NAMESPACE.CONTRACT_TYPE`.
        /// </summary>
        public static readonly string MessageType = "MessageType";
        
        /// <summary>
        /// string - Message intent - `command` or `event`.
        /// </summary>
        public static readonly string Intent = "Intent";
        
        /// <summary>
        /// string - Timestamp at what time the message was sent.
        /// </summary>
        public static readonly string TimeSent = "TimeSent";
        
        /// <summary>
        /// string - Error details attached in case of an exception.
        /// </summary>
        public static readonly string ErrorDetails = "ErrorDetails";
        
        /// <summary>
        /// string - Source queue of a message the has been forwarded to an error queue after it has failed.
        /// </summary>
        public static readonly string SourceQueue = "SourceQueue";
        
        public static Dictionary<string, string> GetDefaultRebusToStandardMap(string headerPrefix)
        {
            return new Dictionary<string, string>
            {
                {Headers.MessageId, $"{headerPrefix}.{MessageId}"},
                {Headers.CorrelationId, $"{headerPrefix}.{CorrelationId}"},
                {Headers.SenderAddress, $"{headerPrefix}.{ReplyToAddress}"},
                {Headers.ReturnAddress, $"{headerPrefix}.{OriginatingAddress}"},
                {Headers.ContentType, $"{headerPrefix}.{ContentType}"},
                {Headers.Type, $"{headerPrefix}.{MessageType}"},
                {Headers.Intent, $"{headerPrefix}.{Intent}"},
                {Headers.SentTime, $"{headerPrefix}.{TimeSent}"},
                {Headers.ErrorDetails, $"{headerPrefix}.{ErrorDetails}"},
                {Headers.SourceQueue, $"{headerPrefix}.{SourceQueue}"}
            };
        }

        public string StandardHeaderPrefix { get; set; }

        public Dictionary<string, string> RebusToStandardMap { get; set; }

        public IStandardIntentOptions StandardIntentOptions { get; set; }
        public IStandardDateTimeConverter StandardDateTimeConverter { get; set; }
    }
}