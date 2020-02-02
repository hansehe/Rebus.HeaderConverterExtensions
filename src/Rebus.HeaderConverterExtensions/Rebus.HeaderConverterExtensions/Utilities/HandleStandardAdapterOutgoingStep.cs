using System;
using System.Threading.Tasks;
using Rebus.HeaderConverterExtensions.HeaderStandard;
using Rebus.Messages;
using Rebus.Pipeline;

namespace Rebus.HeaderConverterExtensions.Utilities
{
    public class HandleStandardAdapterOutgoingStep : IOutgoingStep
    {
        private readonly IStandardHeaderOptions StandardHeaderOptions;

        public HandleStandardAdapterOutgoingStep(IStandardHeaderOptions standardHeaderOptions)
        {
            StandardHeaderOptions = standardHeaderOptions;
        }
        
        public Task Process(OutgoingStepContext context, Func<Task> next)
        {
            var transportMessage = context.Load<TransportMessage>();
            transportMessage = StandardAdapter.ConvertOutgoingTransportMessage(transportMessage, StandardHeaderOptions);
            context.Save(transportMessage);
            return next();
        }
    }
}