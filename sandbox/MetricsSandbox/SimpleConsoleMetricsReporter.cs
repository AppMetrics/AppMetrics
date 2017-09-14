// <copyright file="SimpleConsoleMetricsReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Reporting;
using static System.Console;

namespace MetricsSandbox
{
    public class SimpleConsoleMetricsReporter : IReportMetrics
    {
        private readonly IMetricsOutputFormatter _defaultMetricsOutputFormatter = new MetricsTextOutputFormatter();

        /// <inheritdoc />
        public IFilterMetrics Filter { get; set; }

        /// <inheritdoc />
        public TimeSpan FlushInterval { get; set; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            WriteLine("Metrics Report");
            WriteLine("-------------------------------------------");

            using (var stream = new MemoryStream())
            {
                await _defaultMetricsOutputFormatter.WriteAsync(stream, metricsData, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                WriteLine(output);
            }

            return true;
        }
    }
}