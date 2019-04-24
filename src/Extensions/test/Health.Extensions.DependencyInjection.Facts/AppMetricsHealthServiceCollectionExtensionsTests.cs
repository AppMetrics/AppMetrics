// <copyright file="AppMetricsHealthServiceCollectionExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Health;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Health.Extensions.DependencyInjection.Facts
{
    public class AppMetricsHealthServiceCollectionExtensionsTests
    {
        [Fact]
        public void Can_resolve_health_from_service_collection()
        {
            // Arrange
            var builder = AppMetricsHealth.CreateDefaultBuilder();
            var services = new ServiceCollection();

            // Act
            services.AddHealth(builder);

            // Assert
            var provider = services.BuildServiceProvider();
            provider.GetService<IHealth>().Should().NotBeNull();
        }

        [Fact]
        public void Can_resolve_health_from_service_collection_when_pre_built()
        {
            // Arrange
            var metrics = AppMetricsHealth.CreateDefaultBuilder().Build();
            var services = new ServiceCollection();

            // Act
            services.AddHealth(metrics);

            // Assert
            var provider = services.BuildServiceProvider();
            provider.GetService<IHealthRoot>().Should().NotBeNull();
        }

        [Fact]
        public void Can_resolve_health_from_service_collection_when_using_builder_setup_action()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddHealth(builder =>
            {
                builder.Configuration.Configure(options => options.Enabled = false).OutputHealth.AsPlainText();
            });

            // Assert
            var provider = services.BuildServiceProvider();
            provider.GetService<IHealth>().Should().NotBeNull();
            provider.GetService<HealthOptions>().Enabled.Should().BeFalse();
        }
    }
}
