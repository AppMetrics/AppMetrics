using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class DefaultContextCustomMetricsTests
    {
        private readonly IMetricsContext context = new DefaultMetricsContext(Clock.Default);

        [Fact]
        public void MetricsContext_CanRegisterCustomCounter()
        {
            var counter = context.Advanced.Counter("custom", Unit.Calls, () => new CustomCounter());
            counter.Should().BeOfType<CustomCounter>();
            counter.Increment();
            context.DataProvider.CurrentMetricsData.Counters.Single().Value.Count.Should().Be(10L);
        }

        [Fact]
        public void MetricsContext_CanRegisterTimerWithCustomHistogram()
        {
            var histogram = new CustomHistogram();

            var timer = context.Advanced.Timer("custom", Unit.Calls, () => (IHistogramImplementation)histogram);

            timer.Record(10L, TimeUnit.Nanoseconds);

            histogram.Reservoir.Size.Should().Be(1);
            histogram.Reservoir.Values.Single().Should().Be(10L);
        }

        [Fact]
        public void MetricsContext_CanRegisterTimerWithCustomReservoir()
        {
            var reservoir = new CustomReservoir();
            var timer = context.Advanced.Timer("custom", Unit.Calls, () => (IReservoir)reservoir);

            timer.Record(10L, TimeUnit.Nanoseconds);

            reservoir.Size.Should().Be(1);
            reservoir.Values.Single().Should().Be(10L);
        }

        public class CustomCounter : ICounterImplementation
        {
            public CounterValue Value
            {
                get { return new CounterValue(10L, new CounterValue.SetItem[0]); }
            }

            public void Decrement()
            {
            }

            public void Decrement(long value)
            {
            }

            public void Decrement(string item)
            {
            }

            public void Decrement(string item, long value)
            {
            }

            public CounterValue GetValue(bool resetMetric = false)
            {
                return Value;
            }

            public void Increment()
            {
            }

            public void Increment(long value)
            {
            }

            public void Increment(string item)
            {
            }

            public void Increment(string item, long value)
            {
            }

            public bool Merge(IMetricValueProvider<CounterValue> other)
            {
                return true;
            }

            public void Reset()
            {
            }
        }

        public class CustomHistogram : IHistogramImplementation
        {
            public CustomReservoir Reservoir { get; } = new CustomReservoir();

            public HistogramValue Value
            {
                get { return new HistogramValue(Reservoir.Values.Last(), null, Reservoir.GetSnapshot()); }
            }

            public HistogramValue GetValue(bool resetMetric = false)
            {
                return Value;
            }

            public void Reset()
            {
                Reservoir.Reset();
            }

            public void Update(long value, string userValue)
            {
                Reservoir.Update(value, userValue);
            }
        }

        public class CustomReservoir : IReservoir
        {
            private readonly List<long> _values = new List<long>();

            public long Count
            {
                get { return _values.Count; }
            }

            public int Size
            {
                get { return _values.Count; }
            }

            public IEnumerable<long> Values
            {
                get { return _values; }
            }

            public ISnapshot GetSnapshot(bool resetReservoir = false)
            {
                return new UniformSnapshot(_values.Count, _values);
            }

            public void Reset()
            {
                _values.Clear();
            }

            public void Update(long value, string userValue)
            {
                _values.Add(value);
            }
        }
    }
}