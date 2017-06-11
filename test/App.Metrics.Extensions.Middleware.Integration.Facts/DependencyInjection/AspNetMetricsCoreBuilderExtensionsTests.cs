// <copyright file="AspNetMetricsCoreBuilderExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.DependencyInjection
{
    public class AspNetMetricsCoreBuilderExtensionsTests
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
            options.HealthEndpoint.Should().Be("/health-test");
            options.MetricsEndpoint.Should().Be("/metrics-test");
            options.MetricsTextEndpoint.Should().Be("/metrics-text-test");
            options.PingEndpoint.Should().Be("/ping-test");
            options.HealthEndpointEnabled.Should().Be(false);
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
                    o.HealthEndpointEnabled = true;
                });

            Action resolveOptions = () => { options = provider.GetRequiredService<AppMetricsMiddlewareOptions>(); };

            resolveOptions.ShouldNotThrow();
            options.ApdexTrackingEnabled.Should().Be(true);
            options.ApdexTSeconds.Should().Be(0.7);
            options.HealthEndpointEnabled.Should().Be(true);
        }

        private IServiceProvider SetupServicesAndConfiguration(Action<AppMetricsMiddlewareOptions> setupAction = null)
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("DependencyInjection/TestConfiguration/appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var metricsBuidler = services.AddMetrics();

            if (setupAction == null)
            {
                metricsBuidler.AddMetricsMiddleware(
                    configuration.GetSection("AspNetMetrics"),
                    optionsBuilder =>
                    {
                        optionsBuilder.AddAsciiEnvironmentInfoSerialization().
                                      AddAsciiHealthSerialization().
                                      AddJsonMetricsSerialization().
                                      AddJsonMetricsTextSerialization();
                    });
            }
            else
            {
                metricsBuidler.AddMetricsMiddleware(
                    configuration.GetSection("AspNetMetrics"),
                    setupAction,
                    optionsBuilder =>
                    {
                        optionsBuilder.AddAsciiEnvironmentInfoSerialization().
                                       AddAsciiHealthSerialization().
                                       AddJsonMetricsSerialization().
                                       AddJsonMetricsTextSerialization();
                    });
            }

            return services.BuildServiceProvider();
        }
    }
}