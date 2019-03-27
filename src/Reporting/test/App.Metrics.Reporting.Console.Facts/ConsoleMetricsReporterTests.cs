// <copyright file="ConsoleMetricsReporterTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filtering;
using App.Metrics.Reporting.FactsCommon;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.Console.Facts
{
    public class ConsoleMetricsReporterTests
    {
        [Fact]
        public async Task Can_flush_metrics_to_console_with_defaults()
        {
            // Arrange
            var reporter = new ConsoleMetricsReporter();
            var snapshot = new MetricsDataValueSource(DateTime.Now, Enumerable.Empty<MetricsContextValueSource>());

            // Act
            var result = await reporter.FlushAsync(snapshot, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Can_flush_metrics_to_console_with_options()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var formatter = new TestMetricsFormatter();
            var interval = TimeSpan.FromDays(1);
            var options = new MetricsReportingConsoleOptions
                          {
                              Filter = filter,
                              MetricsOutputFormatter = formatter,
                              FlushInterval = interval
                          };
            var reporter = new ConsoleMetricsReporter(options);
            var snapshot = new MetricsDataValueSource(DateTime.Now, Enumerable.Empty<MetricsContextValueSource>());

            // Act
            var result = await reporter.FlushAsync(snapshot, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }
    }
}
