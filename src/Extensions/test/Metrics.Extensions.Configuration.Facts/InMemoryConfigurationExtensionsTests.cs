// <copyright file="InMemoryConfigurationExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Metrics.Extensions.Configuration.Facts
{
    public class InMemoryConfigurationExtensionsTests
    {
        [Fact]
        public void Can_bind_metrics_options_from_configuration()
        {
            // Arrange
            var builder = new MetricsBuilder();
            var keyValuePairs = new Dictionary<string, string>
                                {
                                    { "MetricsOptions:DefaultContextLabel", "Testing" },
                                    { "MetricsOptions:GlobalTags:tag1", "value1" },
                                    { "MetricsOptions:GlobalTags:tag2", "value2" },
                                    { "MetricsOptions:Enabled", "false" }
                                };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(keyValuePairs).Build();

            // Act
            builder.Configuration.ReadFrom(configuration);
            var metrics = builder.Build();

            // Assert
            metrics.Options.DefaultContextLabel.Should().Be("Testing");
            metrics.Options.Enabled.Should().BeFalse();
            metrics.Options.GlobalTags.Count.Should().Be(2);
        }

        [Fact]
        public void Should_merge_global_tags_when_key_values_provided_that_match_an_existing_tag()
        {
            // Arrange
            var builder = new MetricsBuilder();
            var keyValuePairs = new Dictionary<string, string>
                                {
                                    { "MetricsOptions:GlobalTags:tag1", "replaced" },
                                    { "MetricsOptions:GlobalTags:tag2", "added" }
                                };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(keyValuePairs).Build();
            var options = new MetricsOptions();
            options.GlobalTags.Add("tag1", "value1");

            // Act
            builder.Configuration.Configure(o => o.GlobalTags.Add("tag1", "value1"));
            builder.Configuration.ReadFrom(configuration);
            var metrics = builder.Build();

            // Assert
            metrics.Options.GlobalTags.Count.Should().Be(2);

            var tag1 = metrics.Options.GlobalTags.FirstOrDefault(t => t.Key == "tag1");
            tag1.Should().NotBeNull();
            tag1.Value.Should().Be("replaced");

            var tag2 = metrics.Options.GlobalTags.FirstOrDefault(t => t.Key == "tag2");
            tag2.Should().NotBeNull();
            tag2.Value.Should().Be("added");
        }
    }
}
