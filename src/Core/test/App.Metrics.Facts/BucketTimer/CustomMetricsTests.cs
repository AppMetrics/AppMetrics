// <copyright file="CustomMetricsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.BucketTimer;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.FactsCommon.Fixtures;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.BucketTimer
{
    public class CustomMetricsTests : IDisposable
    {
        private readonly MetricsFixture _fixture;

        public CustomMetricsTests()
        {
            // DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new MetricsFixture();
        }

        [Fact]
        public void Can_register_timer_with_custom_histogram()
        {
            var histogram = new CustomBucketHistogramMetric();
            var timerOptions = new BucketTimerOptions
                               {
                                   Name = "custom",
                                   MeasurementUnit = Unit.Calls,
                                   DurationUnit = TimeUnit.Nanoseconds
                               };

            var timer = _fixture.Metrics.Provider.BucketTimer.WithHistogram(timerOptions, () => histogram);

            timer.Record(10L, TimeUnit.Nanoseconds);

            histogram.Size.Should().Be(1);
            histogram.Values.Single().Should().Be(10L);
        }

        public void Dispose() { _fixture?.Dispose(); }
    }
}