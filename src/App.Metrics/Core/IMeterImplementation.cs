using System;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public interface IMeterImplementation : IMeter, IMetricValueProvider<MeterValue>, IDisposable
    {
    }
}