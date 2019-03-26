// <copyright file="HealthOutputFormattingBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Text;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Formatters.Json;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Facts.Builders
{
    public class HealthOutputFormattingBuilderTests
    {
        [Fact]
        public void At_least_one_formatter_is_required()
        {
            // Arrange
            var builder = new HealthBuilder();

            // Act
            var health = builder.Build();

            // Assert
            health.OutputHealthFormatters.Count.Should().Be(1);
            health.DefaultOutputHealthFormatter.Should().NotBeNull();
        }

        [Fact]
        public void Can_keep_existing_formatter_if_registered_more_than_once()
        {
            // Arrange
            var formatter = new HealthStatusTextOutputFormatter(new HealthTextOptions { Encoding = Encoding.BigEndianUnicode });
            var builder = new HealthBuilder()
                .OutputHealth.Using(formatter)
                .OutputHealth.Using<HealthStatusTextOutputFormatter>(false);

            // Act
            var health = builder.Build();

            // Assert
            health.OutputHealthFormatters.Count.Should().Be(1);
            health.OutputHealthFormatters.First().Should().BeSameAs(formatter);
        }

        [Fact]
        public void Cannot_set_null_health_output_formatter()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var unused = new HealthBuilder().OutputHealth.Using(null);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Default_health_output_formatter_should_be_first_formatter_selected_via_instantiation()
        {
            // Arrange
            var builder = new HealthBuilder()
                .OutputHealth.Using(new HealthStatusTextOutputFormatter())
                .OutputHealth.Using(new HealthStatusJsonOutputFormatter());

            // Act
            var health = builder.Build();

            // Assert
            health.DefaultOutputHealthFormatter.Should().BeOfType<HealthStatusTextOutputFormatter>();
        }

        [Fact]
        public void Default_health_output_formatter_should_be_first_formatter_selected_via_type()
        {
            // Arrange
            var builder = new HealthBuilder()
                .OutputHealth.Using<HealthStatusJsonOutputFormatter>()
                .OutputHealth.Using<HealthStatusTextOutputFormatter>();

            // Act
            var health = builder.Build();

            // Assert
            health.DefaultOutputHealthFormatter.Should().BeOfType<HealthStatusJsonOutputFormatter>();
        }

        [Fact]
        public void Should_not_replace_formatter_if_types_are_different_and_asked_not_to_replace()
        {
            // Arrange
            var formatter = new HealthStatusTextOutputFormatter(new HealthTextOptions { Encoding = Encoding.BigEndianUnicode });
            var builder = new HealthBuilder()
                .OutputHealth.Using(formatter)
                .OutputHealth.Using<HealthStatusJsonOutputFormatter>(false);

            // Act
            var health = builder.Build();

            // Assert
            health.OutputHealthFormatters.Count.Should().Be(2);
        }

        [Fact]
        public void Should_only_add_a_single_formatter_of_the_same_type()
        {
            // Arrange
            var builder = new HealthBuilder()
                .OutputHealth.Using<HealthStatusTextOutputFormatter>()
                .OutputHealth.Using<HealthStatusTextOutputFormatter>();

            // Act
            var health = builder.Build();

            // Assert
            health.OutputHealthFormatters.Count.Should().Be(1);
            health.DefaultOutputHealthFormatter.Should().NotBeNull();
        }

        [Fact]
        public void Should_set_health_output_formatters_when_selected_via_instantiation()
        {
            // Arrange
            var builder = new HealthBuilder()
                .OutputHealth.Using(new HealthStatusTextOutputFormatter())
                .OutputHealth.Using(new HealthStatusJsonOutputFormatter());

            // Act
            var health = builder.Build();

            // Assert
            health.OutputHealthFormatters.Count.Should().Be(2);
        }

        [Fact]
        public void Should_set_health_output_formatters_when_selected_via_type()
        {
            // Arrange
            var builder = new HealthBuilder()
                .OutputHealth.Using<HealthStatusTextOutputFormatter>()
                .OutputHealth.Using<HealthStatusJsonOutputFormatter>();

            // Act
            var health = builder.Build();

            // Assert
            health.OutputHealthFormatters.Count.Should().Be(2);
        }
    }
}