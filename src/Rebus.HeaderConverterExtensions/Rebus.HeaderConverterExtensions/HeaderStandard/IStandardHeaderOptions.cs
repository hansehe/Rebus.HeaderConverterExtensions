using System.Collections.Generic;

namespace Rebus.HeaderConverterExtensions.HeaderStandard
{
    public interface IStandardHeaderOptions
    {
        /// <summary>
        /// Standard Header Prefix.
        /// </summary>
        string StandardHeaderPrefix { get; }
        
        /// <summary>
        /// Rebus Header Keys => Standard Header Keys.
        /// </summary>
        Dictionary<string,string> RebusToStandardMap { get; }
        
        /// <summary>
        /// Standard Intent Options.
        /// </summary>
        IStandardIntentOptions StandardIntentOptions { get; }
        
        /// <summary>
        /// Standard Datetime Converter.
        /// </summary>
        IStandardDateTimeConverter StandardDateTimeConverter { get; }
    }
}