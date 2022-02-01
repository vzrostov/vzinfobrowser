using VZInfoBrowser.ApplicationCore;
using VZInfoBrowser.ApplicationCore.Model;

namespace VZInfoBrowser.Infrastructure
{
    public class CurrentInfoProvider : ICurrentInfoProvider
    {
        readonly ICurrentInfoRepository _currentInfoRepository;
        public CurrentInfoProvider(ICurrencyRatesRequest currencyRatesRequest, ICurrentInfoRepository currentInfoRepository)
        {
            currencyRatesRequest.OnWorkOut += WorkOut;
            _currentInfoRepository = currentInfoRepository;
        }

        CurrencyRatesInfo? currentInfo;
        public CurrencyRatesInfo? CurrentInfo
        {
            get
            {
                currentInfo ??= _currentInfoRepository.Load();
                return currentInfo;
            }
            set
            {
                currentInfo = value;
            }
        }

        void WorkOut(CurrencyRatesInfo? currencyRatesInfo)
        {
            CurrentInfo = currencyRatesInfo;
            _currentInfoRepository.Save(currencyRatesInfo);
        }

    }
}
