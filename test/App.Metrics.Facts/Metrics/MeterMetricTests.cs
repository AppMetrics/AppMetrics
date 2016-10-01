using App.Metrics.Core;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class MeterMetricTests
    {
        private readonly TestClock clock = new TestClock();
        private readonly MeterMetric meter;
        private readonly TestScheduler scheduler;

        public MeterMetricTests()
        {
            this.scheduler = new TestScheduler(this.clock);
            this.meter = new MeterMetric(this.clock, this.scheduler);
        }

        [Fact]
        public void MeterMetric_CanCalculateMeanRate()
        {
            meter.Mark();
            clock.Advance(TimeUnit.Seconds, 1);

            meter.Value.MeanRate.Should().Be(1);

            clock.Advance(TimeUnit.Seconds, 1);

            meter.Value.MeanRate.Should().Be(0.5);
        }

        [Fact]
        public void MeterMetric_CanComputePercentWithZeroTotal()
        {
            meter.Mark("A");
            meter.Mark("A", -1);

            meter.Value.Count.Should().Be(0);

            meter.Value.Items[0].Item.Should().Be("A");
            meter.Value.Items[0].Value.Count.Should().Be(0);
            meter.Value.Items[0].Percent.Should().Be(0);
        }

        [Fact]
        public void MeterMetric_CanComputeRates()
        {
            meter.Mark();
            clock.Advance(TimeUnit.Seconds, 10);
            meter.Mark(2);

            var value = meter.Value;

            value.MeanRate.Should().BeApproximately(0.3, 0.001);
            value.OneMinuteRate.Should().BeApproximately(0.1840, 0.001);
            value.FiveMinuteRate.Should().BeApproximately(0.1966, 0.001);
            value.FifteenMinuteRate.Should().BeApproximately(0.1988, 0.001);
        }

        [Fact]
        public void MeterMetric_CanCount()
        {
            meter.Mark();

            meter.Value.Count.Should().Be(1L);

            meter.Mark();
            meter.Value.Count.Should().Be(2L);

            meter.Mark(8L);
            meter.Value.Count.Should().Be(10L);
        }

        [Fact]
        public void MeterMetric_CanCountForMultipleSetItem()
        {
            meter.Mark("A");
            meter.Mark("B");

            meter.Value.Count.Should().Be(2L);
            meter.Value.Items.Should().HaveCount(2);

            meter.Value.Items[0].Item.Should().Be("A");
            meter.Value.Items[0].Value.Count.Should().Be(1);
            meter.Value.Items[0].Percent.Should().Be(50);

            meter.Value.Items[1].Item.Should().Be("B");
            meter.Value.Items[1].Value.Count.Should().Be(1);
            meter.Value.Items[1].Percent.Should().Be(50);
        }

        [Fact]
        public void MeterMetric_CanCountForSetItem()
        {
            meter.Mark("A");
            meter.Value.Count.Should().Be(1L);
            meter.Value.Items.Should().HaveCount(1);

            meter.Value.Items[0].Item.Should().Be("A");
            meter.Value.Items[0].Value.Count.Should().Be(1);
            meter.Value.Items[0].Percent.Should().Be(100);
        }

        [Fact]
        public void MeterMetric_CanReset()
        {
            meter.Mark();
            meter.Mark();
            clock.Advance(TimeUnit.Seconds, 10);
            meter.Mark(2);

            meter.Reset();
            meter.Value.Count.Should().Be(0L);
            meter.Value.OneMinuteRate.Should().Be(0);
            meter.Value.FiveMinuteRate.Should().Be(0);
            meter.Value.FifteenMinuteRate.Should().Be(0);
        }

        [Fact]
        public void MeterMetric_CanResetSetItem()
        {
            meter.Mark("A");
            meter.Value.Items[0].Value.Count.Should().Be(1);
            meter.Reset();
            meter.Value.Items[0].Value.Count.Should().Be(0L);
        }

        [Fact]
        public void MeterMetric_StartsAtZero()
        {
            meter.Value.MeanRate.Should().Be(0L);
            meter.Value.OneMinuteRate.Should().Be(0L);
            meter.Value.FiveMinuteRate.Should().Be(0L);
            meter.Value.FifteenMinuteRate.Should().Be(0L);
        }

        [Fact]
        public void MeterMetric_ValueCanScaleDown()
        {
            this.meter.Mark();
            this.clock.Advance(TimeUnit.Milliseconds, 1);
            this.meter.Mark();
            this.clock.Advance(TimeUnit.Milliseconds, 1);

            var scaledValue = this.meter.Value.Scale(TimeUnit.Milliseconds);

            scaledValue.MeanRate.Should().Be(1);
        }

        [Fact]
        public void MeterMetric_ValueCanScaleDownToDecimals()
        {
            this.meter.Mark();
            this.clock.Advance(TimeUnit.Seconds, 1);
            this.meter.Mark();
            this.clock.Advance(TimeUnit.Seconds, 1);

            var scaledValue = this.meter.Value.Scale(TimeUnit.Milliseconds);

            scaledValue.MeanRate.Should().Be(0.001);
        }

        [Fact]
        public void MeterMetric_ValueCanScaleUp()
        {
            this.meter.Mark();
            this.clock.Advance(TimeUnit.Minutes, 1);
            this.meter.Mark();
            this.clock.Advance(TimeUnit.Minutes, 1);

            var scaledValue = this.meter.Value.Scale(TimeUnit.Minutes);

            scaledValue.MeanRate.Should().Be(1);
        }
    }
}