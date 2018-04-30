// <copyright file="MetricsOutputFormattingBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Text;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class MetricsOutputFormattingBuilderTests
    {
        [Fact]
        public void At_least_one_formatter_is_required()
        {
            // Arrange
            var builder = new MetricsBuilder();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.OutputMetricsFormatters.Count.Should().Be(1);
            metrics.DefaultOutputMetricsFormatter.Should().NotBeNull();
        }

        [Fact]
        public void Can_keep_existing_formatter_if_registered_more_than_once()
        {
            // Arrange
            var formatter = new MetricsTextOutputFormatter(new MetricsTextOptions { Encoding = Encoding.BigEndianUnicode });
            var builder = new MetricsBuilder()
                .OutputMetrics.Using(formatter)
                .OutputMetrics.Using<MetricsTextOutputFormatter>(false);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.OutputMetricsFormatters.Count.Should().Be(1);
            metrics.OutputMetricsFormatters.First().Should().BeSameAs(formatter);
        }

        [Fact]
        public void Cannot_set_null_metrics_output_formatter()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var unused = new MetricsBuilder().OutputMetrics.Using(null);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Default_metrics_output_formatter_should_be_first_formatter_selected_via_instantiation()
        {
            // Arrange
            var builder = new MetricsBuilder()
                .OutputMetrics.Using(new MetricsTextOutputFormatter())
                .OutputMetrics.Using(new MetricsJsonOutputFormatter());

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.DefaultOutputMetricsFormatter.Should().BeOfType<MetricsTextOutputFormatter>();
        }

        [Fact]
        public void Default_metrics_output_formatter_should_be_first_formatter_selected_via_type()
        {
            // Arrange
            var builder = new MetricsBuilder()
                .OutputMetrics.Using<MetricsJsonOutputFormatter>()
                .OutputMetrics.Using<MetricsTextOutputFormatter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.DefaultOutputMetricsFormatter.Should().BeOfType<MetricsJsonOutputFormatter>();
        }

        [Fact]
        public void Should_not_replace_formatter_if_types_are_different_and_asked_not_to_replace()
        {
            // Arrange
            var formatter = new MetricsTextOutputFormatter(new MetricsTextOptions { Encoding = Encoding.BigEndianUnicode });
            var builder = new MetricsBuilder()
                .OutputMetrics.Using(formatter)
                .OutputMetrics.Using<MetricsJsonOutputFormatter>(false);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.OutputMetricsFormatters.Count.Should().Be(2);
        }

        [Fact]
        public void Should_only_add_a_single_formatter_of_the_same_type()
        {
            // Arrange
            var builder = new MetricsBuilder()
                .OutputMetrics.Using<MetricsTextOutputFormatter>()
                .OutputMetrics.Using<MetricsTextOutputFormatter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.OutputMetricsFormatters.Count.Should().Be(1);
            metrics.DefaultOutputMetricsFormatter.Should().NotBeNull();
        }

        [Fact]
        public void Should_set_metrics_output_formatters_when_selected_via_instantiation()
        {
            // Arrange
            var builder = new MetricsBuilder()
                .OutputMetrics.Using(new MetricsTextOutputFormatter())
                .OutputMetrics.Using(new MetricsJsonOutputFormatter());

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.OutputMetricsFormatters.Count.Should().Be(2);
        }

        [Fact]
        public void Should_set_metrics_output_formatters_when_selected_via_type()
        {
            // Arrange
            var builder = new MetricsBuilder()
                .OutputMetrics.Using<MetricsTextOutputFormatter>()
                .OutputMetrics.Using<MetricsJsonOutputFormatter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.OutputMetricsFormatters.Count.Should().Be(2);
        }
    }
}