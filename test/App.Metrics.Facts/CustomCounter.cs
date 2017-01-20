// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Interfaces;
using App.Metrics.Data;

namespace App.Metrics.Facts
{
    public class CustomCounter : ICounterMetric
    {
        /// <inheritdoc />
        public CounterValue Value => GetValue();

        /// <inheritdoc />
        public void Decrement() { }

        /// <inheritdoc />
        public void Decrement(string item) { }

        /// <inheritdoc />
        public void Decrement(MetricItem item) { }

        /// <inheritdoc />
        public void Decrement(long amount) { }

        /// <inheritdoc />
        public void Decrement(string item, long amount) { }

        /// <inheritdoc />
        public void Decrement(MetricItem item, long amount) { }

        /// <inheritdoc />
        public CounterValue GetValue(bool resetMetric = false) { return new CounterValue(1L); }

        /// <inheritdoc />
        public void Increment() { }

        /// <inheritdoc />
        public void Increment(string item) { }

        /// <inheritdoc />
        public void Increment(MetricItem item) { }

        /// <inheritdoc />
        public void Increment(long amount) { }

        /// <inheritdoc />
        public void Increment(string item, long amount) { }

        /// <inheritdoc />
        public void Increment(MetricItem item, long amount) { }

        /// <inheritdoc />
        public void Reset() { }
    }
}