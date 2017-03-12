// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class CustomMetricsTests : IDisposable
    {
        private readonly MetricsFixture _fixture;

        public CustomMetricsTests()
        {
            //DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new MetricsFixture();
        }

        [Fact]
        public void can_register_timer_with_custom_histogram()
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
        public void can_register_timer_with_custom_reservoir()
        {
            var reservoir = new Lazy<IReservoir>(() => new CustomReservoir());
            var timerOptions = new TimerOptions
                               {
                                   Name = "custom",
                                   MeasurementUnit = Unit.Calls,
                                   Reservoir = reservoir
                               };
            var timer = _fixture.Metrics.Provider.Timer.Instance(timerOptions);

            timer.Record(10L, TimeUnit.Nanoseconds);

            var snapshot = reservoir.Value.GetSnapshot();

            snapshot.Size.Should().Be(1);
            snapshot.Values.Single().Should().Be(10L);
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }        
    }
}