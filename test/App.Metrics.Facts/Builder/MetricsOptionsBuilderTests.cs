// <copyright file="MetricsOptionsBuilderTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Tagging;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builder
{
    public class MetricsOptionsBuilderTests
    {
        [Fact]
        public void Can_override_option_instance_values_with_key_value_pair_options()
        {
            // Arrange
            var options = new MetricsOptions
                          {
                              AddDefaultGlobalTags = true,
                              Enabled = true,
                              DefaultContextLabel = "initial value",
                              GlobalTags = new GlobalMetricTags(new Dictionary<string, string> { { "initial", "value" } })
                          };

            var keyValuePairs = new Dictionary<string, string>
                                {
                                    { "MetricsOptions:AddDefaultGlobalTags", "false" },
                                    { "MetricsOptions:DefaultContextLabel", "Testing" },
                                    { "MetricsOptions:GlobalTags", "tag1=value1,tag2=value2" },
                                    { "MetricsOptions:Enabled", "false" }
                                };

            // Act
            var metrics = new MetricsBuilder().Options.Configure(options, keyValuePairs).Build();

            // Assert
            metrics.Options.AddDefaultGlobalTags.Should().BeFalse();
            metrics.Options.DefaultContextLabel.Should().Be("Testing");
            metrics.Options.GlobalTags.Count.Should().Be(2);
            metrics.Options.GlobalTags.First().Key.Should().Be("tag1");
            metrics.Options.GlobalTags.First().Value.Should().Be("value1");
            metrics.Options.GlobalTags.Skip(1).First().Key.Should().Be("tag2");
            metrics.Options.GlobalTags.Skip(1).First().Value.Should().Be("value2");
            metrics.Options.Enabled.Should().BeFalse();
        }

        [Fact]
        public void Can_set_options_via_key_value_pairs()
        {
            // Arrange
            var keyValuePairs = new Dictionary<string, string>
                                {
                                    { "MetricsOptions:AddDefaultGlobalTags", "false" },
                                    { "MetricsOptions:DefaultContextLabel", "Testing" },
                                    { "MetricsOptions:GlobalTags", "tag1=value1,tag2=value2" },
                                    { "MetricsOptions:Enabled", "false" }
                                };

            // Act
            var metrics = new MetricsBuilder().Options.Configure(keyValuePairs).Build();

            // Assert
            metrics.Options.AddDefaultGlobalTags.Should().BeFalse();
            metrics.Options.DefaultContextLabel.Should().Be("Testing");
            metrics.Options.GlobalTags.Count.Should().Be(2);
            metrics.Options.GlobalTags.First().Key.Should().Be("tag1");
            metrics.Options.GlobalTags.First().Value.Should().Be("value1");
            metrics.Options.GlobalTags.Skip(1).First().Key.Should().Be("tag2");
            metrics.Options.GlobalTags.Skip(1).First().Value.Should().Be("value2");
            metrics.Options.Enabled.Should().BeFalse();
        }

        [Fact]
        public void Can_set_options_via_setup_action()
        {
            // Arrange
            void SetupAction(MetricsOptions options)
            {
                options.Enabled = false;
                options.GlobalTags.Add("tag1", "value1");
                options.AddDefaultGlobalTags = false;
                options.DefaultContextLabel = "test";
            }

            // Act
            var metrics = new MetricsBuilder().Options.Configure(SetupAction).Build();

            // Assert
            metrics.Options.AddDefaultGlobalTags.Should().BeFalse();
            metrics.Options.DefaultContextLabel.Should().Be("test");
            metrics.Options.GlobalTags.Count.Should().Be(1);
            metrics.Options.GlobalTags.First().Key.Should().Be("tag1");
            metrics.Options.GlobalTags.First().Value.Should().Be("value1");
            metrics.Options.Enabled.Should().BeFalse();
        }

        [Fact]
        public void Can_set_options_with_instance()
        {
            // Arrange
            var options = new MetricsOptions();
            options.Enabled = true;
            options.GlobalTags.Add("tag1", "value1");
            options.AddDefaultGlobalTags = false;
            options.DefaultContextLabel = "test";

            // Act
            var metrics = new MetricsBuilder().Options.Configure(options).Build();

            // Assert
            metrics.Options.AddDefaultGlobalTags.Should().BeFalse();
            metrics.Options.DefaultContextLabel.Should().Be("test");
            metrics.Options.GlobalTags.Count.Should().Be(1);
            metrics.Options.GlobalTags.First().Key.Should().Be("tag1");
            metrics.Options.GlobalTags.First().Value.Should().Be("value1");
            metrics.Options.Enabled.Should().BeTrue();
        }

        [Fact]
        public void Should_add_env_tags_when_required()
        {
            // Arrange
            void SetupAction(MetricsOptions options)
            {
                options.AddDefaultGlobalTags = true;
                options.GlobalTags.Add("tag1", "value1");
            }

            // Act
            var metrics = new MetricsBuilder().Options.Configure(SetupAction).Build();

            // Assert
            metrics.Options.GlobalTags.Count.Should().Be(4);
            metrics.Options.GlobalTags.First().Key.Should().Be("tag1");
            metrics.Options.GlobalTags.First().Value.Should().Be("value1");
            metrics.Options.GlobalTags.Skip(1).First().Key.Should().Be("app");
            metrics.Options.GlobalTags.Skip(1).First().Value.Should().NotBeNullOrWhiteSpace();
            metrics.Options.GlobalTags.Skip(2).First().Key.Should().Be("server");
            metrics.Options.GlobalTags.Skip(2).First().Value.Should().NotBeNullOrWhiteSpace();
            metrics.Options.GlobalTags.Skip(3).First().Key.Should().Be("env");
            metrics.Options.GlobalTags.Skip(3).First().Value.Should().NotBeNullOrWhiteSpace();
        }
    }
}