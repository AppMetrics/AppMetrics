// <copyright file="NullCounter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using App.Metrics.Counter;

namespace App.Metrics.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public struct NullCounter : ICounter
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