// <copyright file="MiddlewareAppMetricsBuilderExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Middleware.Options;
using App.Metrics.Builder;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.DependencyInjection
{
    public class MiddlewareAppMetricsBuilderExtensionsTests
    {
        [Fact]
        public void Can_load_settings_from_configuration()
        {
            var options = new AppMetricsMiddlewareOptions();
            var provider = SetupServicesAndConfiguration();
            Action resolveOptions = () => { options = provider.GetRequiredService<AppMetricsMiddlewareOptions>(); };

            resolveOptions.ShouldNotThrow();
            options.ApdexTrackingEnabled.Should().Be(false);
            options.ApdexTSeconds.Should().Be(0.8);
            options.MetricsEndpoint.Should().Be("/metrics-test");
            options.MetricsTextEndpoint.Should().Be("/metrics-text-test");
            options.PingEndpoint.Should().Be("/ping-test");
            options.MetricsTextEndpointEnabled.Should().Be(false);
            options.MetricsEndpointEnabled.Should().Be(false);
            options.PingEndpointEnabled.Should().Be(false);
        }

        [Fact]
        public void Can_override_settings_from_configuration()
        {
            var options = new AppMetricsMiddlewareOptions();
            var provider = SetupServicesAndConfiguration(
                (o) =>
                {
                    o.ApdexTSeconds = 0.7;
                    o.ApdexTrackingEnabled = true;
                });

            Action resolveOptions = () => { options = provider.GetRequiredService<AppMetricsMiddlewareOptions>(); };

            resolveOptions.ShouldNotThrow();
            options.ApdexTrackingEnabled.Should().Be(true);
            options.ApdexTSeconds.Should().Be(0.7);
        }

        private IServiceProvider SetupServicesAndConfiguration(
            Action<AppMetricsMiddlewareOptions> setupAction = null)
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("DependencyInjection/TestConfiguration/appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var metricsBuilder = services.AddMetrics();

            if (setupAction == null)
            {
                metricsBuilder.AddMetricsMiddleware(
                    configuration.GetSection("AspNetMetrics"),
                    optionsBuilder =>
                    {
                        optionsBuilder.AddEnvironmentAsciiFormatters().
                                      AddMetricsJsonFormatters().
                                      AddMetricsTextJsonFormatters();
                    });
            }
            else
            {
                metricsBuilder.AddMetricsMiddleware(
                    configuration.GetSection("AspNetMetrics"),
                    setupAction,
                    optionsBuilder =>
                    {
                        optionsBuilder.AddEnvironmentAsciiFormatters().
                                       AddMetricsJsonFormatters().
                                       AddMetricsTextJsonFormatters();
                    });
            }

            return services.BuildServiceProvider();
        }
    }
}