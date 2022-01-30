using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VZInfoBrowser.Infrastructure
{
    public interface ICurrencyRatesRequest
    {
        void MakeCurrencyRatesAsync();
    }
}
