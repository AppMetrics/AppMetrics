using System.Threading.Tasks;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Facts.Formatting.TestHelpers;
using App.Metrics.Formatting;
using App.Metrics.Formatting.Ascii;
using App.Metrics.Health;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Formatting
{
    public class AsciiFormatterTests
    {
        private readonly MetricsFixture _fixture;

        public AsciiFormatterTests()
        {
            //DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new MetricsFixture();
        }

        [Fact]
        public async Task can_apply_ascii_health_formatting()
        {
            // Arrange
            _fixture.HealthCheckFactory.Register("test", () => Task.FromResult(HealthCheckResult.Healthy()));
            var formatter = new HealthStatusPayloadFormatter();
            var payloadBuilder = new AsciiHealthStatusPayloadBuilder();

            // Act
            var healthStatus = await _fixture.Metrics.Health.ReadStatusAsync();
            formatter.Build(healthStatus, payloadBuilder);

            // Assert
            payloadBuilder.PayloadFormatted().Should().Be("# OVERALL STATUS: Healthy\n--------------------------------------------------------------\n# CHECK: test\n\n           MESSAGE = OK\n            STATUS = Healthy\n--------------------------------------------------------------\n");
        }

        [Fact]
        public void can_apply_ascii_metric_formatting()
        {
            // Arrange
            var counter = new CounterOptions { Context = "test", Name = "counter1" };
            var formatter = new MetricDataValueSourceFormatter();
            var payloadBuilder = new AsciiMetricPayloadBuilder();

            // Act
            _fixture.Metrics.Measure.Counter.Increment(counter);
            formatter.Build(_fixture.Metrics.Snapshot.Get(), payloadBuilder);

            // Assert
            payloadBuilder.PayloadFormatted().Should().Be("# MEASUREMENT: [test] counter1\n# TAGS:\n             mtype = counter\n              unit = none\n# FIELDS:\n             value = 1\n--------------------------------------------------------------\n");
        }

        [Fact]
        public void can_apply_ascii_metric_formatting_with_custom_name_formatter()
        {
            // Arrange
            var counter = new CounterOptions { Context = "test", Name = "counter1" };
            var formatter = new MetricDataValueSourceFormatter();
            var payloadBuilder = new AsciiMetricPayloadBuilder((context, name) => $"{context}---{name}");

            // Act
            _fixture.Metrics.Measure.Counter.Increment(counter);
            formatter.Build(_fixture.Metrics.Snapshot.Get(), payloadBuilder);

            // Assert
            payloadBuilder.PayloadFormatted().Should().Be("# MEASUREMENT: test---counter1\n# TAGS:\n             mtype = counter\n              unit = none\n# FIELDS:\n             value = 1\n--------------------------------------------------------------\n");
        }
    }
}