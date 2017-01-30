// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Core.Internal;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling.Uniform;
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
            var reservoir = new Lazy<IReservoir>(() => new DefaultAlgorithmRReservoir(1028));

            _timer = new DefaultTimerMetric(
                new DefaultHistogramMetric(reservoir),
                new DefaultMeterMetric(_clock, new TestTaskScheduler(_clock)),
                _clock);
        }

        [Fact]
        public void can_count()
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
        public void can_reset()
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
        public void can_track_time()
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
        public void context_records_time_only_on_first_dispose()
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
        public void context_reports_elapsed_time()
        {
            using (var context = _timer.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
                context.Elapsed.TotalMilliseconds.Should().Be(100);
            }
        }

        [Fact]
        public void counts_even_when_action_throws()
        {
            Action action = () => _timer.Time(() => { throw new InvalidOperationException(); });

            action.ShouldThrow<InvalidOperationException>();

            _timer.Value.Rate.Count.Should().Be(1);
        }

        [Fact]
        public void records_active_sessions()
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
        public void records_user_value()
        {
            _timer.Record(1L, TimeUnit.Milliseconds, "A");
            _timer.Record(10L, TimeUnit.Milliseconds, "B");

            _timer.Value.Histogram.MinUserValue.Should().Be("A");
            _timer.Value.Histogram.MaxUserValue.Should().Be("B");
        }

        [Fact]
        public void user_value_can_be_overwritten_after_context_creation()
        {
            using (var x = _timer.NewContext("a"))
            {
                x.TrackUserValue("b");
            }

            _timer.Value.Histogram.LastUserValue.Should().Be("b");
        }

        [Fact]
        public void user_value_can_be_set_after_context_creation()
        {
            using (var x = _timer.NewContext())
            {
                x.TrackUserValue("test");
            }

            _timer.GetValueOrDefault().Histogram.LastUserValue.Should().Be("test");
        }
    }
}