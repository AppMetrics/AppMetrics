using System.Collections.Generic;
using App.Metrics.Core;
using App.Metrics.Extensions.Reporting.InfluxDB.Extensions;
using App.Metrics.Utils;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Extensions
{
    public class MetricValueExtensionsTests
    {
        [Fact]
        public void can_add_apdex_values()
        {
            var clock = new TestClock();
            var apdex = new ApdexMetric(SamplingType.ExponentiallyDecaying, 1028, 0.015, clock, true);
            apdex.Track(10000);
            var values = new Dictionary<string, object>();
            apdex.Value.AddApdexValues(values);

            values.Keys.Should().Contain("samples");
            values.Keys.Should().Contain("score");
            values.Keys.Should().Contain("satisfied");
            values.Keys.Should().Contain("tolerating");
            values.Keys.Should().Contain("frustrating");
        }

        [Fact]
        public void can_add_histgoram_values()
        {
            var histogramMetric = new HistogramMetric(SamplingType.ExponentiallyDecaying, 1028, 0.015);
            histogramMetric.Update(10000, "value");
            var values = new Dictionary<string, object>();
            histogramMetric.Value.AddHistogramValues(values);

            values.Keys.Should().Contain("samples");
            values.Keys.Should().Contain("last");
            values.Keys.Should().Contain("count.hist");
            values.Keys.Should().Contain("min");
            values.Keys.Should().Contain("max");
            values.Keys.Should().Contain("mean");
            values.Keys.Should().Contain("median");
            values.Keys.Should().Contain("stddev");
            values.Keys.Should().Contain("p999");
            values.Keys.Should().Contain("p99");
            values.Keys.Should().Contain("p98");
            values.Keys.Should().Contain("p95");
            values.Keys.Should().Contain("p75");
            values.Keys.Should().Contain("user.last");
            values.Keys.Should().Contain("user.min");
            values.Keys.Should().Contain("user.max");
        }
    }
}