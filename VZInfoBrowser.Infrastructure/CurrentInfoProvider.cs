using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VZInfoBrowser.ApplicationCore;
using VZInfoBrowser.ApplicationCore.Model;

namespace VZInfoBrowser.Infrastructure
{
    public class CurrentInfoProvider : ICurrentInfoProvider
    {
        public CurrencyRatesInfo CurrentInfo => throw new NotImplementedException();
    }
}
