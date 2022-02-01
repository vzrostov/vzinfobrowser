using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using StackExchange.Redis;
using System;
using System.Net.Http;
using VZInfoBrowser.ApplicationCore;
using VZInfoBrowser.ApplicationCore.Model;
using VZInfoBrowser.Infrastructure;
using VZInfoBrowser.Requests;

namespace VZInfoBrowser
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // read config 
            var servicesOptions = new ServicesOptions();
            var exchangeRatesServiceOptions = new ExchangeRatesServiceOptions();
            Configuration.GetSection($"{servicesOptions.Title}").Bind(exchangeRatesServiceOptions);
            var redisOptions = new RedisOptions();
            Configuration.GetSection($"{redisOptions.Title}").Bind(redisOptions);

            // prepare timers, services for info getting  
            services.AddHttpClient(exchangeRatesServiceOptions.Name, 
                c => c.BaseAddress =
                    new Uri($"{exchangeRatesServiceOptions.Host}" +
                    (string.IsNullOrEmpty(exchangeRatesServiceOptions.Port)? "" : $":{exchangeRatesServiceOptions.Port}") 
                    + $"{exchangeRatesServiceOptions.Endpoint}"))
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5)) 
                    .AddPolicyHandler(GetRetryPolicy());

            // hide it below
            //services.AddSingleton<ICurrentInfoProvider, CurrentInfoRepository>();
            services.AddSingleton<ICurrentInfoRepository, CurrentInfoRepository>();
            services.AddSingleton<ICurrencyRatesRequest, CurrencyRatesRequest>();
            services.AddHostedService<TimedHostedService>();
            // new ones
            services.AddSingleton<ICurrentInfoProvider, CurrentInfoProvider>();

            // prepare timers, services for info saving  
            //var multiplexer = ConnectionMultiplexer.Connect($"{redisOptions.Host}" +
            //    (string.IsNullOrEmpty(redisOptions.Port) ? "" : $":{redisOptions.Port}") 
            //    + $",abortConnect=false,connectTimeout=30000,responseTimeout=30000");
            //services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
