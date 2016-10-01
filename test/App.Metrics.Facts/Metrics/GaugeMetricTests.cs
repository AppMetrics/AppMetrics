using System;
using App.Metrics.Core;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class GaugeMetricTests
    {
        [Fact]
        public void GaugeMetric_ReportsNanOnException()
        {
            new FunctionGauge(() => { throw new InvalidOperationException("test"); }).Value.Should().Be(double.NaN);

            new DerivedGauge(new FunctionGauge(() => 5.0), (d) => { throw new InvalidOperationException("test"); }).Value.Should().Be(double.NaN);
        }
    }
}