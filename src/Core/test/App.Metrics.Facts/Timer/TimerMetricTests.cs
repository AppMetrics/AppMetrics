// <copyright file="TimerMetricTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.FactsCommon;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Timer
{
    public class TimerMetricTests
    {
        private readonly IClock _clock = new TestClock();
        private readonly DefaultTimerMetric _timer;

        public TimerMetricTests()
        {
            _timer = new DefaultTimerMetric(
                new DefaultHistogramMetric(new DefaultForwardDecayingReservoir()),
                new DefaultMeterMetric(_clock),
                _clock);
        }

        [Fact]
        public void Can_count()
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
        public void Can_reset()
        {
            using (_timer.NewContext())
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
        public void Can_get_value()
        {
            using (_timer.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            var value = _timer.GetValue();

            value.Rate.Count.Should().NotBe(0);
            value.Histogram.Count.Should().NotBe(0);

            _timer.Value.Rate.Count.Should().NotBe(0);
            _timer.Value.Histogram.Count.Should().NotBe(0);
        }

        [Fact]
        public void Can_get_value_and_reset()
        {
            using (_timer.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            var value = _timer.GetValue(true);

            value.Rate.Count.Should().NotBe(0);
            value.Histogram.Count.Should().NotBe(0);

            _timer.Value.Rate.Count.Should().Be(0);
            _timer.Value.Histogram.Count.Should().Be(0);
        }

        [Fact]
        public void Can_track_time()
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
        public void Context_records_time_only_on_first_dispose()
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
        public void Context_reports_elapsed_time()
        {
            using (var context = _timer.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
                context.Elapsed.TotalMilliseconds.Should().Be(100);
            }
        }

        [Fact]
        public void Counts_even_when_action_throws()
        {
            Action action = () => _timer.Time(() => throw new InvalidOperationException());

            action.Should().Throw<InvalidOperationException>();

            _timer.Value.Rate.Count.Should().Be(1);
        }

        [Fact]
        public void Records_active_sessions()
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
        public void Records_user_value()
        {
            _timer.Record(1L, TimeUnit.Milliseconds, "A");
            _timer.Record(10L, TimeUnit.Milliseconds, "B");

            _timer.Value.Histogram.MinUserValue.Should().Be("A");
            _timer.Value.Histogram.MaxUserValue.Should().Be("B");
        }

        [Fact]
        public void Returns_empty_timer_if_not_timer_metric()
        {
            var timer = new CustomTimer();
            var value = timer.GetValueOrDefault();
            value.Should().NotBeNull();
        }

        [Fact]
        public void User_value_can_be_overwritten_after_context_creation()
        {
            using (var x = _timer.NewContext("a"))
            {
                x.TrackUserValue("b");
            }

            _timer.Value.Histogram.LastUserValue.Should().Be("b");
        }

        [Fact]
        public void User_value_can_be_set_after_context_creation()
        {
            using (var x = _timer.NewContext())
            {
                x.TrackUserValue("test");
            }

            _timer.GetValueOrDefault().Histogram.LastUserValue.Should().Be("test");
        }
    }
}
