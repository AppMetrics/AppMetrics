// <copyright file="MetricsTextEnvOutputFormattingBuilderTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Formatters.Ascii.Facts.Builder
{
    public class MetricsTextEnvOutputFormattingBuilderTests
    {
        [Fact]
        public void Can_set_env_plain_text_output_formatting()
        {
            // Arrange
            var builder = new MetricsBuilder().OutputEnvInfo.AsPlainText();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.DefaultOutputEnvFormatter.Should().BeOfType<EnvInfoTextOutputFormatter>();
            metrics.OutputEnvFormatters.Count.Should().Be(1);
            metrics.OutputEnvFormatters.First().Should().BeOfType<EnvInfoTextOutputFormatter>();
        }
    }
}