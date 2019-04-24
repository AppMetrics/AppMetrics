// <copyright file="HealthyHealthTestStartup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Health.Endpoints;
using App.Metrics.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Startup
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class HealthyHealthTestStartup : TestStartup
        // ReSharper restore ClassNeverInstantiated.Global
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHealthEndpoint();

            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsMiddlewareHelathCheckOptions = new HealthEndpointsOptions();

            SetupServices(
                services,
                appMetricsMiddlewareHelathCheckOptions,
                healthChecks: new[] { HealthCheckResult.Healthy() });
        }
    }
}