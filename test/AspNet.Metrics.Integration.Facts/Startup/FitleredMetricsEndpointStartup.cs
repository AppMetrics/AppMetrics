using App.Metrics;
using App.Metrics.Configuration;
using App.Metrics.Data;
using App.Metrics.Internal;
using AspNet.Metrics.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Integration.Facts.Startup
{
    public class FitleredMetricsEndpointStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: AH - Need to setup global filtering option

            var appMetricsOptions = new AppMetricsOptions
            {
                DefaultSamplingType = SamplingType.LongTerm,
                MetricsEnabled = true,
            };

            var aspNetMetricsOptions = new AspNetMetricsOptions();

            SetupServices(services, appMetricsOptions, aspNetMetricsOptions,
                new DefaultMetricsFilter().WhereType(MetricType.Counter));
        }
    }
}