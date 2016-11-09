using App.Metrics;
using App.Metrics.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Facts.Startup
{
    public class DisabledMetricsEndpointStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsOptions = new AppMetricsOptions
            {
                Clock = new Clock.TestClock(),
                DefaultSamplingType = SamplingType.LongTerm,
                DisableMetrics = false
            };

            var aspNetMetricsOptions = new AspNetMetricsOptions
            {
                MetricsEndpointEnabled = false
            };

            SetupServices(services, appMetricsOptions, aspNetMetricsOptions);
        }
    }
}