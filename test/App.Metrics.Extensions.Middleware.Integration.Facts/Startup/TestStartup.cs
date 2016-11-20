using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using App.Metrics.Configuration;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using App.Metrics.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Startup
{
    public abstract class TestStartup
    {
        public IMetrics Metrics { get; private set; }

        protected void SetupAppBuilder(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMetrics();

            app.Use((context, func) =>
            {
                var clientId = string.Empty;

                if (context.Request.Path.Value.Contains("oauth"))
                {
                    clientId = context.Request.Path.Value.Split('/').Last();
                }

                if (clientId.IsPresent())
                {
                    context.User =
                        new ClaimsPrincipal(new List<ClaimsIdentity>
                        {
                            new ClaimsIdentity(new[]
                            {
                                new Claim("client_id", clientId)
                            })
                        });
                }

                return func();
            });

            Metrics = app.ApplicationServices.GetRequiredService<IMetrics>();

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
                    options.MetricsEnabled = appMetricsOptions.MetricsEnabled;
                    options.DefaultSamplingType = appMetricsOptions.DefaultSamplingType;
                })
                .AddJsonSerialization()
                .AddClockType<TestClock>()
                .AddHealthChecks()
                .AddMetricsMiddleware(options =>
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

                    options.IgnoredRoutesRegexPatterns = aspNetMetricsOptions.IgnoredRoutesRegexPatterns;
                });

            if (filter != null)
            {
                builder.AddGlobalFilter(filter);
            }
        }
    }
}