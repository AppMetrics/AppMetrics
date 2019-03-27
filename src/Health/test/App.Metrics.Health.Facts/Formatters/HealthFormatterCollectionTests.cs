// <copyright file="HealthFormatterCollectionTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Health.Formatters;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Formatters.Json;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Facts.Formatters
{
    public class HealthFormatterCollectionTests
    {
        [Fact]
        public void Can_get_by_mediatype()
        {
            // Arrange
            var mediaType = new HealthMediaTypeValue("application", "vnd.appmetrics.health", "v1", "json");
            var formatters =
                new HealthFormatterCollection
                {
                    new HealthStatusTextOutputFormatter(),
                    new HealthStatusJsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType(mediaType);

            // Assert
            formatter.Should().NotBeNull();
            formatter.Should().BeOfType<HealthStatusJsonOutputFormatter>();
        }

        [Fact]
        public void Can_get_type()
        {
            // Arrange
            var formatters =
                new HealthFormatterCollection
                {
                    new HealthStatusTextOutputFormatter(),
                    new HealthStatusJsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType<HealthStatusTextOutputFormatter>();

            // Assert
            formatter.Should().NotBeNull();
            formatter.Should().BeOfType<HealthStatusTextOutputFormatter>();
        }

        [Fact]
        public void Can_get_type_passing_in_type()
        {
            // Arrange
            var formatters =
                new HealthFormatterCollection
                {
                    new HealthStatusTextOutputFormatter(),
                    new HealthStatusJsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType(typeof(HealthStatusTextOutputFormatter));

            // Assert
            formatter.Should().NotBeNull();
            formatter.Should().BeOfType<HealthStatusTextOutputFormatter>();
        }

        [Fact]
        public void Can_remove_by_mediatype()
        {
            // Arrange
            var mediaType = new HealthMediaTypeValue("text", "vnd.appmetrics.health", "v1", "plain");
            var formatters = new HealthFormatterCollection(
                new List<IHealthOutputFormatter> { new HealthStatusTextOutputFormatter() });

            // Act
            formatters.RemoveType(mediaType);

            // Assert
            formatters.Count.Should().Be(0);
        }

        [Fact]
        public void Can_remove_type()
        {
            // Arrange
            var formatters = new HealthFormatterCollection { new HealthStatusTextOutputFormatter() };

            // Act
            formatters.RemoveType<HealthStatusTextOutputFormatter>();

            // Assert
            formatters.Count.Should().Be(0);
        }

        [Fact]
        public void Can_remove_type_passing_in_type()
        {
            // Arrange
            var formatters = new HealthFormatterCollection { new HealthStatusTextOutputFormatter() };

            // Act
            formatters.RemoveType(typeof(HealthStatusTextOutputFormatter));

            // Assert
            formatters.Count.Should().Be(0);
        }

        [Fact]
        public void Returns_default_when_attempting_to_get_type_not_added()
        {
            // Arrange
            var formatters =
                new HealthFormatterCollection
                {
                    new HealthStatusJsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType<HealthStatusTextOutputFormatter>();

            // Assert
            formatter.Should().BeNull();
        }

        [Fact]
        public void Returns_default_when_attempting_to_get_type_with_mediatype_not_added()
        {
            // Arrange
            var mediaType = new HealthMediaTypeValue("text", "vnd.appmetrics.health", "v1", "plain");
            var formatters =
                new HealthFormatterCollection
                {
                    new HealthStatusJsonOutputFormatter()
                };

            // Act
            var formatter = formatters.GetType(mediaType);

            // Assert
            formatter.Should().BeNull();
        }
    }
}