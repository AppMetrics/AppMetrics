using App.Metrics;
using App.Metrics.Data;
using App.Metrics.Internal;
using App.Metrics.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Facts.Integration.Startup
{
    public class FitleredMetricsEndpointStartup : TestStartup
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
                DisableMetrics = false,
                MetricsFilter = new DefaultMetricsFilter().WhereType(MetricType.Counter)
            };

            var aspNetMetricsOptions = new AspNetMetricsOptions();

            SetupServices(services, appMetricsOptions, aspNetMetricsOptions);
        }
    }
}