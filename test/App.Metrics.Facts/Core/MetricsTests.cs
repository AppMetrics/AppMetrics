// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class MetricsTests : IDisposable
    {
        private readonly MetricsFixture _fixture;

        public MetricsTests()
        {
            //DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new MetricsFixture();
        }

        [Fact]
        public void can_clear_metrics_at_runtime()
        {
            var counterOptions = new CounterOptions
                                 {
                                     Name = "request counter",
                                     MeasurementUnit = Unit.Requests,
                                 };

            _fixture.Metrics.Measure.Counter.Increment(counterOptions);

            var data = _fixture.CurrentData(_fixture.Metrics);
            var counterValue = data.Contexts.Single().Counters.Single();
            counterValue.Value.Count.Should().Be(1);

            _fixture.Metrics.Manage.Reset();

            data = _fixture.CurrentData(_fixture.Metrics);
            data.Contexts.Should().BeNullOrEmpty();
        }

        [Fact]
        public void can_disable_metrics_at_runtime()
        {
            var counterOptions = new CounterOptions
                                 {
                                     Name = "request counter",
                                     MeasurementUnit = Unit.Requests,
                                 };

            _fixture.Metrics.Measure.Counter.Increment(counterOptions);

            var data = _fixture.CurrentData(_fixture.Metrics);
            var counterValue = data.Contexts.Single().Counters.Single();
            counterValue.Value.Count.Should().Be(1);

            _fixture.Metrics.Manage.Disable();

            data = _fixture.CurrentData(_fixture.Metrics);
            data.Contexts.Should().BeNullOrEmpty();
        }


        [Fact]
        public void can_record_metric_in_new_context()
        {
            var counterOptions = new CounterOptions
                                 {
                                     Name = "counter",
                                     Context = "test",
                                     MeasurementUnit = Unit.Requests,
                                 };

            _fixture.Metrics.Measure.Counter.Increment(counterOptions);

            var data = _fixture.CurrentData(_fixture.Metrics);

            data.Contexts.Should().Contain(g => g.Context == "test");

            var counterValue = data.Contexts.First(g => g.Context == "test").Counters.Single();

            counterValue.Name.Should().Be("counter");
        }

        [Fact]
        public void can_shutdown_metric_contexts()
        {
            var context = "test";
            var counterOptions = new CounterOptions
                                 {
                                     Name = "test",
                                     Context = context,
                                     MeasurementUnit = Unit.Bytes
                                 };

            _fixture.Metrics.Measure.Counter.Increment(counterOptions);

            var data = _fixture.CurrentData(_fixture.Metrics);

            data.Contexts.First(g => g.Context == context).Counters.Single().Name.Should().Be("test");

            _fixture.Metrics.Manage.ShutdownContext(context);

            data = _fixture.CurrentData(_fixture.Metrics);

            data.Contexts.FirstOrDefault(g => g.Context == context).Should().BeNull("because the context was shutdown");
        }

        [Fact]
        public void child_with_same_name_are_same_context()
        {
            var counterOptions = new CounterOptions
                                 {
                                     Name = "test",
                                     Context = "test"
                                 };

            var first = _fixture.Metrics.Provider.Counter.Instance(counterOptions);
            var second = _fixture.Metrics.Provider.Counter.Instance(counterOptions);

            ReferenceEquals(first, second).Should().BeTrue();
        }

        [Fact]
        public void data_provider_reflects_new_metrics()
        {
            var counterOptions = new CounterOptions
                                 {
                                     Name = "bytes-counter",
                                     MeasurementUnit = Unit.Bytes,
                                 };

            _fixture.Metrics.Measure.Counter.Increment(counterOptions);

            var data = _fixture.CurrentData(_fixture.Metrics);
            var context = data.Contexts.Single();

            context.Counters.Should().HaveCount(1);
            context.Counters.Single().Name.Should().Be("bytes-counter");
            context.Counters.Single().Value.Count.Should().Be(1L);
        }

        public void Dispose() { Dispose(true); }

        [Fact]
        public void does_not_throw_on_metrics_of_different_type_with_same_name()
        {
            ((Action)(() =>
            {
                var name = "Test";

                var apdexOptions = new ApdexOptions
                                   {
                                       Name = name,
                                       MeasurementUnit = Unit.Calls,
                                   };

                var counterOptions = new CounterOptions
                                     {
                                         Name = name,
                                         MeasurementUnit = Unit.Calls,
                                     };

                var meterOptions = new MeterOptions
                                   {
                                       Name = name,
                                       MeasurementUnit = Unit.Calls
                                   };

                var gaugeOptions = new GaugeOptions
                                   {
                                       Name = name,
                                       MeasurementUnit = Unit.Calls
                                   };

                var histogramOptions = new HistogramOptions
                                       {
                                           Name = name,
                                           MeasurementUnit = Unit.Calls
                                       };

                var timerOptions = new TimerOptions
                                   {
                                       Name = name,
                                       MeasurementUnit = Unit.Calls
                                   };

                _fixture.Metrics.Measure.Apdex.Track(apdexOptions);
                _fixture.Metrics.Measure.Gauge.SetValue(gaugeOptions, () => 0.0);
                _fixture.Metrics.Measure.Counter.Increment(counterOptions);
                _fixture.Metrics.Measure.Meter.Mark(meterOptions);
                _fixture.Metrics.Measure.Histogram.Update(histogramOptions, 1L);
                _fixture.Metrics.Measure.Timer.Time(timerOptions);
            })).ShouldNotThrow();
        }

        [Fact]
        public void metrics_added_are_visible_in_the_data_provider()
        {
            var context = "test";
            var counterOptions = new CounterOptions
                                 {
                                     Name = "test_counter",
                                     Context = context,
                                     MeasurementUnit = Unit.Bytes
                                 };
            var dataProvider = _fixture.Metrics.Snapshot;

            var data = dataProvider.Get();

            data.Contexts.FirstOrDefault(g => g.Context == context).Should().BeNull("the context hasn't been added yet");

            _fixture.Metrics.Measure.Counter.Increment(counterOptions);

            data = dataProvider.Get();
            data.Contexts.First(g => g.Context == context).Counters.Should().HaveCount(1);
        }

        [Fact]
        public void metrics_are_present_in_metrics_data()
        {
            var counterOptions = new CounterOptions
                                 {
                                     Name = "request counter",
                                     MeasurementUnit = Unit.Requests,
                                 };

            _fixture.Metrics.Measure.Counter.Increment(counterOptions);

            var data = _fixture.CurrentData(_fixture.Metrics);

            var counterValue = data.Contexts.Single().Counters.Single();

            counterValue.Name.Should().Be("request counter");
            counterValue.Unit.Should().Be(Unit.Requests);
            counterValue.Value.Count.Should().Be(1);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fixture?.Dispose();
            }
        }
    }
}