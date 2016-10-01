using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using App.Metrics.App_Packages.Concurrency;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public interface CounterImplementation : Counter, MetricValueProvider<CounterValue>
    {
    }

    public sealed class CounterMetric : CounterImplementation
    {
        private readonly StripedLongAdder counter = new StripedLongAdder();
        private ConcurrentDictionary<string, StripedLongAdder> setCounters;

        public CounterValue Value
        {
            get
            {
                if (this.setCounters == null || this.setCounters.Count == 0)
                {
                    return new CounterValue(this.counter.GetValue());
                }
                return GetValueWithSetItems();
            }
        }

        public void Decrement()
        {
            this.counter.Decrement();
        }

        public void Decrement(long value)
        {
            this.counter.Add(-value);
        }

        public void Decrement(string item)
        {
            Decrement();
            SetCounter(item).Decrement();
        }

        public void Decrement(string item, long amount)
        {
            Decrement(amount);
            SetCounter(item).Add(-amount);
        }

        public CounterValue GetValue(bool resetMetric = false)
        {
            var value = this.Value;
            if (resetMetric)
            {
                this.Reset();
            }
            return value;
        }

        public void Increment()
        {
            this.counter.Increment();
        }

        public void Increment(long value)
        {
            this.counter.Add(value);
        }

        public void Increment(string item)
        {
            Increment();
            SetCounter(item).Increment();
        }

        public void Increment(string item, long amount)
        {
            Increment(amount);
            SetCounter(item).Add(amount);
        }

        public void Reset()
        {
            this.counter.Reset();
            if (this.setCounters != null)
            {
                foreach (var item in this.setCounters)
                {
                    item.Value.Reset();
                }
            }
        }

        private CounterValue GetValueWithSetItems()
        {
            Debug.Assert(this.setCounters != null);
            var total = this.counter.GetValue();

            var items = new CounterValue.SetItem[this.setCounters.Count];
            var index = 0;
            foreach (var item in this.setCounters)
            {
                var itemValue = item.Value.GetValue();

                var percent = total > 0 ? itemValue / (double)total * 100 : 0.0;
                var setCounter = new CounterValue.SetItem(item.Key, itemValue, percent);
                items[index++] = setCounter;
                if (index == items.Length)
                {
                    break;
                }
            }

            Array.Sort(items, CounterValue.SetItemComparer);

            return new CounterValue(total, items);
        }

        private StripedLongAdder SetCounter(string item)
        {
            if (this.setCounters == null)
            {
                Interlocked.CompareExchange(ref this.setCounters, new ConcurrentDictionary<string, StripedLongAdder>(), null);
            }
            Debug.Assert(this.setCounters != null);
            return this.setCounters.GetOrAdd(item, v => new StripedLongAdder());
        }
    }
}