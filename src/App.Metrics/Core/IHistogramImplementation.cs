using System;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public interface IHistogramImplementation : IHistogram, IMetricValueProvider<HistogramValue>, IDisposable
    {
    }
}