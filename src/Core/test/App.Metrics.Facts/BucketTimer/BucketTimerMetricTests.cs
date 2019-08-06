// <copyright file="TimerMetricTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.FactsCommon;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.BucketTimer
{
    public class BucketTimerMetricTests
    {
        private readonly IClock _clock = new TestClock();
        private readonly DefaultBucketTimerMetric _timer;

        public BucketTimerMetricTests()
        {
            _timer = new DefaultBucketTimerMetric(
                new DefaultBucketHistogramMetric(new[] { 100000000d, 300000000 }),
                new DefaultMeterMetric(_clock),
                _clock,
                TimeUnit.Nanoseconds);
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
        public void Can_track_time()
        {
            using (_timer.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            _timer.Value.Histogram.Count.Should().Be(1);
            _timer.Value.Histogram.Buckets[100000000].Should().Be(1);

            using (_timer.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 300);
            }

            _timer.Value.Histogram.Count.Should().Be(2);
            _timer.Value.Histogram.Buckets[100000000].Should().Be(1);
            _timer.Value.Histogram.Buckets[300000000].Should().Be(1);
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
            _timer.Value.Histogram.Buckets[100000000].Should().Be(1);
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
        public void Returns_empty_timer_if_not_timer_metric()
        {
            var timer = new CustomTimer();
            var value = timer.GetValueOrDefault();
            value.Should().NotBeNull();
        }
    }
}