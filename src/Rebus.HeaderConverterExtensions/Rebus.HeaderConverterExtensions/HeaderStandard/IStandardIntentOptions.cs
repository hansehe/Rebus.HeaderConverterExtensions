using System.Collections.Generic;

namespace Rebus.HeaderConverterExtensions.HeaderStandard
{
    public interface IStandardIntentOptions
    {
        /// <summary>
        /// Standard Intent Key.
        /// </summary>
        string StandardIntentKey { get; }
        
        /// <summary>
        /// Rebus Intent Keys => Standard Intent Keys.
        /// </summary>
        Dictionary<string,string> RebusIntentToStandardIntentMap { get; }
    }
}