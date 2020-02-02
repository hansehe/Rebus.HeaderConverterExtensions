using System.Collections.Generic;
using Rebus.Messages;

namespace Rebus.HeaderConverterExtensions.HeaderStandard
{
    public class DefaultStandardIntentOptions : IStandardIntentOptions
    {
        public DefaultStandardIntentOptions(string standardIntentKey, 
            Dictionary<string,string> rebusIntentToStandardIntentMap = null)
        {
            StandardIntentKey = standardIntentKey;
            RebusIntentToStandardIntentMap = rebusIntentToStandardIntentMap ?? GetDefaultRebusIntentToStandardIntentMap();
        }
        
        /// <summary>
        /// string - Command intent used when sending a message from point to point.
        /// </summary>
        public const string Command = "command";
        
        /// <summary>
        /// string - Event intent used when publishing a message.
        /// </summary>
        public const string Event = "event";

        public static Dictionary<string, string> GetDefaultRebusIntentToStandardIntentMap()
        {
            return new Dictionary<string, string>
            {
                { Headers.IntentOptions.PointToPoint, Command },
                { Headers.IntentOptions.PublishSubscribe, Event },
            };
        }
        
        public string StandardIntentKey { get; set; }
        public Dictionary<string,string> RebusIntentToStandardIntentMap { get; set; }
    }
}