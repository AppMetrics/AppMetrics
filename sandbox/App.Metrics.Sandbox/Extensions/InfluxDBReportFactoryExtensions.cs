// <copyright file="InfluxDBReportFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using App.Metrics.Reporting.InfluxDB;

// ReSharper disable CheckNamespace
namespace App.Metrics.Reporting
    // ReSharper restore CheckNamespace
{
    public static class InfluxDBReportFactoryExtensions
    {
        private static readonly string InfluxDbDatabase = "appmetricssandbox";
        private static readonly Uri InfluxDbUri = new Uri("http://127.0.0.1:8086");

        public static void AddInfluxReporting(this IReportFactory factory)
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
                    InfluxDbSettings = new Reporting.InfluxDB.Client.InfluxDBSettings(InfluxDbDatabase, InfluxDbUri),
                    ReportInterval = TimeSpan.FromSeconds(5)
                });
        }
    }
}
