// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using App.Metrics.Sandbox.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Sandbox
{
    public class Startup
    {
        private const bool HaveAppRunSampleRequests = true;

        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            app.UseTestStuff(lifetime, HaveAppRunSampleRequests);

            app.UseMetrics();
            app.UseHealthChecks();
            app.UseMetricsReporting(lifetime);

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTestStuff();

            services.AddMvc(options => options.AddMetricsResourceFilter());

            services.AddMetrics(Configuration.GetSection("AppMetrics")).
                     AddSandboxReporting().
                     AddMetricsMiddleware(
                         Configuration.GetSection("AspNetMetrics"),
                         optionsBuilder =>
                         {
                             optionsBuilder.AddMetricsJsonFormatters().
                                            AddMetricsTextAsciiFormatters().
                                            AddEnvironmentAsciiFormatters();
                         });

            services.
                AddHealthChecks().
                // AddHealthCheckMetrics().
                AddHealthCheckMiddleware(optionsBuilder => optionsBuilder.AddAsciiFormatters()).
                AddChecks(
                    registry =>
                    {
                        registry.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                        registry.AddHttpGetCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));

                        // registry.AddOveralWebRequestsApdexCheck(metrics);
                        // registry.AddMetricCheck(
                        //     name: "Database Call Duration",
                        //     metrics: metrics,
                        //     options: SandboxMetricsRegistry.DatabaseTimer,
                        //     tags: new MetricTags("client_id", "client-9"),
                        //     passing: value => { return (message: $"OK. 98th Percentile < 100ms ({value.Histogram.Percentile98}{SandboxMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})", result: value.Histogram.Percentile98 < 100); },
                        //     warning: value => { return (message: $"WARNING. 98th Percentile > 100ms ({value.Histogram.Percentile98}{SandboxMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})", result: value.Histogram.Percentile98 < 200); },
                        //     failing: value => { return (message: $"FAILED. 98th Percentile > 200ms ({value.Histogram.Percentile98}{SandboxMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})", result: value.Histogram.Percentile98 > 200); });
                    });
        }
    }
}