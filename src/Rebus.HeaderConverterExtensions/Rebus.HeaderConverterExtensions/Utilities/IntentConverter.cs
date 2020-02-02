using System.Linq;
using Rebus.HeaderConverterExtensions.HeaderStandard;

namespace Rebus.HeaderConverterExtensions.Utilities
{
    public static class IntentConverter
    {
        public static string ConvertToStandardIntent(string intent, IStandardIntentOptions standardIntentOptions)
        {
            return standardIntentOptions.RebusIntentToStandardIntentMap.ContainsKey(intent) ? 
                standardIntentOptions.RebusIntentToStandardIntentMap[intent] : intent;
        }
        
        public static string ConvertToRebusIntent(string intent, IStandardIntentOptions standardIntentOptions)
        {
            var standardIntentToRebusIntentMap = standardIntentOptions.RebusIntentToStandardIntentMap.ToDictionary(k=>k.Value, v=>v.Key);
            return standardIntentToRebusIntentMap.ContainsKey(intent) ? 
                standardIntentToRebusIntentMap[intent] : intent;
        }
    }
}