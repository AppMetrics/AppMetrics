// <copyright file="SandboxMetricsHostExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Builder;
using App.Metrics.Core.Filtering;
using App.Metrics.Filters;
using App.Metrics.Reporting;
using App.Metrics.Reporting.InfluxDB;
using App.Metrics.Reporting.InfluxDB.Client;

namespace App.Metrics.Sandbox.Extensions
{
#pragma warning disable SA1111, SA1009, SA1008
    public static class SandboxMetricsHostExtensions
    {
        // private static readonly string ElasticSearchIndex = "appmetricssandbox";
        // private static readonly Uri ElasticSearchUri = new Uri("http://127.0.0.1:9200");
        // private static readonly Uri GraphiteUri = new Uri("net.tcp://127.0.0.1:32776");
        private static readonly string InfluxDbDatabase = "AppMetricsSandbox";

        private static readonly Uri InfluxDbUri = new Uri("http://127.0.0.1:8086");

        private static readonly List<ReportType> ReportTypes =
            new List<ReportType> { ReportType.InfluxDB, ReportType.ElasticSearch, /*ReportType.Graphite*/ };

        public static IMetricsHostBuilder AddSandboxHealthChecks(this IMetricsHostBuilder host)
        {
            host.AddHealthChecks(
                factory =>
                {
                    factory.RegisterOveralWebRequestsApdexCheck();
                    factory.RegisterPingHealthCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                    factory.RegisterHttpGetHealthCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));
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
            );

            return host;
        }

        public static IMetricsHostBuilder AddSandboxReporting(this IMetricsHostBuilder host)
        {
            var reportFilter = new DefaultMetricsFilter();

            host.AddReporting(
                factory =>
                {
                    if (ReportTypes.Any(r => r == ReportType.InfluxDB))
                    {
                        AddInfluxReporting(factory, reportFilter);
                    }

                    if (ReportTypes.Any(r => r == ReportType.ElasticSearch))
                    {
                        AddElasticSearchReporting(factory, reportFilter);
                    }

                    if (ReportTypes.Any(r => r == ReportType.Graphite))
                    {
                        AddGraphiteReporting(factory, reportFilter);
                    }
                });

            return host;
        }

        private static void AddElasticSearchReporting(IReportFactory factory, DefaultMetricsFilter reportFilter)
        {
            // factory.AddElasticSearch(
            //     new ElasticSearchReporterSettings
            //     {
            //         HttpPolicy = new HttpPolicy
            //                      {
            //                          FailuresBeforeBackoff = 3,
            //                          BackoffPeriod = TimeSpan.FromSeconds(30),
            //                          Timeout = TimeSpan.FromSeconds(10)
            //                      },
            //         ElasticSearchSettings = new ElasticSearchSettings(ElasticSearchUri, ElasticSearchIndex),
            //         ReportInterval = TimeSpan.FromSeconds(5)
            //     },
            //     reportFilter);
        }

        private static void AddGraphiteReporting(IReportFactory factory, DefaultMetricsFilter reportFilter)
        {
            // factory.AddGraphite(
            //     new GraphiteReporterSettings
            //     {
            //         HttpPolicy = new Extensions.Reporting.Graphite.HttpPolicy
            //                      {
            //                          FailuresBeforeBackoff = 3,
            //                          BackoffPeriod = TimeSpan.FromSeconds(30),
            //                          Timeout = TimeSpan.FromSeconds(3)
            //                      },
            //         GraphiteSettings = new GraphiteSettings(GraphiteUri),
            //         ReportInterval = TimeSpan.FromSeconds(5)
            //     });
        }

        private static void AddInfluxReporting(IReportFactory factory, IFilterMetrics reportFilter)
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
        }
    }
#pragma warning restore SA1111, SA1009, SA1008
}