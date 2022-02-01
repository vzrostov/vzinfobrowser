using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VZInfoBrowser.ApplicationCore;
using VZInfoBrowser.ApplicationCore.Model;
using VZInfoBrowser.Infrastructure;
using Xunit;

namespace VZInfoBrowser.Tests
{
    public class QueryTest
    {
        // environment
        private readonly Mock<IHttpClientFactory> _mock_httpClientFactory = new ();
        private readonly Mock<IConfiguration> _mock_configuration = new ();
        private readonly Mock<ILogger<CurrencyRatesRequest>> _mock_logger = new ();
        // we use and receive
        private readonly CurrencyRatesRequest currencyRatesRequest = null;
        private readonly string responseStr = "{\n  \"disclaimer\": \"Usage subject to terms: https://openexchangerates.org/terms\",\n  \"license\": \"https://openexchangerates.org/license\",\n  \"timestamp\": 1611013600,\n  \"base\": \"USD\",\n  \"rates\": {\n    \"AED\": 3.67299,\n    \"AFN\": 105.039905,\n    \"ALL\": 107.220809,\n    \"AMD\": 481.763527,\n    \"ANG\": 1.809724,\n    \"AOA\": 535.498,\n    \"ARS\": 103.855895,\n    \"AUD\": 1.385802,\n    \"AWG\": 1.8,\n    \"AZN\": 1.7,\n    \"BAM\": 1.714813,\n    \"BBD\": 2,\n    \"BDT\": 86.266783,\n    \"BGN\": 1.71173,\n    \"BHD\": 0.37698,\n    \"BIF\": 2000.21835,\n    \"BMD\": 1,\n    \"BND\": 1.345334,\n    \"BOB\": 6.91319,\n    \"BRL\": 5.535001,\n    \"BSD\": 1,\n    \"BTC\": 0.000023301879,\n    \"BTN\": 74.094295,\n    \"BWP\": 11.568766,\n    \"BYN\": 2.589026,\n    \"BZD\": 2.023945,\n    \"CAD\": 1.251085,\n    \"CDF\": 2008.251161,\n    \"CHF\": 0.913554,\n    \"CLF\": 0.029707,\n    \"CLP\": 819.71,\n    \"CNH\": 6.35188,\n    \"CNY\": 6.3472,\n    \"COP\": 3977.335614,\n    \"CRC\": 639.43405,\n    \"CUC\": 1,\n    \"CUP\": 25.75,\n    \"CVE\": 96.7,\n    \"CZK\": 21.4057,\n    \"DJF\": 178.756634,\n    \"DKK\": 6.514054,\n    \"DOP\": 57.864811,\n    \"DZD\": 139.964742,\n    \"EGP\": 15.743379,\n    \"ERN\": 15.00001,\n    \"ETB\": 49.891851,\n    \"EUR\": 0.875362,\n    \"FJD\": 2.10975,\n    \"FKP\": 0.73103,\n    \"GBP\": 0.73103,\n    \"GEL\": 3.085,\n    \"GGP\": 0.73103,\n    \"GHS\": 6.255677,\n    \"GIP\": 0.73103,\n    \"GMD\": 53.05,\n    \"GNF\": 9087.09055,\n    \"GTQ\": 7.705409,\n    \"GYD\": 210.075488,\n    \"HKD\": 7.78905,\n    \"HNL\": 24.687321,\n    \"HRK\": 6.5828,\n    \"HTG\": 103.654227,\n    \"HUF\": 311.501203,\n    \"IDR\": 14328.85,\n    \"ILS\": 3.10854,\n    \"IMP\": 0.73103,\n    \"INR\": 74.242196,\n    \"IQD\": 1465.491897,\n    \"IRR\": 42250,\n    \"ISK\": 128.5,\n    \"JEP\": 0.73103,\n    \"JMD\": 155.36527,\n    \"JOD\": 0.709,\n    \"JPY\": 114.5015,\n    \"KES\": 113.45,\n    \"KGS\": 84.779801,\n    \"KHR\": 4073.766499,\n    \"KMF\": 430.624801,\n    \"KPW\": 900,\n    \"KRW\": 1191.434105,\n    \"KWD\": 0.302057,\n    \"KYD\": 0.836713,\n    \"KZT\": 437.177066,\n    \"LAK\": 11336.239102,\n    \"LBP\": 1518.224011,\n    \"LKR\": 203.703925,\n    \"LRD\": 149.749962,\n    \"LSL\": 15.386328,\n    \"LYD\": 4.590788,\n    \"MAD\": 9.249769,\n    \"MDL\": 18.093631,\n    \"MGA\": 4000.830018,\n    \"MKD\": 53.974144,\n    \"MMK\": 1785.412948,\n    \"MNT\": 2860.505582,\n    \"MOP\": 8.051326,\n    \"MRU\": 36.458567,\n    \"MUR\": 43.5,\n    \"MVR\": 15.45,\n    \"MWK\": 820.360395,\n    \"MXN\": 20.30082,\n    \"MYR\": 4.1835,\n    \"MZN\": 63.841999,\n    \"NAD\": 15.35,\n    \"NGN\": 415.5,\n    \"NIO\": 35.598869,\n    \"NOK\": 8.73606,\n    \"NPR\": 119.066614,\n    \"NZD\": 1.46872,\n    \"OMR\": 0.384509,\n    \"PAB\": 1,\n    \"PEN\": 3.910971,\n    \"PGK\": 3.525657,\n    \"PHP\": 51.222501,\n    \"PKR\": 176.583204,\n    \"PLN\": 3.958399,\n    \"PYG\": 6987.240138,\n    \"QAR\": 3.640801,\n    \"RON\": 4.3274,\n    \"RSD\": 102.923885,\n    \"RUB\": 76.45905,\n    \"RWF\": 1042.437277,\n    \"SAR\": 3.751329,\n    \"SBD\": 8.054878,\n    \"SCR\": 13.79908,\n    \"SDG\": 437.5,\n    \"SEK\": 9.028356,\n    \"SGD\": 1.348092,\n    \"SHP\": 0.73103,\n    \"SLL\": 11375.197091,\n    \"SOS\": 580.873109,\n    \"SRD\": 21.303,\n    \"SSP\": 130.26,\n    \"STD\": 21108.640504,\n    \"STN\": 21.68,\n    \"SVC\": 8.78615,\n    \"SYP\": 2512,\n    \"SZL\": 15.388925,\n    \"THB\": 33.094846,\n    \"TJS\": 11.29205,\n    \"TMT\": 3.5,\n    \"TND\": 2.885,\n    \"TOP\": 2.267956,\n    \"TRY\": 13.5048,\n    \"TTD\": 6.815426,\n    \"TWD\": 27.536201,\n    \"TZS\": 2308,\n    \"UAH\": 28.086632,\n    \"UGX\": 3539.513493,\n    \"USD\": 1,\n    \"UYU\": 44.568812,\n    \"UZS\": 10862.411042,\n    \"VES\": 4.6301,\n    \"VND\": 22729.855959,\n    \"VUV\": 113.203707,\n    \"WST\": 2.599432,\n    \"XAF\": 574.199591,\n    \"XAG\": 0.04340285,\n    \"XAU\": 0.00054895,\n    \"XCD\": 2.70255,\n    \"XDR\": 0.712634,\n    \"XOF\": 574.199591,\n    \"XPD\": 0.0005287,\n    \"XPF\": 104.458429,\n    \"XPT\": 0.00102723,\n    \"YER\": 250.249937,\n    \"ZAR\": 15.40188,\n    \"ZMW\": 17.194938,\n    \"ZWL\": 322\n  }\n}";
        private readonly Task<CurrencyRatesInfo> res = null;

