// <copyright file="TestStartup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Health.Endpoints;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
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
            app.UseHealthEndpoint();
        }

        protected void SetupServices(
            IServiceCollection services,
            HealthEndpointsOptions healthMiddlewareCoreChecksOptions,
            IEnumerable<HealthCheckResult> healthChecks = null)
        {
            services.AddOptions();
            services.AddLogging();

            // TODO: scan for healthchecks
            // var startupAssemblyName = typeof(TestStartup).Assembly.GetName().Name;

            var builder = new HealthBuilder()
                .Configuration.Configure(options => options.Enabled = true)
                .OutputHealth.AsPlainText()
                .OutputHealth.AsJson();

            var checks = healthChecks?.ToList() ?? new List<HealthCheckResult>();

            for (var i = 0; i < checks.Count; i++)
            {
                var check = checks[i];
                builder.HealthChecks.AddCheck("Check" + i, () => new ValueTask<HealthCheckResult>(check));
            }

            services.AddHealth(builder)
                .AddHealthEndpoints(
                        options =>
                        {
                            options.HealthEndpointEnabled = healthMiddlewareCoreChecksOptions.HealthEndpointEnabled;
                            options.PingEndpointEnabled = healthMiddlewareCoreChecksOptions.PingEndpointEnabled;
                            options.Timeout = healthMiddlewareCoreChecksOptions.Timeout;
                        });
        }
    }
}