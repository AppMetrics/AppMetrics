using System;
using App.Metrics.Core;
using App.Metrics.Internal.Test;
using App.Metrics.Sampling;
using App.Metrics.Utils;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class TimerMetricTests
    {
        private readonly Clock.TestClock _clock = new Clock.TestClock();
        private readonly TimerMetric _timer;

        public TimerMetricTests()
        {
            _timer = new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(_clock, new TestTaskScheduler(_clock)),
                _clock);
        }

        [Fact]
        public void TimerMetric_CanCount()
        {
            _timer.Value.Rate.Count.Should().Be(0);
            using (_timer.NewContext())
            {
            }
            _timer.Value.Rate.Count.Should().Be(1);
            using (_timer.NewContext())
            {
            }
            _timer.Value.Rate.Count.Should().Be(2);
            _timer.Time(() => { });
            _timer.Value.Rate.Count.Should().Be(3);
            _timer.Time(() => 1);
            _timer.Value.Rate.Count.Should().Be(4);
        }

        [Fact]
        public void TimerMetric_CanReset()
        {
            using (var context = _timer.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            _timer.Value.Rate.Count.Should().NotBe(0);
            _timer.Value.Histogram.Count.Should().NotBe(0);

            _timer.Reset();

            _timer.Value.Rate.Count.Should().Be(0);
            _timer.Value.Histogram.Count.Should().Be(0);
        }

        [Fact]
        public void TimerMetric_CanTrackTime()
        {
            using (_timer.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            _timer.Value.Histogram.Count.Should().Be(1);
            _timer.Value.Histogram.Max.Should().Be(TimeUnit.Milliseconds.ToNanoseconds(100));

            using (_timer.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 300);
            }

            _timer.Value.Histogram.Count.Should().Be(2);
            _timer.Value.Histogram.Min.Should().Be(TimeUnit.Milliseconds.ToNanoseconds(100));
            _timer.Value.Histogram.Max.Should().Be(TimeUnit.Milliseconds.ToNanoseconds(300));
        }

        [Fact]
        public void TimerMetric_ContextRecordsTimeOnlyOnFirstDispose()
        {
            var context = _timer.NewContext();
            _clock.Advance(TimeUnit.Milliseconds, 100);
            context.Dispose(); // passing the structure to using() creates a copy
            _clock.Advance(TimeUnit.Milliseconds, 100);
            context.Dispose();

            _timer.Value.Histogram.Count.Should().Be(1);
            _timer.Value.Histogram.Max.Should().Be(TimeUnit.Milliseconds.ToNanoseconds(100));
        }

        [Fact]
        public void TimerMetric_ContextReportsElapsedTime()
        {
            using (var context = _timer.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
                context.Elapsed.TotalMilliseconds.Should().Be(100);
            }
        }

        [Fact]
        public void TimerMetric_CountsEvenIfActionThrows()
        {
            Action action = () => this._timer.Time(() => { throw new InvalidOperationException(); });

            action.ShouldThrow<InvalidOperationException>();

            this._timer.Value.Rate.Count.Should().Be(1);
        }

        [Fact]
        public void TimerMetric_RecordsActiveSessions()
        {
            _timer.Value.ActiveSessions.Should().Be(0);
            var context1 = _timer.NewContext();
            _timer.Value.ActiveSessions.Should().Be(1);
            var context2 = _timer.NewContext();
            _timer.Value.ActiveSessions.Should().Be(2);
            context1.Dispose();
            _timer.Value.ActiveSessions.Should().Be(1);
            context2.Dispose();
            _timer.Value.ActiveSessions.Should().Be(0);
        }

        [Fact]
        public void TimerMetric_RecordsUserValue()
        {
            _timer.Record(1L, TimeUnit.Milliseconds, "A");
            _timer.Record(10L, TimeUnit.Milliseconds, "B");

            _timer.Value.Histogram.MinUserValue.Should().Be("A");
            _timer.Value.Histogram.MaxUserValue.Should().Be("B");
        }

        [Fact]
        public void TimerMetric_UserValueCanBeOverwrittenAfterContextCreation()
        {
            using (var x = _timer.NewContext("a"))
            {
                x.TrackUserValue("b");
            }

            _timer.Value.Histogram.LastUserValue.Should().Be("b");
        }

        [Fact]
        public void TimerMetric_UserValueCanBeSetAfterContextCreation()
        {
            using (var x = _timer.NewContext())
            {
                x.TrackUserValue("test");
            }

            _timer.Value.Histogram.LastUserValue.Should().Be("test");
        }
    }
}