using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VZInfoBrowser.Infrastructure
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private const int TimerPeriodValue = (60*60*1); // every hour we try
        private const int AfterCreatingTimerPeriodValue = 2; // how many seconds we wait after start
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, 
                TimeSpan.FromSeconds(AfterCreatingTimerPeriodValue), 
                TimeSpan.FromSeconds(TimerPeriodValue));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var currencyRatesRequest = scope.ServiceProvider.GetRequiredService<ICurrencyRatesRequest>();
                currencyRatesRequest.MakeCurrencyRatesAsync();
            }

            var count = Interlocked.Increment(ref executionCount);
            _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
