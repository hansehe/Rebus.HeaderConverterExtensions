using System;
using System.Globalization;

namespace Rebus.HeaderConverterExtensions.HeaderStandard
{
    public class DefaultStandardDateTimeConverter : IStandardDateTimeConverter
    {
        public DefaultStandardDateTimeConverter(string standardTimeSentKey)
        {
            StandardTimeSentKey = standardTimeSentKey;
        }
        
        public string StandardTimeSentKey { get; set; }

        public string ToStandardHeaderValidString(DateTime dateTime)
        {
            return dateTime.ToString(CultureInfo.InvariantCulture);
        }
        
        public string ToRebusHeaderValidString(string dateTimeString)
        {
            return dateTimeString;
        }
    }
}