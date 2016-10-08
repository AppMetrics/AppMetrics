using System;
using App.Metrics.MetricData;
using App.Metrics.Sampling;

namespace App.Metrics.Core
{
    public sealed class DefaultMetricsBuilder : IMetricsBuilder
    {
        public CounterImplementation BuildCounter(string name, Unit unit)
        {
            return new CounterMetric();
        }

        public IMetricValueProvider<double> BuildGauge(string name, Unit unit, Func<double> valueProvider)
        {
            return new FunctionGauge(valueProvider);
        }

        public IHistogramImplementation BuildHistogram(string name, Unit unit, SamplingType samplingType)
        {
            return new HistogramMetric(samplingType);
        }

        public IHistogramImplementation BuildHistogram(string name, Unit unit, IReservoir reservoir)
        {
            return new HistogramMetric(reservoir);
        }

        public IMeterImplementation BuildMeter(string name, Unit unit, TimeUnit rateUnit)
        {
            return new MeterMetric();
        }

        public ITimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, SamplingType samplingType)
        {
            return new TimerMetric(samplingType);
        }

        public ITimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IHistogramImplementation histogram)
        {
            return new TimerMetric(histogram);
        }

        public ITimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IReservoir reservoir)
        {
            return new TimerMetric(reservoir);
        }
    }
}