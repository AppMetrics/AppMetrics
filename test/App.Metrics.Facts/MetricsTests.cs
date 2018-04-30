// <copyright file="MetricsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.FactsCommon.Fixtures;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts
{
    public class MetricsTests : IDisposable
    {
        private readonly MetricsFixture _fixture;

        public MetricsTests()
        {
            // DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new MetricsFixture();
        }

        [Fact]
        public void Can_clear_metrics_at_runtime()
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
        public void Can_disable_metrics_at_runtime()
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
        public void Can_record_metric_in_new_context()
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
        public void Can_shutdown_metric_contexts()
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
        public void Child_with_same_name_are_same_context()
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
        public void Data_provider_reflects_new_metrics()
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
        public void Does_not_throw_on_metrics_of_different_type_with_same_name()
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
            })).Should().NotThrow();
        }

        [Fact]
        public void Metrics_added_are_visible_in_the_data_provider()
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
        public void Metrics_are_present_in_metrics_data()
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