// <copyright file="CustomMetricsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.FactsCommon.Fixtures;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Timer
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
            var histogram = new CustomHistogramMetric();
            var timerOptions = new TimerOptions
                               {
                                   Name = "custom",
                                   MeasurementUnit = Unit.Calls
                               };

            var timer = _fixture.Metrics.Provider.Timer.WithHistogram(timerOptions, () => histogram);

            timer.Record(10L, TimeUnit.Nanoseconds);

            histogram.Reservoir.Size.Should().Be(1);
            histogram.Reservoir.Values.Single().Should().Be(10L);
        }

        [Fact]
        public void Can_register_timer_with_custom_reservoir()
        {
            var timerOptions = new TimerOptions
                               {
                                   Name = "custom",
                                   MeasurementUnit = Unit.Calls,
                                   Reservoir = () => new CustomReservoir()
                               };
            var timer = _fixture.Metrics.Provider.Timer.Instance(timerOptions);

            timer.Record(10L, TimeUnit.Nanoseconds);

            var snapshot = _fixture.Metrics.Snapshot.Get();

            snapshot.Contexts.First().Timers.First().Value.Histogram.Count.Should().Be(1);
        }

        public void Dispose() { _fixture?.Dispose(); }
    }
}