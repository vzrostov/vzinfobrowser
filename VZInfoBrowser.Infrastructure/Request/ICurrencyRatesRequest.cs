using VZInfoBrowser.ApplicationCore.Model;

namespace VZInfoBrowser.Infrastructure
{
    public delegate void WorkOut(CurrencyRatesInfo? currencyRatesInfo);

    public interface ICurrencyRatesRequest
    {
        void MakeCurrencyRatesAsync();
        public event WorkOut OnWorkOut;
    }
}
