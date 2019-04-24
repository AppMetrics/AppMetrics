// <copyright file="CustomCounter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Counter;

namespace App.Metrics.Facts.TestHelpers
{
    public class CustomCounter : ICounterMetric
    {
        /// <inheritdoc />
        public CounterValue Value => GetValue();

        /// <inheritdoc />
        public void Decrement()
        {
        }

        /// <inheritdoc />
        public void Decrement(string setItem)
        {
        }

        /// <inheritdoc />
        public void Decrement(MetricSetItem setItem)
        {
        }

        /// <inheritdoc />
        public void Decrement(long amount)
        {
        }

        /// <inheritdoc />
        public void Decrement(string setItem, long amount)
        {
        }

        /// <inheritdoc />
        public void Decrement(MetricSetItem setItem, long amount)
        {
        }

        /// <inheritdoc />
        public CounterValue GetValue(bool resetMetric = false)
        {
            return new CounterValue(1L);
        }

        /// <inheritdoc />
        public void Increment()
        {
        }

        /// <inheritdoc />
        public void Increment(string setItem)
        {
        }

        /// <inheritdoc />
        public void Increment(MetricSetItem setItem)
        {
        }

        /// <inheritdoc />
        public void Increment(long amount)
        {
        }

        /// <inheritdoc />
        public void Increment(string setItem, long amount)
        {
        }

        /// <inheritdoc />
        public void Increment(MetricSetItem setItem, long amount)
        {
        }

        /// <inheritdoc />
        public void Reset()
        {
        }
    }
}