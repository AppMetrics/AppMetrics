// <copyright file="GraphiteReportFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics.Reporting
    // ReSharper restore CheckNamespace
{
    public static class GraphiteReportFactoryExtensions
    {
        // private static readonly Uri GraphiteUri = new Uri("net.tcp://127.0.0.1:32776");

        public static void AddGraphiteReporting(this IReportFactory factory)
        {
            // factory.AddGraphite(
            //     new GraphiteReporterSettings
            //     {
            //         HttpPolicy = new Extensions.Reporting.Graphite.HttpPolicy
            //         {
            //             FailuresBeforeBackoff = 3,
            //             BackoffPeriod = TimeSpan.FromSeconds(30),
            //             Timeout = TimeSpan.FromSeconds(3)
            //         },
            //         GraphiteSettings = new GraphiteSettings(GraphiteUri),
            //         ReportInterval = TimeSpan.FromSeconds(5)
            //     });
        }
    }
}
