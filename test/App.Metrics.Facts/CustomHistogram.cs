using System;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Data;

namespace App.Metrics.Facts
{
    public class CustomHistogram : IHistogramMetric
    {
        private bool _disposed = false;

        ~CustomHistogram()
        {
            Dispose(false);
        }

        public CustomReservoir Reservoir { get; } = new CustomReservoir();

        public HistogramValue Value => new HistogramValue(Reservoir.Values.Last(), null, Reservoir.GetSnapshot());

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                }
            }

            _disposed = true;
        }

        public HistogramValue GetValue(bool resetMetric = false)
        {
            return Value;
        }

        public void Reset()
        {
            Reservoir.Reset();
        }

        public void Update(long value, string userValue)
        {
            Reservoir.Update(value, userValue);
        }
    }
}