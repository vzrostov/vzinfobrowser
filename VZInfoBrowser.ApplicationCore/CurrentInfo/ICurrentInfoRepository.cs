using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VZInfoBrowser.ApplicationCore.Model;

namespace VZInfoBrowser.ApplicationCore
{
    /// <summary>
    /// Save and load current exchange rates. This entity is NOT for presentation layer
    /// </summary>
    public interface ICurrentInfoRepository
    {
        CurrencyRatesInfo? Load();
        bool Save(CurrencyRatesInfo? info);
    }
}
