// <copyright file="MiddlewareHealthChecksAppMetricsBuilderExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Options;
using App.Metrics.Builder;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.DependencyInjection
{
    public class MiddlewareHealthChecksAppMetricsBuilderExtensionsTests
    {
        [Fact]
        public void Can_load_settings_from_configuration()
        {
            var options = new AppMetricsMiddlewareHealthChecksOptions();
            var provider = SetupServicesAndConfiguration();
            Action resolveOptions = () => { options = provider.GetRequiredService<AppMetricsMiddlewareHealthChecksOptions>(); };

            resolveOptions.ShouldNotThrow();
            options.HealthEndpoint.Should().Be("/health-test");
            options.HealthEndpointEnabled.Should().Be(false);
        }

        [Fact]
        public void Can_override_settings_from_configuration()
        {
            var options = new AppMetricsMiddlewareHealthChecksOptions();
            var provider = SetupServicesAndConfiguration(
                (o) =>
                {
                    o.HealthEndpointEnabled = true;
                });

            Action resolveOptions = () => { options = provider.GetRequiredService<AppMetricsMiddlewareHealthChecksOptions>(); };

            resolveOptions.ShouldNotThrow();
            options.HealthEndpointEnabled.Should().Be(true);
        }

        private IServiceProvider SetupServicesAndConfiguration(
            Action<AppMetricsMiddlewareHealthChecksOptions> setupHealthAction = null)
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("DependencyInjection/TestConfiguration/appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var healthChecksBuilder = services.AddHealthChecks();

            if (setupHealthAction == null)
            {
                healthChecksBuilder.AddHealthCheckMiddleware(
                    configuration.GetSection("AspNetMetrics"),
                    optionsBuilder =>
                    {
                        optionsBuilder.AddAsciiFormatters();
                    });
            }
            else
            {
                healthChecksBuilder.AddHealthCheckMiddleware(
                    configuration.GetSection("AspNetMetrics"),
                    setupHealthAction,
                    optionsBuilder =>
                    {
                        optionsBuilder.AddAsciiFormatters();
                    });
            }

            return services.BuildServiceProvider();
        }
    }
}