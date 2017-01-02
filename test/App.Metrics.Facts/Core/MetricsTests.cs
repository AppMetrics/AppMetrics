using System;
using System.Linq;
using System.Threading.Tasks;
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
            var counter = _fixture.Metrics.Advanced.Counter(counterOptions);

            counter.Increment();

            var data = _fixture.CurrentData(_fixture.Metrics);
            var counterValue = data.Contexts.Single().Counters.Single();
            counterValue.Value.Count.Should().Be(1);

            _fixture.Metrics.Advanced.Data.Reset();

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
            var counter = _fixture.Metrics.Advanced.Counter(counterOptions);

            counter.Increment();

            var data = _fixture.CurrentData(_fixture.Metrics);
            var counterValue = data.Contexts.Single().Counters.Single();
            counterValue.Value.Count.Should().Be(1);

            _fixture.Metrics.Advanced.Disable();

            data = _fixture.CurrentData(_fixture.Metrics);
            data.Contexts.Should().BeNullOrEmpty();
        }

        [Fact]
        public void can_get_tags_from_set_item_string()
        {
            var expectedTags = new MetricTags().With("item", "machine-1").With("item", "machine-2");
           
            var tags = new MetricTags();
            tags = tags.FromSetItemString("item:machine-1|item:machine-2");

            tags.Should().Equal(expectedTags);
        }

        [Fact]
        public void can_get_tags_from_set_item_string_when_single_item()
        {
            var expectedTags = new MetricTags().With("item", "machine-1");

            var tags = new MetricTags();
            tags = tags.FromSetItemString("item:machine-1");

            tags.Should().Equal(expectedTags);
        }

        [Fact]
        public void can_get_tags_from_set_item_when_item_is_missing()
        {
            var expectedTags = MetricTags.None;

            var tags = new MetricTags();
            tags = tags.FromSetItemString(string.Empty);

            tags.Should().Equal(expectedTags);
        }

        [Fact]
        public void can_propergate_value_tags()
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

            _fixture.Metrics.Increment(counterOptions);
            _fixture.Metrics.Mark(meterOptions);
            _fixture.Metrics.Update(histogramOptions, 1);
            _fixture.Metrics.Time(timerOptions, () => { });

            var data = _fixture.CurrentData(_fixture.Metrics);
            var context = data.Contexts.Single();

            context.Counters.Single().Tags.Should().Equals(tags);
            context.Meters.Single().Tags.Should().Equals("tag");
            context.Histograms.Single().Tags.Should().Equals("tag");
            context.Timers.Single().Tags.Should().Equals("tag");
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

            _fixture.Metrics.Increment(counterOptions);

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

            _fixture.Metrics.Advanced.Counter(counterOptions).Increment();

            var data = _fixture.CurrentData(_fixture.Metrics);

            data.Contexts.First(g => g.Context == context).Counters.Single().Name.Should().Be("test");

            _fixture.Metrics.Advanced.Data.ShutdownContext(context);

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

            var first = _fixture.Metrics.Advanced.Counter(counterOptions);
            var second = _fixture.Metrics.Advanced.Counter(counterOptions);

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

            _fixture.Metrics.Advanced.Counter(counterOptions).Increment();

            var data = _fixture.CurrentData(_fixture.Metrics);
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

                _fixture.Metrics.Gauge(gaugeOptions, () => 0.0);
                _fixture.Metrics.Advanced.Counter(counterOptions);
                _fixture.Metrics.Advanced.Meter(meterOptions);
                _fixture.Metrics.Advanced.Histogram(histogramOptions);
                _fixture.Metrics.Advanced.Timer(timerOptions);
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
                MeasurementUnit = Unit.Bytes,
            };
            var dataProvider = _fixture.Metrics.Advanced.Data;

            var data = dataProvider.ReadData();

            data.Contexts.FirstOrDefault(g => g.Context == context).Should().BeNull("the context hasn't been added yet");
            _fixture.Metrics.Advanced.Counter(counterOptions).Increment();

            data = dataProvider.ReadData();
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
            var counter = _fixture.Metrics.Advanced.Counter(counterOptions);

            counter.Increment();

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