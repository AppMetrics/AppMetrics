// <copyright file="CustomHistogramMetric.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Histogram;

namespace App.Metrics.Facts.TestHelpers
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