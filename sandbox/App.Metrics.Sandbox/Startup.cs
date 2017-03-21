using System;
using App.Metrics.Configuration;
using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Filtering;
using App.Metrics.Reporting.Interfaces;
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
        private static bool HaveAppRunSampleRequests = true;
        private static bool RunSamplesWithClientId = true;
        private static readonly string InfluxDbDatabase = "AppMetricsSandbox";
        private static readonly Uri InfluxDbUri = new Uri("http://127.0.0.1:8086");

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).
                                                     AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
                                                     AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true).
                                                     AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime)
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

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            // loggerFactory.AddDebug();

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

            var reportFilter = new DefaultMetricsFilter();
            reportFilter.WithHealthChecks(false);

            services.AddMetrics(Configuration.GetSection("AppMetrics"),
                         options =>
                         {
                             options.WithGlobalTags(
                                 (globalTags, info) =>
                                 {
                                     globalTags.Add("app", info.EntryAssemblyName);
                                 });
                         }).
                     AddJsonSerialization().
                     AddReporting(
                         factory =>
                         {
                             factory.AddInfluxDb(
                                 new InfluxDBReporterSettings
                                 {
                                     HttpPolicy = new HttpPolicy
                                                  {
                                                      FailuresBeforeBackoff = 3,
                                                      BackoffPeriod = TimeSpan.FromSeconds(30),
                                                      Timeout = TimeSpan.FromSeconds(10)
                                                  },
                                     InfluxDbSettings = new InfluxDBSettings(InfluxDbDatabase, InfluxDbUri),
                                     ReportInterval = TimeSpan.FromSeconds(5)
                                 }, reportFilter);
                         }).
                     AddHealthChecks(
                         factory =>
                         {                             
                             factory.RegisterPingHealthCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                             factory.RegisterHttpGetHealthCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));
                         }).
                     AddMetricsMiddleware(Configuration.GetSection("AspNetMetrics"));
        }
    }
}