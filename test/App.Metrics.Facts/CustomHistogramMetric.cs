// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Histogram;

namespace App.Metrics.Facts
{
    public class CustomHistogramMetric : IHistogramMetric
    {
        private bool _disposed;

        public CustomReservoir Reservoir { get; } = new CustomReservoir();

        public HistogramValue Value => new HistogramValue(Reservoir.Values.Last(), null, Reservoir.GetSnapshot());

        public void Dispose()
        {
            if (!_disposed)
            {
                Reservoir?.Dispose();
            }

            _disposed = true;
        }

        public HistogramValue GetValue(bool resetMetric = false) { return Value; }

        public void Reset() { Reservoir.Reset(); }

        public void Update(long value, string userValue) { Reservoir.Update(value, userValue); }

        public void Update(long value) { Reservoir.Update(value); }
    }
}