// <copyright file="SimpleConsoleMetricsReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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
        public SimpleConsoleMetricsReporter()
        {
            Formatter = new MetricsTextOutputFormatter();
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; set; }

        /// <inheritdoc />
        public TimeSpan FlushInterval { get; set; }

        /// <inheritdoc />
        public IMetricsOutputFormatter Formatter { get; set; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            WriteLine("Metrics Report");
            WriteLine("-------------------------------------------");

            using (var stream = new MemoryStream())
            {
                await Formatter.WriteAsync(stream, metricsData, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                WriteLine(output);
            }

            return true;
        }
    }
}