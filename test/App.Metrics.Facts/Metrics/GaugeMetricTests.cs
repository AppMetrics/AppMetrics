using System;
using App.Metrics.Core;
using App.Metrics.Internal;
using App.Metrics.Internal.Test;
using App.Metrics.Sampling;
using App.Metrics.Utils;
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

        [Theory]
        [InlineData(2.0, 4.0, 50.0)]
        [InlineData(0.0, 4.0, 0.0)]
        [InlineData(4.0, 2.0, 100.0)]
        [InlineData(4.0, 0.0, 100.0)]        
        public void can_calculate_percentage(double numerator, double denominator, double expectedPercentage)
        {
           var hitPercentage = new PercentageGauge(() => numerator, () => denominator);

            hitPercentage.Value.Should().Be(expectedPercentage);
        }
    }
}