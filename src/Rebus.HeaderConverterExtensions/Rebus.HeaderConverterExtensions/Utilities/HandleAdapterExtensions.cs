using Rebus.Config;
using Rebus.HeaderConverterExtensions.HeaderStandard;
using Rebus.Pipeline.Receive;

namespace Rebus.HeaderConverterExtensions.Utilities
{
    public static class HandleAdapterExtensions
    {
        public static OptionsConfigurer HandleMessageAddStandardHeaders(this OptionsConfigurer configurer, IStandardHeaderOptions standardHeaderOptions)
        {
            return configurer
                .RegisterIncomingStep(new HandleStandardAdapterIncomingStep(standardHeaderOptions),
                    anchorStep: typeof(DeserializeIncomingMessageStep))
                .RegisterOutgoingStep(new HandleStandardAdapterOutgoingStep(standardHeaderOptions));
        }
    }
}