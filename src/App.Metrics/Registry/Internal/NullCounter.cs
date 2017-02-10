// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Internal;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Registry.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal struct NullCounter : ICounter
    {
        public void Decrement() { }

        public void Decrement(long value) { }

        public void Decrement(string setItem) { }

        public void Decrement(string setItem, long value) { }

        public void Decrement(MetricSetItem setItem) { }

        public void Decrement(MetricSetItem setItem, long amount) { }

        public void Increment() { }

        public void Increment(long value) { }

        public void Increment(string setItem) { }

        public void Increment(string setItem, long value) { }

        public void Increment(MetricSetItem setItem) { }

        public void Increment(MetricSetItem setItem, long amount) { }

        public void Reset() { }
    }
}