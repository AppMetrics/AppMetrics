// <copyright file="MetricsJsonOutputFormattingBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Formatters.Json.Facts.Builder
{
    public class MetricsJsonOutputFormattingBuilderTests
    {
        [Fact]
        public void Can_set_metrics_json_output_formatting()
        {
            // Arrange
            var builder = new MetricsBuilder().OutputMetrics.AsJson();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.DefaultOutputMetricsFormatter.Should().BeOfType<MetricsJsonOutputFormatter>();
            metrics.OutputMetricsFormatters.Count.Should().Be(1);
            metrics.OutputMetricsFormatters.First().Should().BeOfType<MetricsJsonOutputFormatter>();
        }
    }
}