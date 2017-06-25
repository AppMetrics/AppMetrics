// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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
            app.UseMetricsReporting(lifetime);

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTestStuff();

            services.AddMvc(options => options.AddMetricsResourceFilter());

            services.AddMetrics().
                     AddSandboxHealthChecks().
                     AddSandboxReporting().
                     AddMetricsMiddleware(
                         optionsBuilder =>
                         {
                             optionsBuilder.AddJsonMetricsSerialization().
                                            AddAsciiHealthSerialization().
                                            AddAsciiMetricsTextSerialization().
                                            AddAsciiEnvironmentInfoSerialization();
                         });
        }

        // public void ConfigureServices(IServiceCollection services)
        // {
        //     services.AddTestStuff();
        //     services.AddMvc(options => options.AddMetricsResourceFilter());
        //     services.AddMetrics(Configuration.GetSection("AppMetrics")).
        //              AddSandboxHealthChecks().
        //              AddSandboxReporting().
        //              AddMetricsMiddleware(
        //                  Configuration.GetSection("AspNetMetrics"),
        //                  optionsBuilder =>
        //                  {
        //                      optionsBuilder.AddJsonMetricsSerialization().
        //                                     AddAsciiHealthSerialization().
        //                                     AddAsciiMetricsTextSerialization().
        //                                     AddAsciiEnvironmentInfoSerialization();
        //                  });
        // }
    }
}