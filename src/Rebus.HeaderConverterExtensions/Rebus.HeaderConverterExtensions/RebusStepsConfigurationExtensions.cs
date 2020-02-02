using Rebus.Config;
using Rebus.HeaderConverterExtensions.HeaderStandard;
using Rebus.HeaderConverterExtensions.Utilities;

namespace Rebus.HeaderConverterExtensions
{
    public static class RebusStepsConfigurationExtensions
    {
        public static OptionsConfigurer AddStandardHeaderConverter(this OptionsConfigurer configurer, 
            IStandardHeaderOptions standardHeaderOptions = null)
        {
            standardHeaderOptions = standardHeaderOptions ?? new DefaultStandardHeaderOptions();
            // The steps are executed in the order they are registered.
            return configurer
                .HandleMessageAddStandardHeaders(standardHeaderOptions);
        }
    }
}