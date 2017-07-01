// <copyright file="TestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using App.Metrics.AspNetCore.Middleware.Options;
using App.Metrics.Builder;
using App.Metrics.Core.Configuration;
using App.Metrics.Core.Infrastructure;
using App.Metrics.Core.ReservoirSampling.Uniform;
using App.Metrics.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    public abstract class TestStartup
    {
        protected IMetrics Metrics { get; private set; }

        protected void SetupAppBuilder(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMetrics();

            app.Use(
                (context, func) =>
                {
                    var clientId = string.Empty;

                    if (context.Request.Path.Value.Contains("oauth"))
                    {
                        clientId = context.Request.Path.Value.Split('/').Last();
                    }

                    if (!string.IsNullOrWhiteSpace(clientId))
                    {
                        context.User =
                            new ClaimsPrincipal(
                                new List<ClaimsIdentity>
                                {
                                    new ClaimsIdentity(
                                        new[]
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

        protected void SetupServices(
            IServiceCollection services,
            AppMetricsOptions appMetricsOptions,
            AppMetricsMiddlewareOptions appMetricsMiddlewareOptions,
            IFilterMetrics filter = null)
        {
            services
                .AddLogging()
                .AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            var builder = services
                .AddMetrics(
                    options =>
                    {
                        options.DefaultContextLabel = appMetricsOptions.DefaultContextLabel;
                        options.MetricsEnabled = appMetricsOptions.MetricsEnabled;
                    })
                .AddDefaultReservoir(() => new DefaultAlgorithmRReservoir(1028))
                .AddClockType<TestClock>()
                .AddMetricsMiddleware(
                    options =>
                    {
                        options.MetricsTextEndpointEnabled = appMetricsMiddlewareOptions.MetricsTextEndpointEnabled;
                        options.MetricsEndpointEnabled = appMetricsMiddlewareOptions.MetricsEndpointEnabled;
                        options.PingEndpointEnabled = appMetricsMiddlewareOptions.PingEndpointEnabled;
                        options.OAuth2TrackingEnabled = appMetricsMiddlewareOptions.OAuth2TrackingEnabled;

                        options.MetricsEndpoint = appMetricsMiddlewareOptions.MetricsEndpoint;
                        options.MetricsTextEndpoint = appMetricsMiddlewareOptions.MetricsTextEndpoint;
                        options.PingEndpoint = appMetricsMiddlewareOptions.PingEndpoint;

                        options.IgnoredRoutesRegexPatterns = appMetricsMiddlewareOptions.IgnoredRoutesRegexPatterns;
                        options.IgnoredHttpStatusCodes = appMetricsMiddlewareOptions.IgnoredHttpStatusCodes;

                        options.DefaultTrackingEnabled = appMetricsMiddlewareOptions.DefaultTrackingEnabled;
                    },
                    optionsBuilder =>
                    {
                        optionsBuilder.AddMetricsJsonFormatters().AddMetricsTextAsciiFormatters();
                    });

            if (filter != null)
            {
                builder.AddGlobalFilter(filter);
            }
        }
    }
}