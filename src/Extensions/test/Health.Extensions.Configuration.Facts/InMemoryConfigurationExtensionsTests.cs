// <copyright file="InMemoryConfigurationExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Extensions.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Health.Extensions.Configuration.Facts
{
    public class InMemoryConfigurationExtensionsTests
    {
        [Fact]
        public void Can_bind_metrics_options_from_configuration()
        {
            // Arrange
            var builder = new HealthBuilder();
            var keyValuePairs = new Dictionary<string, string>
                                {
                                    { "HealthOptions:Enabled", "false" }
                                };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(keyValuePairs).Build();

            // Act
            builder.Configuration.ReadFrom(configuration);
            var health = builder.Build();

            // Assert
            health.Options.Enabled.Should().BeFalse();
        }
    }
}
