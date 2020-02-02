using System.Linq;
using Rebus.HeaderConverterExtensions.HeaderStandard;
using Rebus.Messages;

namespace Rebus.HeaderConverterExtensions.Utilities
{
    public static class StandardAdapter
    {
        public static bool IsUsableOnIncoming(TransportMessage transportMessage, IStandardHeaderOptions standardHeaderOptions)
        {
            return transportMessage.Headers.Keys.Any(k=>k.StartsWith(standardHeaderOptions.StandardHeaderPrefix));
        }

        public static TransportMessage ConvertIncomingTransportMessage(TransportMessage incomingTransportMessage, IStandardHeaderOptions standardHeaderOptions)
        {
            var standardHeaders = incomingTransportMessage.Headers;
            var rebusHeaders = HeaderConverter.ConvertToRebusHeaders(standardHeaders, standardHeaderOptions);
            var updatedTransportMessage = new TransportMessage(rebusHeaders, incomingTransportMessage.Body);
            
            return updatedTransportMessage;
        }

        public static TransportMessage ConvertOutgoingTransportMessage(TransportMessage outgoingTransportMessage, IStandardHeaderOptions standardHeaderOptions)
        {
            var standardHeaders = HeaderConverter.ConvertToStandardHeaders(outgoingTransportMessage.Headers, standardHeaderOptions);
            var updatedTransportMessage = new TransportMessage(standardHeaders, outgoingTransportMessage.Body);
            return updatedTransportMessage;
        }
    }
}