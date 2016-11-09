using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class MetricsContextsTests : IDisposable
    {
        private readonly DefaultMetricsContextTestFixture _fixture;

        public MetricsContextsTests()
        {
            //DEVNOTE: Don't want Context to be shared between tests
            _fixture = new DefaultMetricsContextTestFixture();
        }

        [Fact]
        public async Task can_clear_metrics_at_runtime()
        {
            var counterOptions = new CounterOptions
            {
                Name = "request counter",
                MeasurementUnit = Unit.Requests,
            };
            var counter = _fixture.Context.Advanced.Counter(counterOptions);

            counter.Increment();

            var data = await _fixture.CurrentData(_fixture.Context);
            var counterValue = data.Contexts.Single().Counters.Single();
            counterValue.Value.Count.Should().Be(1);

            _fixture.Context.Advanced.DataManager.Reset();

            data = await _fixture.CurrentData(_fixture.Context);
            data.Contexts.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task can_disable_metrics_at_runtime()
        {
            var counterOptions = new CounterOptions
            {
                Name = "request counter",
                MeasurementUnit = Unit.Requests,
            };
            var counter = _fixture.Context.Advanced.Counter(counterOptions);

            counter.Increment();

            var data = await _fixture.CurrentData(_fixture.Context);
            var counterValue = data.Contexts.Single().Counters.Single();
            counterValue.Value.Count.Should().Be(1);

            _fixture.Context.Advanced.Disable();

            data = await _fixture.CurrentData(_fixture.Context);
            data.Contexts.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task can_filter_metrics_by_type()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.Requests,
            };


            var meterOptions = new MeterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
            };

            var counter = _fixture.Context.Advanced.Counter(counterOptions);
            var meter = _fixture.Context.Advanced.Meter(meterOptions);

            var filter = new DefaultMetricsFilter().WhereType(MetricType.Counter);

            counter.Increment();
            meter.Mark(1);

            var currentData = await _fixture.CurrentDataWithFilter(_fixture.Context, filter);
            var context = currentData.Contexts.Single();

            var counterValue = context.Counters.Single();
            var meterValue = context.Meters.FirstOrDefault();

            counterValue.Name.Should().Be("test");
            counterValue.Unit.Should().Be(Unit.Requests);
            counterValue.Value.Count.Should().Be(1);

            Assert.Null(meterValue);
        }

        [Fact]
        public async Task can_propergate_value_tags()
        {
            var tags = new MetricTags().With("tag", "value");
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = tags
            };

            var meterOptions = new MeterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = tags
            };

            var histogramOptions = new HistogramOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = tags
            };

            var timerOptions = new TimerOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = tags
            };

            _fixture.Context.Increment(counterOptions);
            _fixture.Context.Mark(meterOptions);
            _fixture.Context.Update(histogramOptions, 1);
            _fixture.Context.Time(timerOptions, () => { });

            var data = await _fixture.CurrentData(_fixture.Context);
            var context = data.Contexts.Single();

            context.Counters.Single().Tags.Should().Equals(tags);
            context.Meters.Single().Tags.Should().Equals("tag");
            context.Histograms.Single().Tags.Should().Equals("tag");
            context.Timers.Single().Tags.Should().Equals("tag");
        }

        [Fact]
        public async Task can_record_metric_in_new_context()
        {
            var counterOptions = new CounterOptions
            {
                Name = "counter",
                Context = "test",
                MeasurementUnit = Unit.Requests,
            };

            _fixture.Context.Increment(counterOptions);

            var data = await _fixture.CurrentData(_fixture.Context);

            data.Contexts.Should().Contain(g => g.Context == "test");

            var counterValue = data.Contexts.First(g => g.Context == "test").Counters.Single();

            counterValue.Name.Should().Be("counter");
        }

        [Fact]
        public async Task can_shutdown_metric_contexts()
        {
            var context = "test";
            var counterOptions = new CounterOptions
            {
                Name = "test",
                Context = context,
                MeasurementUnit = Unit.Bytes
            };

            _fixture.Context.Advanced.Counter(counterOptions).Increment();

            var data = await _fixture.CurrentData(_fixture.Context);

            data.Contexts.First(g => g.Context == context).Counters.Single().Name.Should().Be("test");

            _fixture.Context.Advanced.DataManager.ShutdownContext(context);

            data = await _fixture.CurrentData(_fixture.Context);

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

            var first = _fixture.Context.Advanced.Counter(counterOptions);
            var second = _fixture.Context.Advanced.Counter(counterOptions);

            ReferenceEquals(first, second).Should().BeTrue();
        }

        [Fact]
        public async Task data_provider_reflects_new_metrics()
        {
            var counterOptions = new CounterOptions
            {
                Name = "bytes-counter",
                MeasurementUnit = Unit.Bytes,
            };

            _fixture.Context.Advanced.Counter(counterOptions).Increment();

            var data = await _fixture.CurrentData(_fixture.Context);
            var context = data.Contexts.Single();

            context.Counters.Should().HaveCount(1);
            context.Counters.Single().Name.Should().Be("bytes-counter");
            context.Counters.Single().Value.Count.Should().Be(1L);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        [Fact]
        public void does_not_throw_on_metrics_of_different_type_with_same_name()
        {
            ((Action)(() =>
            {
                var name = "Test";

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

                _fixture.Context.Advanced.Gauge(gaugeOptions, () => 0.0);
                _fixture.Context.Advanced.Counter(counterOptions);
                _fixture.Context.Advanced.Meter(meterOptions);
                _fixture.Context.Advanced.Histogram(histogramOptions);
                _fixture.Context.Advanced.Timer(timerOptions);
            })).ShouldNotThrow();
        }

        [Fact]
        public async Task metrics_added_are_visible_in_the_data_provider()
        {
            var context = "test";
            var counterOptions = new CounterOptions
            {
                Name = "test_counter",
                Context = context,
                MeasurementUnit = Unit.Bytes,
            };
            var dataManager = _fixture.Context.Advanced.DataManager;

            var data = await dataManager.GetAsync();

            data.Contexts.FirstOrDefault(g => g.Context == context).Should().BeNull("the context hasn't been added yet");
            _fixture.Context.Advanced.Counter(counterOptions).Increment();

            data = await dataManager.GetAsync();
            data.Contexts.First(g => g.Context == context).Counters.Should().HaveCount(1);
        }

        [Fact]
        public async Task metrics_are_present_in_metrics_data()
        {
            var counterOptions = new CounterOptions
            {
                Name = "request counter",
                MeasurementUnit = Unit.Requests,
            };
            var counter = _fixture.Context.Advanced.Counter(counterOptions);

            counter.Increment();

            var data = await _fixture.CurrentData(_fixture.Context);

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