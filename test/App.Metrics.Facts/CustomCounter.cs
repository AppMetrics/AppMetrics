using App.Metrics.Core;
using App.Metrics.Data;

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
}