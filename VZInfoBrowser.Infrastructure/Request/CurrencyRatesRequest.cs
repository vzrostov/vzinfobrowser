using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using VZInfoBrowser.ApplicationCore;
using VZInfoBrowser.ApplicationCore.Model;

namespace VZInfoBrowser.Infrastructure
{

    public class CurrencyRatesRequest : ICurrencyRatesRequest
    {
        private const int RealTimerPeriodValue = (60 * 60 * 6); // 4 times per a day - real query

        private readonly IHttpClientFactory _httpClientFactory = null!;
        private readonly IConfiguration _configuration = null!;
        private readonly ILogger<CurrencyRatesRequest> _logger = null!;
        private readonly ICurrentInfoRepository? _settings = null;

        public event WorkOut OnWorkOut;

        public CurrencyRatesRequest() { }

        public CurrencyRatesRequest(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<CurrencyRatesRequest> logger,
            ICurrentInfoRepository settings) =>
            (_httpClientFactory, _configuration, _logger, _settings) =
                (httpClientFactory, configuration, logger, settings);

        public void MakeCurrencyRatesAsync()
        {
            var t = GetCurrencyRatesAsync();
            if (t == null || t.Result == null)
                return;
            OnWorkOut(t.Result);
        }

        async Task<CurrencyRatesInfo?> GetCurrencyRatesAsync()
        {
            _logger.LogInformation("Query: just timer fired");
            // to not allow a big number of quieries - to check needs to ask
            if (!CheckNeedsToAsk())
                return null;
            // Create the client
            HttpClient client = _httpClientFactory.CreateClient(_configuration.GetSection("Services:Name").Value); // TODO

            try
            {
                _logger.LogInformation("Query: real one goes");
                
                // Make HTTP GET request
                HttpResponseMessage res = await client.GetAsync("");
                return await TryParseResponse(res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting something fun to say: {Error}", ex);
            }
            return null;
        }

        public static async Task<CurrencyRatesInfo> TryParseResponse(HttpResponseMessage res)
        {
            if (res.StatusCode == HttpStatusCode.NoContent)
            {
                Console.WriteLine("HttpStatusCode.NoContent");
                throw new Exception("HttpStatusCode.NoContent");
            }
            else
                if (res.IsSuccessStatusCode)
                {
                    // Parse JSON response deserialize 
                    return await res.Content.ReadFromJsonAsync<CurrencyRatesInfo>();
                }
                else
                {
                    string msg = await res.Content.ReadAsStringAsync();
                    Console.WriteLine(msg);
                    throw new Exception(msg);
                }
        }

        private bool CheckNeedsToAsk()
        {
            var l = _settings?.Load();
            if (l == null || l.Timestamp == 0)
                return true;
            long diff = Math.Abs(l.Timestamp - ConvertToTimestamp(DateTime.Now));
            return diff > RealTimerPeriodValue; // if previous timestamp is some hours older than timenow
        }

        private static long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }
    }
}
