using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class DefaultContextCustomMetricsTests
    {
        private readonly MetricsContext context = new DefaultMetricsContext();

        public class CustomCounter : CounterImplementation
        {
            public void Increment() { }
            public void Increment(long value) { }
            public void Decrement() { }
            public void Decrement(long value) { }

            public void Increment(string item) { }
            public void Increment(string item, long value) { }
            public void Decrement(string item) { }
            public void Decrement(string item, long value) { }

            public void Reset() { }

            public CounterValue GetValue(bool resetMetric = false)
            {
                return this.Value;
            }
            public CounterValue Value
            {
                get { return new CounterValue(10L, new CounterValue.SetItem[0]); }
            }

            public bool Merge(MetricValueProvider<CounterValue> other)
            {
                return true;
            }
        }

        [Fact]
        public void MetricsContext_CanRegisterCustomCounter()
        {
            var counter = context.Advanced.Counter("custom", Unit.Calls, () => new CustomCounter());
            counter.Should().BeOfType<CustomCounter>();
            counter.Increment();
            context.DataProvider.CurrentMetricsData.Counters.Single().Value.Count.Should().Be(10L);
        }

        public class CustomReservoir : Reservoir
        {
            private readonly List<long> values = new List<long>();

            public long Count { get { return this.values.Count; } }
            public int Size { get { return this.values.Count; } }

            public void Update(long value, string userValue) { this.values.Add(value); }

            public Snapshot GetSnapshot(bool resetReservoir = false)
            {
                return new UniformSnapshot(this.values.Count, this.values);
            }

            public void Reset()
            {
                this.values.Clear();
            }
            public IEnumerable<long> Values { get { return this.values; } }
        }

        [Fact]
        public void MetricsContext_CanRegisterTimerWithCustomReservoir()
        {
            var reservoir = new CustomReservoir();
            var timer = context.Advanced.Timer("custom", Unit.Calls, () => (Reservoir)reservoir);

            timer.Record(10L, TimeUnit.Nanoseconds);

            reservoir.Size.Should().Be(1);
            reservoir.Values.Single().Should().Be(10L);
        }

        public class CustomHistogram : HistogramImplementation
        {
            private readonly CustomReservoir reservoir = new CustomReservoir();
            public void Update(long value, string userValue) { this.reservoir.Update(value, userValue); }
            public void Reset() { this.reservoir.Reset(); }

            public CustomReservoir Reservoir { get { return this.reservoir; } }

            public HistogramValue GetValue(bool resetMetric = false)
            {
                return this.Value;
            }

            public HistogramValue Value
            {
                get
                {
                    return new HistogramValue(this.reservoir.Values.Last(), null, this.reservoir.GetSnapshot());
                }
            }
        }

        [Fact]
        public void MetricsContext_CanRegisterTimerWithCustomHistogram()
        {
            var histogram = new CustomHistogram();

            var timer = context.Advanced.Timer("custom", Unit.Calls, () => (HistogramImplementation)histogram);

            timer.Record(10L, TimeUnit.Nanoseconds);

            histogram.Reservoir.Size.Should().Be(1);
            histogram.Reservoir.Values.Single().Should().Be(10L);
        }
    }
}
