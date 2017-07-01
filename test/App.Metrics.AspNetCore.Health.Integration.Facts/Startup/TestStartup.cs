// <copyright file="TestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Health.Options;
using App.Metrics.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Startup
{
    public abstract class TestStartup
    {
        protected void SetupAppBuilder(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHealthChecks();
        }

        protected void SetupServices(
            IServiceCollection services,
            AppMetricsMiddlewareHealthChecksOptions appMetricsMiddlewareHealthChecksOptions,
            IEnumerable<HealthCheckResult> healthChecks = null)
        {
            services
                .AddLogging()
                .AddRouting(options => { options.LowercaseUrls = true; });

            services
                .AddHealthChecks()
                .AddHealthCheckMiddleware(
                    options =>
                    {
                        options.HealthEndpointEnabled = appMetricsMiddlewareHealthChecksOptions.HealthEndpointEnabled;
                        options.HealthEndpoint = appMetricsMiddlewareHealthChecksOptions.HealthEndpoint;
                    },
                    optionsBuilder =>
                    {
                        optionsBuilder.AddJsonFormatters();
                    })
                .AddChecks(
                    factory =>
                    {
                        var checks = healthChecks != null
                            ? healthChecks.ToList()
                            : new List<HealthCheckResult>();

                        for (var i = 0; i < checks.Count; i++)
                        {
                            var check = checks[i];
                            factory.Register("Check" + i, () => new ValueTask<HealthCheckResult>(check));
                        }
                    });
        }
    }
}