// <copyright file="MetricsConfigurationBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Internal.Extensions;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class MetricsConfigurationBuilderTests
    {
        [Fact]
        public void Can_override_option_instance_values_with_key_value_pair_options()
        {
            // Arrange
            var options = new MetricsOptions
                          {
                              Enabled = true,
                              DefaultContextLabel = "initial value",
                              GlobalTags = new GlobalMetricTags(new Dictionary<string, string> { { "initial", "value" } })
                          };

            var keyValuePairs = new Dictionary<string, string>
                                {
                                    { "MetricsOptions:DefaultContextLabel", "Testing" },
                                    { "MetricsOptions:GlobalTags:tag1", "value1" },
                                    { "MetricsOptions:GlobalTags:tag2", "value2" },
                                    { "MetricsOptions:Enabled", "false" }
                                };

            // Act
            var metrics = new MetricsBuilder().Configuration.Configure(options, keyValuePairs).Build();

            // Assert
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
                                    { "MetricsOptions:DefaultContextLabel", "Testing" },
                                    { "MetricsOptions:GlobalTags:tag1", "value1" },
                                    { "MetricsOptions:GlobalTags:tag2", "value2" },
                                    { "MetricsOptions:Enabled", "false" },
                                    { "MetricsOptions:ReportingEnabled", "false" }
                                };

            // Act
            var metrics = new MetricsBuilder().Configuration.Configure(keyValuePairs).Build();

            // Assert
            metrics.Options.DefaultContextLabel.Should().Be("Testing");
            metrics.Options.GlobalTags.Count.Should().Be(2);
            metrics.Options.GlobalTags.First().Key.Should().Be("tag1");
            metrics.Options.GlobalTags.First().Value.Should().Be("value1");
            metrics.Options.GlobalTags.Skip(1).First().Key.Should().Be("tag2");
            metrics.Options.GlobalTags.Skip(1).First().Value.Should().Be("value2");
            metrics.Options.Enabled.Should().BeFalse();
            metrics.Options.ReportingEnabled.Should().BeFalse();
        }

        [Fact]
        public void Can_set_options_via_setup_action()
        {
            // Arrange
            void SetupAction(MetricsOptions options)
            {
                options.Enabled = false;
                options.GlobalTags.Add("tag1", "value1");
                options.DefaultContextLabel = "test";
            }

            // Act
            var metrics = new MetricsBuilder().Configuration.Configure(SetupAction).Build();

            // Assert
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
            options.DefaultContextLabel = "test";

            // Act
            var metrics = new MetricsBuilder().Configuration.Configure(options).Build();

            // Assert
            metrics.Options.DefaultContextLabel.Should().Be("test");
            metrics.Options.GlobalTags.Count.Should().Be(1);
            metrics.Options.GlobalTags.First().Key.Should().Be("tag1");
            metrics.Options.GlobalTags.First().Value.Should().Be("value1");
            metrics.Options.Enabled.Should().BeTrue();
        }

        [Fact]
        public void Should_merge_global_tags_when_configured_more_than_once()
        {
            // Arrange
            var options = new MetricsOptions();
            options.Enabled = true;
            options.GlobalTags.Add("tag1", "value1");
            options.DefaultContextLabel = "test";

            var options2 = new MetricsOptions();
            options2.Enabled = true;
            options2.GlobalTags.Add("tag2", "value2");
            options2.DefaultContextLabel = "test";

            // Act
            var metrics = new MetricsBuilder()
                .Configuration.Configure(options)
                .Configuration.Extend(options2)
                .Build();

            // Assert
            metrics.Options.GlobalTags.Count.Should().Be(2);
            metrics.Options.GlobalTags.First().Key.Should().Be("tag1");
            metrics.Options.GlobalTags.First().Value.Should().Be("value1");
            metrics.Options.GlobalTags.Skip(1).First().Key.Should().Be("tag2");
            metrics.Options.GlobalTags.Skip(1).First().Value.Should().Be("value2");
        }

        [Fact]
        public void Should_merge_global_tags_when_configured_more_than_once_key_values_provided()
        {
            // Arrange
            var options = new MetricsOptions();
            options.Enabled = true;
            options.GlobalTags.Add("tag1", "value1");
            options.DefaultContextLabel = "test";

            var options2 = new MetricsOptions();
            options2.Enabled = true;
            options2.GlobalTags.Add("tag2", "value2");
            options2.DefaultContextLabel = "test";

            // Act
            var metrics = new MetricsBuilder()
                .Configuration.Configure(options)
                .Configuration.Extend(options2.ToKeyValue())
                .Build();

            // Assert
            metrics.Options.GlobalTags.Count.Should().Be(2);
            metrics.Options.GlobalTags.First().Key.Should().Be("tag1");
            metrics.Options.GlobalTags.First().Value.Should().Be("value1");
            metrics.Options.GlobalTags.Skip(1).First().Key.Should().Be("tag2");
            metrics.Options.GlobalTags.Skip(1).First().Value.Should().Be("value2");
        }

        [Fact]
        public void Should_merge_global_tags_when_key_values_provided_that_match_an_existing_tag()
        {
            // Arrange
            var keyValuePairs = new Dictionary<string, string>
                                {
                                    { "MetricsOptions:GlobalTags:tag1", "replaced" },
                                    { "MetricsOptions:GlobalTags:tag2", "added" }
                                };
            var options = new MetricsOptions();
            options.GlobalTags.Add("tag1", "value1");

            // Act
            var metrics = new MetricsBuilder().Configuration.Configure(options, keyValuePairs).Build();

            // Assert
            metrics.Options.GlobalTags.Count.Should().Be(2);
            metrics.Options.GlobalTags.First().Key.Should().Be("tag1");
            metrics.Options.GlobalTags.First().Value.Should().Be("replaced");
            metrics.Options.GlobalTags.Skip(1).First().Key.Should().Be("tag2");
            metrics.Options.GlobalTags.Skip(1).First().Value.Should().Be("added");
        }

        [Fact]
        public void Should_add_env_tags_when_required()
        {
            // Arrange
            void SetupAction(MetricsOptions options)
            {
                options.GlobalTags.Add("tag1", "value1");
                options.AddAppTag();
                options.AddServerTag();
                options.AddEnvTag();
            }

            // Act
            var metrics = new MetricsBuilder().Configuration.Configure(SetupAction).Build();

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