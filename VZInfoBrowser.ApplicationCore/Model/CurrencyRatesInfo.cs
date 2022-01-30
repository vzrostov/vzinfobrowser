using System;
using System.Collections.Generic;
using System.Globalization;

namespace VZInfoBrowser.ApplicationCore.Model
{
    public class CurrencyRatesInfo
    {
        public CurrencyRatesInfo() { }
        public string? Base { get; set; }
        public int Timestamp { get; set; }

        public string TimestampAsString { get
            {
                if (Timestamp == 0)
                    return "";
                DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return dateTime.AddSeconds(Timestamp).ToLocalTime().ToString("U", CultureInfo.GetCultureInfo("en-US"));
            } 
        }
        public Dictionary<string, double> Rates { get; set; } = new();
    }
}
