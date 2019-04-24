// <copyright file="MetricsFormatterReadonlyCollectionExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Formatters
{
    public class MetricsFormatterReadonlyCollectionExtensionsTests
    {
        [Fact]
        public void Can_get_formatter_by_type()
        {
            // Arrange
            var formatters = new MetricsFormatterCollection();
            formatters.TryAdd(new MetricsJsonOutputFormatter());
            formatters.TryAdd(new MetricsTextOutputFormatter());
            var readonlyFormatters = new ReadOnlyCollection<IMetricsOutputFormatter>(formatters);

            // Act
            var result = readonlyFormatters.GetType<MetricsTextOutputFormatter>();

            // Assert
            result.Should().BeOfType(typeof(MetricsTextOutputFormatter));
        }
    }
}