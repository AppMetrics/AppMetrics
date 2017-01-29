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

        public void Decrement(string item) { }

        public void Decrement(string item, long value) { }

        public void Decrement(MetricItem item) { }

        public void Decrement(MetricItem item, long amount) { }

        public void Increment() { }

        public void Increment(long value) { }

        public void Increment(string item) { }

        public void Increment(string item, long value) { }

        public void Increment(MetricItem item) { }

        public void Increment(MetricItem item, long amount) { }

        public void Reset() { }
    }
}