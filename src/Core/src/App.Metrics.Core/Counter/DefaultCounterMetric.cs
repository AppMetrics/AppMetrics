// <copyright file="DefaultCounterMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using App.Metrics.Concurrency;

namespace App.Metrics.Counter
{
    public sealed class DefaultCounterMetric : ICounterMetric
    {
        private readonly StripedLongAdder _counter = new StripedLongAdder();
        private ConcurrentDictionary<string, StripedLongAdder> _setCounters;

        public CounterValue Value
        {
            get
            {
                if (_setCounters == null || _setCounters.Count == 0)
                {
                    return new CounterValue(_counter.GetValue());
                }

                return GetValueWithSetItems();
            }
        }

        /// <inheritdoc />
        public void Decrement() { _counter.Decrement(); }

        /// <inheritdoc />
        public void Decrement(long value) { _counter.Add(-value); }

        /// <inheritdoc />
        public void Decrement(string setItem)
        {
            Decrement();
            SetCounter(setItem).Decrement();
        }

        /// <inheritdoc />
        public void Decrement(string setItem, long amount)
        {
            Decrement(amount);
            SetCounter(setItem).Add(-amount);
        }

        /// <inheritdoc />
        public void Decrement(MetricSetItem setItem) { Decrement(setItem.ToString()); }

        /// <inheritdoc />
        public void Decrement(MetricSetItem setItem, long amount) { Decrement(setItem.ToString(), amount); }

        /// <inheritdoc />
        public CounterValue GetValue(bool resetMetric = false)
        {
            var value = Value;

            if (resetMetric)
            {
                Reset();
            }

            return value;
        }

        /// <inheritdoc />
        public void Increment() { _counter.Increment(); }

        /// <inheritdoc />
        public void Increment(long value) { _counter.Add(value); }

        /// <inheritdoc />
        public void Increment(string setItem)
        {
            Increment();
            SetCounter(setItem).Increment();
        }

        /// <inheritdoc />
        public void Increment(string setItem, long amount)
        {
            Increment(amount);
            SetCounter(setItem).Add(amount);
        }

        /// <inheritdoc />
        public void Increment(MetricSetItem setItem) { Increment(setItem.ToString()); }

        /// <inheritdoc />
        public void Increment(MetricSetItem setItem, long amount) { Increment(setItem.ToString(), amount); }

        /// <inheritdoc />
        public void Reset()
        {
            _counter.Reset();

            if (_setCounters != null)
            {
                foreach (var item in _setCounters)
                {
                    item.Value.Reset();
                }
            }
        }

        private CounterValue GetValueWithSetItems()
        {
            Debug.Assert(_setCounters != null, "set counters not null");

            var total = _counter.GetValue();

            var items = new CounterValue.SetItem[_setCounters.Count];
            var index = 0;

            foreach (var item in _setCounters)
            {
                var itemValue = item.Value.GetValue();

                double percent = total > 0 ? itemValue / (double)total * 100 : 0.0;
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
            if (_setCounters == null)
            {
                Interlocked.CompareExchange(ref _setCounters, new ConcurrentDictionary<string, StripedLongAdder>(), null);
            }

            Debug.Assert(_setCounters != null, "set counters not null");

            return _setCounters.GetOrAdd(item, v => new StripedLongAdder());
        }
    }
}