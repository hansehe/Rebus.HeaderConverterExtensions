using System;
using System.Threading.Tasks;
using Rebus.HeaderConverterExtensions.HeaderStandard;
using Rebus.Messages;
using Rebus.Pipeline;

namespace Rebus.HeaderConverterExtensions.Utilities
{
    public class HandleStandardAdapterIncomingStep : IIncomingStep
    {
        private readonly IStandardHeaderOptions StandardHeaderOptions;

        public HandleStandardAdapterIncomingStep(IStandardHeaderOptions standardHeaderOptions)
        {
            StandardHeaderOptions = standardHeaderOptions;
        }
        
        public async Task Process(IncomingStepContext context, Func<Task> next)
        {
            var transportMessage = context.Load<TransportMessage>();
            if (StandardAdapter.IsUsableOnIncoming(transportMessage, StandardHeaderOptions))
            {
                transportMessage = StandardAdapter.ConvertIncomingTransportMessage(transportMessage, StandardHeaderOptions);
                context.Save(transportMessage);
            }
            
            await next();
        }
    }
}