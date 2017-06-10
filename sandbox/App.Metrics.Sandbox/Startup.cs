// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics.Extensions.Reporting.ElasticSearch;
using App.Metrics.Extensions.Reporting.ElasticSearch.Client;
using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Filtering;
using App.Metrics.Reporting.Interfaces;
using App.Metrics.Sandbox.JustForTesting;
using App.Metrics.Tagging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HttpPolicy = App.Metrics.Extensions.Reporting.InfluxDB.HttpPolicy;

namespace App.Metrics.Sandbox
{
    public enum ReportType
    {
#pragma warning disable SA1602 // Enumeration items must be documented
        InfluxDB,
        ElasticSearch,
        Graphite
#pragma warning restore SA1602 // Enumeration items must be documented
    }

    public class Startup
    {
        private static readonly string ElasticSearchIndex = "appmetricssandbox";
        private static readonly Uri ElasticSearchUri = new Uri("http://127.0.0.1:9200");
        private static readonly Uri GraphiteUri = new Uri("net.tcp://127.0.0.1:32776");
        private static readonly bool HaveAppRunSampleRequests = true;
        private static readonly string InfluxDbDatabase = "AppMetricsSandbox";
        private static readonly Uri InfluxDbUri = new Uri("http://127.0.0.1:8086");

        private static readonly List<ReportType> ReportTypes =
            new List<ReportType> { ReportType.InfluxDB, ReportType.ElasticSearch, /*ReportType.Graphite*/ };

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
                                                factory =>
                                                {
                                                    factory.AddConsole();
                                                }).UseIISIntegration().UseKestrel().
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

            var reportFilter = new DefaultMetricsFilter();

            services.AddMetrics(Configuration.GetSection("AppMetrics")).
                     // AddJsonMetricsSerialization().
                     // AddElasticsearchMetricsSerialization(ElasticSearchIndex).
                     AddJsonMetricsSerialization().
                     AddAsciiHealthSerialization().
                     AddAsciiMetricsTextSerialization().
                     // AddPrometheusPlainTextSerialization().
                     // AddInfluxDBLineProtocolMetricsTextSerialization().
                     AddAsciiEnvironmentInfoSerialization().
                     // AddJsonEnvironmentInfoSerialization().
                     AddReporting(
                         factory =>
                         {
                             if (ReportTypes.Any(r => r == ReportType.InfluxDB))
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
                                     },
                                     reportFilter);

                                 if (ReportTypes.Any(r => r == ReportType.ElasticSearch))
                                 {
                                     factory.AddElasticSearch(
                                         new ElasticSearchReporterSettings
                                         {
                                             HttpPolicy = new Extensions.Reporting.ElasticSearch.HttpPolicy
                                                          {
                                                              FailuresBeforeBackoff = 3,
                                                              BackoffPeriod = TimeSpan.FromSeconds(30),
                                                              Timeout = TimeSpan.FromSeconds(10)
                                                          },
                                             ElasticSearchSettings = new ElasticSearchSettings(ElasticSearchUri, ElasticSearchIndex),
                                             ReportInterval = TimeSpan.FromSeconds(5)
                                         },
                                         reportFilter);
                                 }

                                 // if (ReportTypes.Any(r => r == ReportType.Graphite))
                                 // {
                                 //     factory.AddGraphite(
                                 //         new GraphiteReporterSettings
                                 //         {
                                 //             HttpPolicy = new Extensions.Reporting.Graphite.HttpPolicy
                                 //             {
                                 //                 FailuresBeforeBackoff = 3,
                                 //                 BackoffPeriod = TimeSpan.FromSeconds(30),
                                 //                 Timeout = TimeSpan.FromSeconds(3)
                                 //             },
                                 //             GraphiteSettings = new GraphiteSettings(GraphiteUri),
                                 //             ReportInterval = TimeSpan.FromSeconds(5)
                                 //         });
                                 // }
                             }
                         }).
                     AddHealthChecks(
                         factory =>
                         {
                             factory.RegisterPingHealthCheck("google ping", "google.com", TimeSpan.FromSeconds(10));

                             factory.RegisterHttpGetHealthCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly

                             factory.RegisterMetricCheck(
                                 name: "Database Call Duration",
                                 options: SandboxMetricsRegistry.DatabaseTimer,
                                 tags: new MetricTags("client_id", "client-9"),
                                 passing: value => (message:
                                     $"OK. 98th Percentile < 100ms ({value.Histogram.Percentile98}{SandboxMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})"
                                     , result: value.Histogram.Percentile98 < 100),
                                 warning: value => (message:
                                     $"WARNING. 98th Percentile > 100ms ({value.Histogram.Percentile98}{SandboxMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})"
                                     , result: value.Histogram.Percentile98 < 200),
                                 failing: value => (message:
                                     $"FAILED. 98th Percentile > 200ms ({value.Histogram.Percentile98}{SandboxMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})"
                                     , result: value.Histogram.Percentile98 > 200));
                         }
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
#pragma warning disable SA1111 // Closing parenthesis must be on line of last parameter
#pragma warning disable SA1009 // Closing parenthesis must be spaced correctly
                     ).
#pragma warning restore SA1009 // Closing parenthesis must be spaced correctly
#pragma warning restore SA1111 // Closing parenthesis must be on line of last parameter
                     AddMetricsMiddleware(Configuration.GetSection("AspNetMetrics"));
        }
    }
}