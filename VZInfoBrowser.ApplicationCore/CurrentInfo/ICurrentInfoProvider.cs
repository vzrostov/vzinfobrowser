using VZInfoBrowser.ApplicationCore.Model;

namespace VZInfoBrowser.ApplicationCore
{
    /// <summary>
    /// Get current exchange rates. This entity is for presentation layer
    /// </summary>
    public interface ICurrentInfoProvider
    {
        CurrencyRatesInfo CurrentInfo { get; }
    }
}
