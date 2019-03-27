// <copyright file="MetricsFormatterCollectionTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Formatters
{
    public class MetricsFormatterCollectionTests
    {
        [Fact]
        public void Can_get_formatter_by_media_type()
        {
            // Arrange
            var formatters = new MetricsFormatterCollection();
            formatters.TryAdd(new MetricsJsonOutputFormatter());
            formatters.TryAdd(new MetricsTextOutputFormatter());
            var mediaType = new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics", "v1", "plain");

            // Act
            var result = formatters.GetType(mediaType);

            // Assert
            result.Should().BeOfType(typeof(MetricsTextOutputFormatter));
        }

        [Fact]
        public void Can_get_formatter_by_type()
        {
            // Arrange
            var formatters = new MetricsFormatterCollection();
            formatters.TryAdd(new MetricsJsonOutputFormatter());
            formatters.TryAdd(new MetricsTextOutputFormatter());

            // Act
            var result = formatters.GetType<MetricsTextOutputFormatter>();

            // Assert
            result.Should().BeOfType(typeof(MetricsTextOutputFormatter));
        }

        [Fact]
        public void Can_remove_formatter_by_media_type()
        {
            // Arrange
            var formatters = new MetricsFormatterCollection();
            formatters.TryAdd(new MetricsJsonOutputFormatter());
            formatters.TryAdd(new MetricsTextOutputFormatter());
            var mediaType = new MetricsMediaTypeValue("application", "vnd.appmetrics.metrics", "v1", "json");

            // Act
            formatters.RemoveType(mediaType);

            // Assert
            formatters.Single().Should().BeOfType(typeof(MetricsTextOutputFormatter));
        }

        [Fact]
        public void Can_remove_formatter_by_type()
        {
            // Arrange
            var formatters = new MetricsFormatterCollection();
            formatters.TryAdd(new MetricsJsonOutputFormatter());
            formatters.TryAdd(new MetricsTextOutputFormatter());

            // Act
            formatters.RemoveType<MetricsTextOutputFormatter>();

            // Assert
            formatters.Single().Should().BeOfType(typeof(MetricsJsonOutputFormatter));
        }

        [Fact]
        public void Should_remove_formatter_of_existing_type_when_adding()
        {
            // Arrange
            var formatters = new MetricsFormatterCollection();
            var existingFormatter = new MetricsJsonOutputFormatter();
            formatters.TryAdd(existingFormatter);
            formatters.TryAdd(new MetricsTextOutputFormatter());

            // Act
            var newFormatter = new MetricsJsonOutputFormatter();
            formatters.TryAdd(newFormatter);

            // Assert
            formatters.Single(f => f.GetType() == typeof(MetricsJsonOutputFormatter)).Should().NotBeSameAs(existingFormatter);
        }
    }
}