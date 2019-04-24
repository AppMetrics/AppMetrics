// <copyright file="MiddlewareAppMetricsBuilderExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;
using App.Metrics.Extensions.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.DependencyInjection
{
    public class MiddlewareAppMetricsBuilderExtensionsTests
    {
        [Fact]
        public void Can_load_settings_from_configuration()
        {
            var trackingOptions = new MetricsWebTrackingOptions();
            var endpointOptions = new MetricEndpointsOptions();

            var provider = SetupServicesAndConfiguration();
            Action resolveOptions = () => { trackingOptions = provider.GetRequiredService<IOptions<MetricsWebTrackingOptions>>().Value; };
            Action resolveEndpointsOptions = () => { endpointOptions = provider.GetRequiredService<IOptions<MetricEndpointsOptions>>().Value; };

            resolveOptions.Should().NotThrow();
            resolveEndpointsOptions.Should().NotThrow();

            trackingOptions.ApdexTrackingEnabled.Should().Be(false);
            trackingOptions.ApdexTSeconds.Should().Be(0.8);
            endpointOptions.MetricsTextEndpointEnabled.Should().Be(false);
            endpointOptions.MetricsEndpointEnabled.Should().Be(false);
        }

        [Fact]
        public void Can_override_settings_from_configuration()
        {
            var options = new MetricsWebTrackingOptions();
            var provider = SetupServicesAndConfiguration(
                (o) =>
                {
                    o.ApdexTSeconds = 0.7;
                    o.ApdexTrackingEnabled = true;
                });

            Action resolveOptions = () => { options = provider.GetRequiredService<IOptions<MetricsWebTrackingOptions>>().Value; };

            resolveOptions.Should().NotThrow();
            options.ApdexTrackingEnabled.Should().Be(true);
            options.ApdexTSeconds.Should().Be(0.7);
        }

        private IServiceProvider SetupServicesAndConfiguration(
            Action<MetricsWebTrackingOptions> trackingSetupAction = null,
            Action<MetricEndpointsOptions> setupEndpointAction = null)
        {
            var services = new ServiceCollection();
            services.AddOptions();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("DependencyInjection/TestConfiguration/appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var metricsBuilder = AppMetrics.CreateDefaultBuilder()
                .Configuration.ReadFrom(configuration);
            services.AddMetrics(metricsBuilder);

            if (setupEndpointAction == null)
            {
                services.AddMetricsEndpoints(configuration.GetSection(nameof(MetricEndpointsOptions)));
            }
            else
            {
                services.AddMetricsEndpoints(configuration.GetSection(nameof(MetricEndpointsOptions)), setupEndpointAction);
            }

            if (trackingSetupAction == null)
            {
                services.AddMetricsTrackingMiddleware(configuration.GetSection(nameof(MetricsWebTrackingOptions)));
            }
            else
            {
                services.AddMetricsTrackingMiddleware(configuration.GetSection(nameof(MetricsWebTrackingOptions)), trackingSetupAction);
            }

            return services.BuildServiceProvider();
        }
    }
}