using App.Metrics;
using App.Metrics.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Integration.Facts.Startup
{
    public class DisabledHealthCheckTestStartup : TestStartup
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
                DisableMetrics = false,
                Clock = new Clock.TestClock(),
                DefaultSamplingType = SamplingType.LongTerm
            };

            var aspNetMetricsOptions = new AspNetMetricsOptions
            {
                MetricsTextEndpointEnabled = true,
                HealthEndpointEnabled = false,
                MetricsEndpointEnabled = true,
                PingEndpointEnabled = true
            };

            SetupServices(services, appMetricsOptions, aspNetMetricsOptions);
        }
    }
}