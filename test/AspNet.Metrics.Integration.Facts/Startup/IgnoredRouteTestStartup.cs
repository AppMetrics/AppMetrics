using App.Metrics;
using App.Metrics.Configuration;
using AspNet.Metrics.DependencyInjection.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Integration.Facts.Startup
{
    public class IgnoredRouteTestStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsOptions = new AppMetricsOptions
            {
                DefaultContextLabel = "testing",
                MetricsEnabled = true,
                DefaultSamplingType = SamplingType.LongTerm
            };

            var aspNetMetricsOptions = new AspNetMetricsOptions
            {
                MetricsTextEndpointEnabled = true,
                HealthEndpointEnabled = true,
                MetricsEndpointEnabled = true,
                PingEndpointEnabled = true                
            };

            aspNetMetricsOptions.IgnoredRoutesRegexPatterns.Add("(?i)^api/test/ignore");

            SetupServices(services, appMetricsOptions, aspNetMetricsOptions);
        }
    }
}