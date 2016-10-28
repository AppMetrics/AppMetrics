using System;

namespace App.Metrics.MetricData
{
    public sealed class ScaledValueProvider<T> : IMetricValueProvider<T>
    {
        private readonly Func<T, T> _scalingFunction;

        public ScaledValueProvider(IMetricValueProvider<T> valueProvider, Func<T, T> transformation)
        {
            ValueProvider = valueProvider;
            _scalingFunction = transformation;
        }

        public T Value => _scalingFunction(ValueProvider.Value);

        public IMetricValueProvider<T> ValueProvider { get; }

        public T GetValue(bool resetMetric = false)
        {
            return _scalingFunction(ValueProvider.GetValue(resetMetric));
        }
    }
}