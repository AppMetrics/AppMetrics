using App.Metrics.Core;
using App.Metrics.Utils;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class MeterMetricTests
    {
        private readonly Clock.TestClock _clock = new Clock.TestClock();
        private readonly MeterMetric _meter;

        public MeterMetricTests()
        {
            var scheduler = new TestScheduler(_clock);
            _meter = new MeterMetric(_clock, scheduler);
        }

        [Fact]
        public void MeterMetric_CanCalculateMeanRate()
        {
            _meter.Mark();
            _clock.Advance(TimeUnit.Seconds, 1);

            _meter.Value.MeanRate.Should().Be(1);

            _clock.Advance(TimeUnit.Seconds, 1);

            _meter.Value.MeanRate.Should().Be(0.5);
        }

        [Fact]
        public void MeterMetric_CanComputePercentWithZeroTotal()
        {
            _meter.Mark("A");
            _meter.Mark("A", -1);

            _meter.Value.Count.Should().Be(0);

            _meter.Value.Items[0].Item.Should().Be("A");
            _meter.Value.Items[0].Value.Count.Should().Be(0);
            _meter.Value.Items[0].Percent.Should().Be(0);
        }

        [Fact]
        public void MeterMetric_CanComputeRates()
        {
            _meter.Mark();
            _clock.Advance(TimeUnit.Seconds, 10);
            _meter.Mark(2);

            var value = _meter.Value;

            value.MeanRate.Should().BeApproximately(0.3, 0.001);
            value.OneMinuteRate.Should().BeApproximately(0.1840, 0.001);
            value.FiveMinuteRate.Should().BeApproximately(0.1966, 0.001);
            value.FifteenMinuteRate.Should().BeApproximately(0.1988, 0.001);
        }

        [Fact]
        public void MeterMetric_CanCount()
        {
            _meter.Mark();

            _meter.Value.Count.Should().Be(1L);

            _meter.Mark();
            _meter.Value.Count.Should().Be(2L);

            _meter.Mark(8L);
            _meter.Value.Count.Should().Be(10L);
        }

        [Fact]
        public void MeterMetric_CanCountForMultipleSetItem()
        {
            _meter.Mark("A");
            _meter.Mark("B");

            _meter.Value.Count.Should().Be(2L);
            _meter.Value.Items.Should().HaveCount(2);

            _meter.Value.Items[0].Item.Should().Be("A");
            _meter.Value.Items[0].Value.Count.Should().Be(1);
            _meter.Value.Items[0].Percent.Should().Be(50);

            _meter.Value.Items[1].Item.Should().Be("B");
            _meter.Value.Items[1].Value.Count.Should().Be(1);
            _meter.Value.Items[1].Percent.Should().Be(50);
        }

        [Fact]
        public void MeterMetric_CanCountForSetItem()
        {
            _meter.Mark("A");
            _meter.Value.Count.Should().Be(1L);
            _meter.Value.Items.Should().HaveCount(1);

            _meter.Value.Items[0].Item.Should().Be("A");
            _meter.Value.Items[0].Value.Count.Should().Be(1);
            _meter.Value.Items[0].Percent.Should().Be(100);
        }

        [Fact]
        public void MeterMetric_CanReset()
        {
            _meter.Mark();
            _meter.Mark();
            _clock.Advance(TimeUnit.Seconds, 10);
            _meter.Mark(2);

            _meter.Reset();
            _meter.Value.Count.Should().Be(0L);
            _meter.Value.OneMinuteRate.Should().Be(0);
            _meter.Value.FiveMinuteRate.Should().Be(0);
            _meter.Value.FifteenMinuteRate.Should().Be(0);
        }

        [Fact]
        public void MeterMetric_CanResetSetItem()
        {
            _meter.Mark("A");
            _meter.Value.Items[0].Value.Count.Should().Be(1);
            _meter.Reset();
            _meter.Value.Items[0].Value.Count.Should().Be(0L);
        }

        [Fact]
        public void MeterMetric_StartsAtZero()
        {
            _meter.Value.MeanRate.Should().Be(0L);
            _meter.Value.OneMinuteRate.Should().Be(0L);
            _meter.Value.FiveMinuteRate.Should().Be(0L);
            _meter.Value.FifteenMinuteRate.Should().Be(0L);
        }

        [Fact]
        public void MeterMetric_ValueCanScaleDown()
        {
            this._meter.Mark();
            this._clock.Advance(TimeUnit.Milliseconds, 1);
            this._meter.Mark();
            this._clock.Advance(TimeUnit.Milliseconds, 1);

            var scaledValue = this._meter.Value.Scale(TimeUnit.Milliseconds);

            scaledValue.MeanRate.Should().Be(1);
        }

        [Fact]
        public void MeterMetric_ValueCanScaleDownToDecimals()
        {
            this._meter.Mark();
            this._clock.Advance(TimeUnit.Seconds, 1);
            this._meter.Mark();
            this._clock.Advance(TimeUnit.Seconds, 1);

            var scaledValue = this._meter.Value.Scale(TimeUnit.Milliseconds);

            scaledValue.MeanRate.Should().Be(0.001);
        }

        [Fact]
        public void MeterMetric_ValueCanScaleUp()
        {
            this._meter.Mark();
            this._clock.Advance(TimeUnit.Minutes, 1);
            this._meter.Mark();
            this._clock.Advance(TimeUnit.Minutes, 1);

            var scaledValue = this._meter.Value.Scale(TimeUnit.Minutes);

            scaledValue.MeanRate.Should().Be(1);
        }
    }
}