        public QueryTest()
        {
            currencyRatesRequest = new CurrencyRatesRequest(_mock_httpClientFactory.Object, _mock_configuration.Object, _mock_logger.Object, null);
            res = CurrencyRatesRequest.TryParseResponse(new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(responseStr) });
        }

        [Fact]
        public void QueryTestForGoodResult()
        {
            Assert.NotNull(res);
            Assert.NotNull(res.Result);
            var ci = res.Result;
            Assert.True(res.Exception == null);
            Assert.Equal(1611013600, ci.Timestamp);
            Assert.Equal("USD", ci.Base);
            Assert.Equal(169, ci.Rates.Count);
            ci.Rates.TryGetValue("XOF", out var d);
            Assert.Equal(574.199591, d);
        }

        [Fact]
        public void QueryTestForBadResult()
        {
            Task<CurrencyRatesInfo> tn = null;
            tn = CurrencyRatesRequest.TryParseResponse(new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(responseStr[1..^1]) });
            Assert.NotNull(tn);
            Assert.True(tn.Exception != null);
        }

        [Fact]
        public void LoadSaveCurrentInfo()
        {
            CurrencyRatesInfo CurrencyRatesInfo = new ();
            CurrencyRatesInfo.Base = "UAE";
            CurrencyRatesInfo.Timestamp = 8989;
            CurrencyRatesInfo.Rates.Add("RUB", 3.4);
            ICurrentInfoRepository settings = new CurrentInfoRepository("test");
            settings.Save(CurrencyRatesInfo);
            var newCurr = settings.Load();
            Assert.Equal(CurrencyRatesInfo.Base, newCurr.Base);
            Assert.Equal(CurrencyRatesInfo.Timestamp, newCurr.Timestamp);
            Assert.Single(CurrencyRatesInfo.Rates);
            Assert.True(CurrencyRatesInfo.Rates.ContainsKey("RUB"));
            Assert.True(CurrencyRatesInfo.Rates.TryGetValue("RUB", out var val));
            Assert.Equal(3.4, val);
        }
    }
}
