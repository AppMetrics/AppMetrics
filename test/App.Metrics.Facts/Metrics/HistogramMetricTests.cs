using App.Metrics.Core;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class HistogramMetricTests
    {
        private readonly HistogramMetric _histogram = new HistogramMetric();

        [Fact]
        public void HistogramMetric_CanCount()
        {
            _histogram.Update(1L);
            _histogram.Value.Count.Should().Be(1);
            _histogram.Update(1L);
            _histogram.Value.Count.Should().Be(2);
        }

        [Fact]
        public void HistogramMetric_CanReset()
        {
            _histogram.Update(1L);
            _histogram.Update(10L);

            _histogram.Value.Count.Should().NotBe(0);
            _histogram.Value.LastValue.Should().NotBe(0);
            _histogram.Value.Median.Should().NotBe(0);

            _histogram.Reset();

            _histogram.Value.Count.Should().Be(0);
            _histogram.Value.LastValue.Should().Be(0);
            _histogram.Value.Median.Should().Be(0);
        }

        [Fact]
        public void HistogramMetric_RecordsMeanForOneElement()
        {
            _histogram.Update(1L);
            _histogram.Value.Mean.Should().Be(1);
        }

        [Fact]
        public void HistogramMetric_RecordsUserValue()
        {
            _histogram.Update(1L, "A");
            _histogram.Update(10L, "B");

            _histogram.Value.MinUserValue.Should().Be("A");
            _histogram.Value.MaxUserValue.Should().Be("B");
        }
    }
}