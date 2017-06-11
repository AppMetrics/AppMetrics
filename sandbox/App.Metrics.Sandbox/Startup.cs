// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using App.Metrics.Sandbox.Extensions;
using App.Metrics.Sandbox.JustForTesting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Sandbox
{
    public class Startup
    {
        private static readonly bool HaveAppRunSampleRequests = true;
        private static readonly bool RunSamplesWithClientId = true;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).
                                                     AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
                                                     AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true).
                                                     AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder().UseContentRoot(Directory.GetCurrentDirectory()).
                                            ConfigureLogging(
                                                factory => { factory.AddConsole(); }).UseIISIntegration().UseKestrel().
                                            UseStartup<Startup>().Build();

            host.Run();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            if (RunSamplesWithClientId && HaveAppRunSampleRequests)
            {
                app.Use(
                    (context, func) =>
                    {
                        RandomClientIdForTesting.SetTheFakeClaimsPrincipal(context);
                        return func();
                    });
            }

            app.UseMetrics();
            app.UseMetricsReporting(lifetime);

            app.UseMvc();

            if (HaveAppRunSampleRequests)
            {
                SampleRequests.Run(lifetime.ApplicationStopping);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTestStuff();
            services.AddLogging().AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            services.AddMetrics(Configuration.GetSection("AppMetrics")).
                     AddSandboxHealthChecks().
                     AddSandboxReporting().
                     AddHealthChecks().
                     AddMetricsMiddleware(
                         Configuration.GetSection("AspNetMetrics"),
                         optionsBuilder =>
                         {
                             optionsBuilder.AddJsonMetricsSerialization().
                                     AddAsciiHealthSerialization().
                                     AddAsciiMetricsTextSerialization().
                                     AddAsciiEnvironmentInfoSerialization();
                         });
        }
    }
}