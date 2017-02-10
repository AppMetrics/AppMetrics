// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Histogram;

namespace App.Metrics.Facts
{
    public class CustomHistogram : IHistogramMetric
    {
        private bool _disposed = false;

        public CustomReservoir Reservoir { get; } = new CustomReservoir();

        public HistogramValue Value => new HistogramValue(Reservoir.Values.Last(), null, Reservoir.GetSnapshot());

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                    Reservoir?.Dispose();
                }
            }

            _disposed = true;
        }

        public HistogramValue GetValue(bool resetMetric = false) { return Value; }

        public void Reset() { Reservoir.Reset(); }

        public void Update(long value, string userValue) { Reservoir.Update(value, userValue); }

        public void Update(long value) { Reservoir.Update(value); }
    }
}