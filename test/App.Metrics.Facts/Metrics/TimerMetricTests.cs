using System;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Sampling;
using App.Metrics.Utils;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class TimerMetricTests
    {
        private readonly Clock.TestClock clock = new Clock.TestClock();
        private readonly TimerMetric timer;

        public TimerMetricTests()
        {
            this.timer = new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(this.clock, new TestScheduler(this.clock)),
                this.clock);
        }

        [Fact]
        public void TimerMetric_CanCount()
        {
            timer.Value.Rate.Count.Should().Be(0);
            using (timer.NewContext())
            {
            }
            timer.Value.Rate.Count.Should().Be(1);
            using (timer.NewContext())
            {
            }
            timer.Value.Rate.Count.Should().Be(2);
            timer.Time(() => { });
            timer.Value.Rate.Count.Should().Be(3);
            timer.Time(() => 1);
            timer.Value.Rate.Count.Should().Be(4);
        }

        [Fact]
        public void TimerMetric_CanReset()
        {
            using (var context = timer.NewContext())
            {
                clock.Advance(TimeUnit.Milliseconds, 100);
            }

            timer.Value.Rate.Count.Should().NotBe(0);
            timer.Value.Histogram.Count.Should().NotBe(0);

            timer.Reset();

            timer.Value.Rate.Count.Should().Be(0);
            timer.Value.Histogram.Count.Should().Be(0);
        }

        [Fact]
        public void TimerMetric_CanTrackTime()
        {
            using (timer.NewContext())
            {
                clock.Advance(TimeUnit.Milliseconds, 100);
            }

            timer.Value.Histogram.Count.Should().Be(1);
            timer.Value.Histogram.Max.Should().Be(TimeUnit.Milliseconds.ToNanoseconds(100));

            using (timer.NewContext())
            {
                clock.Advance(TimeUnit.Milliseconds, 300);
            }

            timer.Value.Histogram.Count.Should().Be(2);
            timer.Value.Histogram.Min.Should().Be(TimeUnit.Milliseconds.ToNanoseconds(100));
            timer.Value.Histogram.Max.Should().Be(TimeUnit.Milliseconds.ToNanoseconds(300));
        }

        [Fact]
        public void TimerMetric_ContextRecordsTimeOnlyOnFirstDispose()
        {
            var context = timer.NewContext();
            clock.Advance(TimeUnit.Milliseconds, 100);
            context.Dispose(); // passing the structure to using() creates a copy
            clock.Advance(TimeUnit.Milliseconds, 100);
            context.Dispose();

            timer.Value.Histogram.Count.Should().Be(1);
            timer.Value.Histogram.Max.Should().Be(TimeUnit.Milliseconds.ToNanoseconds(100));
        }

        [Fact]
        public void TimerMetric_ContextReportsElapsedTime()
        {
            using (var context = timer.NewContext())
            {
                clock.Advance(TimeUnit.Milliseconds, 100);
                context.Elapsed.TotalMilliseconds.Should().Be(100);
            }
        }

        [Fact]
        public void TimerMetric_CountsEvenIfActionThrows()
        {
            Action action = () => this.timer.Time(() => { throw new InvalidOperationException(); });

            action.ShouldThrow<InvalidOperationException>();

            this.timer.Value.Rate.Count.Should().Be(1);
        }

        [Fact]
        public void TimerMetric_RecordsActiveSessions()
        {
            timer.Value.ActiveSessions.Should().Be(0);
            var context1 = timer.NewContext();
            timer.Value.ActiveSessions.Should().Be(1);
            var context2 = timer.NewContext();
            timer.Value.ActiveSessions.Should().Be(2);
            context1.Dispose();
            timer.Value.ActiveSessions.Should().Be(1);
            context2.Dispose();
            timer.Value.ActiveSessions.Should().Be(0);
        }

        [Fact]
        public void TimerMetric_RecordsUserValue()
        {
            timer.Record(1L, TimeUnit.Milliseconds, "A");
            timer.Record(10L, TimeUnit.Milliseconds, "B");

            timer.Value.Histogram.MinUserValue.Should().Be("A");
            timer.Value.Histogram.MaxUserValue.Should().Be("B");
        }

        [Fact]
        public void TimerMetric_UserValueCanBeOverwrittenAfterContextCreation()
        {
            using (var x = timer.NewContext("a"))
            {
                x.TrackUserValue("b");
            }

            timer.Value.Histogram.LastUserValue.Should().Be("b");
        }

        [Fact]
        public void TimerMetric_UserValueCanBeSetAfterContextCreation()
        {
            using (var x = timer.NewContext())
            {
                x.TrackUserValue("test");
            }

            timer.Value.Histogram.LastUserValue.Should().Be("test");
        }
    }
}