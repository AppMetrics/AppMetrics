// <copyright file="KeyValuePairMetricsOptionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Options
{
    public class KeyValuePairMetricsOptionsTests
    {
        [Fact]
        public void Can_load_options_from_key_values()
        {
            // Arrange
            var keyValuePairs = new Dictionary<string, string>
                          {
                              { "MetricsOptions:DefaultContextLabel", "Testing" },
                              { "MetricsOptions:GlobalTags:tag1", "value1" },
                              { "MetricsOptions:GlobalTags:tag2", "value2" },
                              { "MetricsOptions:Enabled", "false" }
                          };

            // Act
            var options = new KeyValuePairMetricsOptions(keyValuePairs).AsOptions();

            // Assert
            options.DefaultContextLabel.Should().Be("Testing");
            options.GlobalTags.Count.Should().Be(2);
            options.GlobalTags.First().Key.Should().Be("tag1");
            options.GlobalTags.First().Value.Should().Be("value1");
            options.GlobalTags.Skip(1).First().Key.Should().Be("tag2");
            options.GlobalTags.Skip(1).First().Value.Should().Be("value2");
            options.Enabled.Should().BeFalse();
        }

        [Fact]
        public void Key_value_pairs_cannot_be_null()
        {
            // Arrange
            // Act
            Action action = () =>
            {
                var unused = new KeyValuePairMetricsOptions(null);
            };

            // Assert
           action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Metrics_enabled_should_be_bool()
        {
            // Arrange
            // Act
            Action action = () =>
            {
                var keyValuePairs = new Dictionary<string, string>
                                    {
                                        { "MetricsOptions:Enabled", "not_a_bool" }
                                    };

                // Act
                var unused = new KeyValuePairMetricsOptions(keyValuePairs).AsOptions();
            };

            // Assert
            action.Should().Throw<InvalidCastException>();
        }

        [Theory]
        [InlineData("  ")]
        [InlineData(null)]
        public void Global_tags_should_be_formatted_correctly(string tags)
        {
            // Arrange
            // Act
            Action action = () =>
            {
                var keyValuePairs = new Dictionary<string, string>
                                    {
                                        { "MetricsOptions:GlobalTags:test", tags }
                                    };

                // Act
                var unused = new KeyValuePairMetricsOptions(keyValuePairs).AsOptions();
            };

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }
    }
}
