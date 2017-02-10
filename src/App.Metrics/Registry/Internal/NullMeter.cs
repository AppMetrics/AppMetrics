// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Internal;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Registry.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal struct NullMeter : IMeter
    {
        public void Mark() { }

        public void Mark(MetricSetItem setItem, long amount) { }

        public void Mark(long amount) { }

        public void Mark(string item) { }

        public void Mark(MetricSetItem setItem) { }

        public void Mark(string item, long amount) { }

        public void Reset() { }
    }
}