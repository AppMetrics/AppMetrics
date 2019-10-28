// <copyright file="JsonConfigurationTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Sdk;

namespace Metrics.Extensions.Configuration.Facts
{
    public class JsonConfigurationTests
    {
        [Fact(Skip = "Skipping for now to get Linux build working")]
        public void Can_bind_metrics_options_from_configuration()
        {
            // Arrange
            var builder = new MetricsBuilder();
            var configuration = new ConfigurationBuilder().AddJsonFile("JsonFIles/MetricsOptions.json").Build();

            // Act
            builder.Configuration.ReadFrom(configuration);
            var metrics = builder.Build();

            // Assert
            metrics.Options.DefaultContextLabel.Should().Be("Testing");
            metrics.Options.Enabled.Should().BeFalse();
            metrics.Options.GlobalTags.Count.Should().Be(2);
            metrics.Options.GlobalTags.First(t => t.Key == "env" && t.Value == "stage").Should().NotBeNull();
            metrics.Options.GlobalTags.First(t => t.Key == "tagkey" && t.Value == "tagValue").Should().NotBeNull();
        }

        [Fact(Skip = "Skipping for now to get Linux build working")]
        public void Should_merge_global_tags_when_key_values_provided_that_match_an_existing_tag()
        {
            // Arrange
            var builder = new MetricsBuilder();
            var configuration = new ConfigurationBuilder().AddJsonFile("JsonFIles/MetricsOptions.json").Build();
            var options = new MetricsOptions();
            options.GlobalTags.Add("tag1", "value1");

            // Act
            builder.Configuration.Configure(options);
            builder.Configuration.ReadFrom(configuration);
            var metrics = builder.Build();

            // Assert
            metrics.Options.GlobalTags.Count.Should().Be(3);

            var tag1 = metrics.Options.GlobalTags.FirstOrDefault(t => t.Key == "tag1");
            tag1.Should().NotBeNull();
            tag1.Value.Should().Be("value1");

            var tag2 = metrics.Options.GlobalTags.FirstOrDefault(t => t.Key == "env");
            tag2.Should().NotBeNull();
            tag2.Value.Should().Be("stage");

            var tag3 = metrics.Options.GlobalTags.FirstOrDefault(t => t.Key == "tagkey");
            tag3.Should().NotBeNull();
            tag3.Value.Should().Be("tagValue");
        }
    }
}
