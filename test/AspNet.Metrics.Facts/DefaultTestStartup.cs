using App.Metrics;
using App.Metrics.DependencyInjection;
using App.Metrics.Utils;
using Microsoft.AspNet.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Facts
{
    public class DefaultTestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMetrics();

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging()
                .AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            services
                .AddMetrics(options =>
                {
                    options.DefaultGroupName = "testing";
                    options.DisableMetrics = false;
                    options.Clock = new Clock.TestClock();
                    options.DefaultSamplingType = SamplingType.LongTerm;
                })
                .AddHealthChecks()
                .AddAspNetMetrics(options =>
                {
                    options.MetricsTextEndpointEnabled = true;
                    options.HealthEndpointEnabled = true;
                    options.MetricsEndpointEnabled = true;
                    options.PingEndpointEnabled = true;
                });
        }
    }

    public class DisabledMetricsTestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMetrics();

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging()
                .AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            services
                .AddMetrics(options =>
                {
                    options.DefaultGroupName = "testing";
                    options.DisableMetrics = true;
                    options.Clock = new Clock.TestClock();
                    options.DefaultSamplingType = SamplingType.LongTerm;
                })
                .AddHealthChecks()
                .AddAspNetMetrics(options =>
                {
                    options.MetricsTextEndpointEnabled = true;
                    options.HealthEndpointEnabled = true;
                    options.MetricsEndpointEnabled = true;
                    options.PingEndpointEnabled = true;
                });
        }
    }
}