using System;
using System.Collections.Generic;
using System.Linq;
using Rebus.HeaderConverterExtensions.HeaderStandard;
using Rebus.Messages;

namespace Rebus.HeaderConverterExtensions.Utilities
{
    public static class HeaderConverter
    {
        public static Dictionary<string, string> ConvertToStandardHeaders(Dictionary<string, string> rebusHeaders, IStandardHeaderOptions standardHeaderOptions)
        {
            var standardHeaders = new Dictionary<string,string>(rebusHeaders);

            foreach(var header in rebusHeaders
                .Where(h=>standardHeaderOptions.RebusToStandardMap.ContainsKey(h.Key)))
            {
                standardHeaders[standardHeaderOptions.RebusToStandardMap[header.Key]] = header.Value;
            }

            var standardIntentKey = standardHeaderOptions.StandardIntentOptions.StandardIntentKey;
            if (standardIntentKey != null && standardHeaders.ContainsKey(standardIntentKey))
            {
                standardHeaders[standardIntentKey] = 
                    IntentConverter.ConvertToStandardIntent(standardHeaders[standardIntentKey], standardHeaderOptions.StandardIntentOptions);
            }

            var standardTimeSentKey = standardHeaderOptions.StandardDateTimeConverter.StandardTimeSentKey;
            if (standardTimeSentKey != null)
            {
                standardHeaders[standardTimeSentKey] =
                    standardHeaderOptions.StandardDateTimeConverter.ToStandardHeaderValidString(DateTime.UtcNow);
            }
            
            return standardHeaders;
        }
        
        public static Dictionary<string, string> ConvertToRebusHeaders(Dictionary<string, string> standardHeaders, IStandardHeaderOptions standardHeaderOptions)
        {
            var rebusHeaders = new Dictionary<string,string>(standardHeaders);
            var standardToRebusMap = standardHeaderOptions.RebusToStandardMap.ToDictionary(k=>k.Value, v=>v.Key);

            foreach(var header in standardHeaders
                .Where(h=>standardToRebusMap.ContainsKey(h.Key)))
            {
                rebusHeaders[standardToRebusMap[header.Key]] = header.Value;
            }
            
            var standardIntentKey = standardHeaderOptions.StandardIntentOptions.StandardIntentKey;
            if (standardIntentKey != null && rebusHeaders.ContainsKey(standardIntentKey))
            {
                rebusHeaders[Headers.Intent] = 
                    IntentConverter.ConvertToRebusIntent(rebusHeaders[standardIntentKey], standardHeaderOptions.StandardIntentOptions);
            }
            
            var standardTimeSentKey = standardHeaderOptions.StandardDateTimeConverter.StandardTimeSentKey;
            if (standardTimeSentKey != null && rebusHeaders.ContainsKey(standardTimeSentKey))
            {
                rebusHeaders[Headers.SentTime] =
                    standardHeaderOptions.StandardDateTimeConverter.ToRebusHeaderValidString(rebusHeaders[standardTimeSentKey]);
            }

            return rebusHeaders;
        }
    }
}