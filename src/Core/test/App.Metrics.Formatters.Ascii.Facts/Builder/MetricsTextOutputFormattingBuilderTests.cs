// <copyright file="MetricsTextOutputFormattingBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Formatters.Ascii.Facts.Builder
{
    public class MetricsTextOutputFormattingBuilderTests
    {
        [Fact]
        public void Can_set_metrics_plain_text_output_formatting()
        {
            // Arrange
            var builder = new MetricsBuilder().OutputMetrics.AsPlainText();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.DefaultOutputMetricsFormatter.Should().BeOfType<MetricsTextOutputFormatter>();
            metrics.OutputMetricsFormatters.Count.Should().Be(1);
            metrics.OutputMetricsFormatters.First().Should().BeOfType<MetricsTextOutputFormatter>();
        }
    }
}