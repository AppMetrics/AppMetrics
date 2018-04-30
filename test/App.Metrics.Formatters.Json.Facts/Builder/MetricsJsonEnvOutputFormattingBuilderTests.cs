// <copyright file="MetricsJsonEnvOutputFormattingBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Formatters.Json.Facts.Builder
{
    public class MetricsJsonEnvOutputFormattingBuilderTests
    {
        [Fact]
        public void Can_set_env_plain_text_output_formatting()
        {
            // Arrange
            var builder = new MetricsBuilder().OutputEnvInfo.AsJson();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.DefaultOutputEnvFormatter.Should().BeOfType<EnvInfoJsonOutputFormatter>();
            metrics.OutputEnvFormatters.Count.Should().Be(1);
            metrics.OutputEnvFormatters.First().Should().BeOfType<EnvInfoJsonOutputFormatter>();
        }
    }
}