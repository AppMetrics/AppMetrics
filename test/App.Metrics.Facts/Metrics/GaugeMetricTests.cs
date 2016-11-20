using System;
using App.Metrics.Core;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class GaugeMetricTests
    {
        [Fact]
        public void should_report_nan_on_exception()
        {
            new FunctionGauge(() => { throw new InvalidOperationException("test"); }).Value.Should().Be(double.NaN);

            new DerivedGauge(new FunctionGauge(() => 5.0), (d) => { throw new InvalidOperationException("test"); }).Value.Should().Be(double.NaN);
        }
    }
}