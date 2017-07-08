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

namespace App.Metrics.Sandbox.Extensions
{
#pragma warning disable SA1111, SA1009, SA1008
    public static class SandboxMetricsHostExtensions
    {
        // private static readonly string ElasticSearchIndex = "appmetricssandbox";
        // private static readonly Uri ElasticSearchUri = new Uri("http://127.0.0.1:9200");
        // private static readonly Uri GraphiteUri = new Uri("net.tcp://127.0.0.1:32776");
        // private static readonly string InfluxDbDatabase = "appmetricssandbox";

        // private static readonly Uri InfluxDbUri = new Uri("http://127.0.0.1:8086");

        private static readonly List<ReportType> ReportTypes =
            new List<ReportType> { ReportType.InfluxDB, ReportType.ElasticSearch, /*ReportType.Graphite*/ };

        public static IAppMetricsBuilder AddSandboxReporting(this IAppMetricsBuilder builder)
        {
            var reportFilter = new DefaultMetricsFilter();

            builder.AddReporting(
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

            return builder;
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
            // factory.AddInfluxDb(
            //     new InfluxDBReporterSettings
            //     {
            //         HttpPolicy = new HttpPolicy
            //         {
            //             FailuresBeforeBackoff = 3,
            //             BackoffPeriod = TimeSpan.FromSeconds(30),
            //             Timeout = TimeSpan.FromSeconds(10)
            //         },
            //         InfluxDbSettings = new Reporting.InfluxDB.Client.InfluxDBSettings(InfluxDbDatabase, InfluxDbUri),
            //         ReportInterval = TimeSpan.FromSeconds(5)
            //     },
            //     reportFilter);
        }
    }
#pragma warning restore SA1111, SA1009, SA1008
}