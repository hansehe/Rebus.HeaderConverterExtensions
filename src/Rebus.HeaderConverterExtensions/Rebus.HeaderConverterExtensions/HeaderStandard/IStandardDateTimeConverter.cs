using System;

namespace Rebus.HeaderConverterExtensions.HeaderStandard
{
    public interface IStandardDateTimeConverter
    {
        /// <summary>
        /// Standard TimeSent Key.
        /// </summary>
        string StandardTimeSentKey { get; }
        
        string ToStandardHeaderValidString(DateTime dateTime);
        
        string ToRebusHeaderValidString(string dateTimeString);
    }
}