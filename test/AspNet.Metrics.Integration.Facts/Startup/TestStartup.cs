using App.Metrics;
using App.Metrics.DependencyInjection;
using App.Metrics.Internal;
using Microsoft.AspNet.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Integration.Facts.Startup
{
    public abstract class TestStartup
    {
        protected void SetupAppBuilder(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMetrics();

            app.UseMvc();
        }

        protected void SetupServices(IServiceCollection services,
            AppMetricsOptions appMetricsOptions,
            AspNetMetricsOptions aspNetMetricsOptions,
            IMetricsFilter filter = null)
        {
            services
                .AddLogging()
                .AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            var builder = services
                .AddMetrics(options =>
                {
                    options.DefaultContextLabel = appMetricsOptions.DefaultContextLabel;
                    options.DisableMetrics = appMetricsOptions.DisableMetrics;
                    options.Clock = appMetricsOptions.Clock;
                    options.DefaultSamplingType = appMetricsOptions.DefaultSamplingType;
                })
                .AddHealthChecks()
                .AddAspNetMetrics(options =>
                {
                    options.MetricsTextEndpointEnabled = aspNetMetricsOptions.MetricsTextEndpointEnabled;
                    options.HealthEndpointEnabled = aspNetMetricsOptions.HealthEndpointEnabled;
                    options.MetricsEndpointEnabled = aspNetMetricsOptions.MetricsEndpointEnabled;
                    options.PingEndpointEnabled = aspNetMetricsOptions.PingEndpointEnabled;
                    options.OAuth2TrackingEnabled = aspNetMetricsOptions.OAuth2TrackingEnabled;

                    options.HealthEndpoint = aspNetMetricsOptions.HealthEndpoint;
                    options.MetricsEndpoint = aspNetMetricsOptions.MetricsEndpoint;
                    options.MetricsTextEndpoint = aspNetMetricsOptions.MetricsTextEndpoint;
                    options.PingEndpoint = aspNetMetricsOptions.PingEndpoint;
                });

            if (filter != null)
            {
                builder.AddGlobalFilter(filter);
            }
        }
    }
}