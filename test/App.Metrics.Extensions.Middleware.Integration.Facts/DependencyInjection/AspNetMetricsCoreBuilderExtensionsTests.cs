using System;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.DependencyInjection
{
    public class AspNetMetricsCoreBuilderExtensionsTests
    {
        [Fact]
        public void can_load_settings_from_configuration()
        {
            var options = new AspNetMetricsOptions();
            var provider = SetupServicesAndConfiguration();
            Action resolveOptions = () => { options = provider.GetRequiredService<AspNetMetricsOptions>(); };

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
        public void can_override_settings_from_configuration()
        {
            var options = new AspNetMetricsOptions();
            var provider = SetupServicesAndConfiguration(
                (o) =>
                {
                    o.ApdexTSeconds = 0.7;
                    o.ApdexTrackingEnabled = true;
                    o.HealthEndpointEnabled = true;
                });

            Action resolveOptions = () => { options = provider.GetRequiredService<AspNetMetricsOptions>(); };

            resolveOptions.ShouldNotThrow();
            options.ApdexTrackingEnabled.Should().Be(true);
            options.ApdexTSeconds.Should().Be(0.7);
            options.HealthEndpointEnabled.Should().Be(true);
        }

        public IServiceProvider SetupServicesAndConfiguration(Action<AspNetMetricsOptions> setupAction = null)
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
                .AddJsonFile("DependencyInjection/TestConfiguration/appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var metricsBuidler = services.AddMetrics();

            if (setupAction == null)
            {
                metricsBuidler.AddMetricsMiddleware(configuration.GetSection("AspNetMetrics"));
            }
            else
            {
                metricsBuidler.AddMetricsMiddleware(configuration.GetSection("AspNetMetrics"), setupAction);
            }

            return services.BuildServiceProvider();
        }
    }
}