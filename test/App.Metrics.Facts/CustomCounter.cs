using App.Metrics.Core.Interfaces;
using App.Metrics.Data;
using App.Metrics.Data.Interfaces;

namespace App.Metrics.Facts
{
    public class CustomCounter : ICounterMetric
    {
        public CounterValue Value => new CounterValue(10L, new CounterValue.SetItem[0]);

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

        public void Decrement(MetricItem item)
        {
        }

        public void Decrement(MetricItem item, long amount)
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

        public void Increment(MetricItem item)
        {
        }

        public void Increment(MetricItem item, long amount)
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
}