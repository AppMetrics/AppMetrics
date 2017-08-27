// <copyright file="EnvOutputFormattingBuilderTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builder
{
    public class EnvOutputFormattingBuilderTests
    {
        [Fact]
        public void Cannot_set_null_env_output_formatter()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var builder = new MetricsBuilder().OutputEnvInfo.Using(null);
            };

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Default_env_output_formatter_should_be_first_formatter_selected_via_instantiation()
        {
            // Arrange
            var builder = new MetricsBuilder()
                .OutputEnvInfo.Using(new EnvInfoTextOutputFormatter())
                .OutputEnvInfo.Using(new EnvInfoJsonOutputFormatter());

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.DefaultOutputEnvFormatter.Should().BeOfType<EnvInfoTextOutputFormatter>();
        }

        [Fact]
        public void Default_env_output_formatter_should_be_first_formatter_selected_via_type()
        {
            // Arrange
            var builder = new MetricsBuilder()
                .OutputEnvInfo.Using<EnvInfoTextOutputFormatter>()
                .OutputEnvInfo.Using<EnvInfoJsonOutputFormatter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.DefaultOutputEnvFormatter.Should().BeOfType<EnvInfoTextOutputFormatter>();
        }

        [Fact]
        public void Should_set_env_output_formatters_when_selected_via_instantiation()
        {
            // Arrange
            var builder = new MetricsBuilder()
                .OutputEnvInfo.Using(new EnvInfoTextOutputFormatter())
                .OutputEnvInfo.Using(new EnvInfoJsonOutputFormatter());

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.OutputEnvFormatters.Count.Should().Be(2);
        }

        [Fact]
        public void Should_set_env_output_formatters_when_selected_via_type()
        {
            // Arrange
            var builder = new MetricsBuilder()
                .OutputEnvInfo.Using<EnvInfoTextOutputFormatter>()
                .OutputEnvInfo.Using<EnvInfoJsonOutputFormatter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.OutputEnvFormatters.Count.Should().Be(2);
        }
    }
}