// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Counter;
using App.Metrics.Tagging;

namespace App.Metrics.Facts
{
    public class CustomCounter : ICounterMetric
    {
        /// <inheritdoc />
        public CounterValue Value => GetValue();

        /// <inheritdoc />
        public void Decrement() { }

        /// <inheritdoc />
        public void Decrement(string setItem) { }

        /// <inheritdoc />
        public void Decrement(MetricSetItem setItem) { }

        /// <inheritdoc />
        public void Decrement(long amount) { }

        /// <inheritdoc />
        public void Decrement(string setItem, long amount) { }

        /// <inheritdoc />
        public void Decrement(MetricSetItem setItem, long amount) { }

        /// <inheritdoc />
        public CounterValue GetValue(bool resetMetric = false) { return new CounterValue(1L); }

        /// <inheritdoc />
        public void Increment() { }

        /// <inheritdoc />
        public void Increment(string setItem) { }

        /// <inheritdoc />
        public void Increment(MetricSetItem setItem) { }

        /// <inheritdoc />
        public void Increment(long amount) { }

        /// <inheritdoc />
        public void Increment(string setItem, long amount) { }

        /// <inheritdoc />
        public void Increment(MetricSetItem setItem, long amount) { }

        /// <inheritdoc />
        public void Reset() { }
    }
}