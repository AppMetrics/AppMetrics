using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Sampling;
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
        public async Task can_register_custom_counter()
        {
            var counterOptions = new CounterOptions
            {
                Name = "Custom Counter",
                MeasurementUnit = Unit.Calls
            };
            var counter = _fixture.Metrics.Advanced.Counter(counterOptions, () => new CustomCounter());
            counter.Should().BeOfType<CustomCounter>();
            counter.Increment();

            var data = await _fixture.Metrics.Advanced.Data.ReadDataAsync();
            var context = data.Contexts.Single();

            context.Counters.Single().Value.Count.Should().Be(10L);
        }

        [Fact]
        public void can_register_timer_with_custom_histogram()
        {
            var histogram = new CustomHistogram();
            var timerOptions = new TimerOptions
            {
                Name = "custom",
                MeasurementUnit = Unit.Calls
            };

            var timer = _fixture.Metrics.Advanced.Timer(timerOptions, () => (IHistogramMetric)histogram);

            timer.Record(10L, TimeUnit.Nanoseconds);

            histogram.Reservoir.Size.Should().Be(1);
            histogram.Reservoir.Values.Single().Should().Be(10L);
        }

        [Fact]
        public void can_register_timer_with_custom_reservoir()
        {
            var reservoir = new CustomReservoir();
            var timerOptions = new TimerOptions
            {
                Name = "custom",
                MeasurementUnit = Unit.Calls,
                WithReservoir = () => reservoir as IReservoir
            };
            var timer = _fixture.Metrics.Advanced.Timer(timerOptions);

            timer.Record(10L, TimeUnit.Nanoseconds);

            reservoir.Size.Should().Be(1);
            reservoir.Values.Single().Should().Be(10L);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fixture?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}