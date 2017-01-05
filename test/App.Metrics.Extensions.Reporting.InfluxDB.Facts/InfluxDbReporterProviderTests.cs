using System;
using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Internal;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts
{
    public class InfluxDbReporterProviderTests
    {
        [Fact]
        public void can_create_metric_reporter()
        {
            var provider = new InfluxDbReporterProvider(new InfluxDBReporterSettings(), new DefaultMetricsFilter());

            var reporter = provider.CreateMetricReporter("influx", new LoggerFactory());

            reporter.Should().NotBeNull();
        }

        [Fact]
        public void defaults_filter_to_no_op()
        {
            var provider = new InfluxDbReporterProvider(new InfluxDBReporterSettings());

            provider.Filter.Should().BeOfType<NoOpMetricsFilter>();
        }

        [Fact]
        public void filter_is_not_required()
        {
            Action action = () =>
            {
                var provider = new InfluxDbReporterProvider(new InfluxDBReporterSettings(), null);
                provider.Filter.Should().BeOfType<NoOpMetricsFilter>();
            };

            action.ShouldNotThrow();
        }

        [Fact]
        public void settings_are_required()
        {
            Action action = () =>
            {
                var provider = new InfluxDbReporterProvider(null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }
    }
}