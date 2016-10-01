using System;
using App.Metrics.MetricData;
using App.Metrics.Sampling;

namespace App.Metrics.Core
{
    public sealed class DefaultMetricsBuilder : MetricsBuilder
    {
        public CounterImplementation BuildCounter(string name, Unit unit)
        {
            return new CounterMetric();
        }

        public MetricValueProvider<double> BuildGauge(string name, Unit unit, Func<double> valueProvider)
        {
            return new FunctionGauge(valueProvider);
        }

        public HistogramImplementation BuildHistogram(string name, Unit unit, SamplingType samplingType)
        {
            return new HistogramMetric(samplingType);
        }

        public HistogramImplementation BuildHistogram(string name, Unit unit, Reservoir reservoir)
        {
            return new HistogramMetric(reservoir);
        }

        public MeterImplementation BuildMeter(string name, Unit unit, TimeUnit rateUnit)
        {
            return new MeterMetric();
        }

        public TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, SamplingType samplingType)
        {
            return new TimerMetric(samplingType);
        }

        public TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, HistogramImplementation histogram)
        {
            return new TimerMetric(histogram);
        }

        public TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, Reservoir reservoir)
        {
            return new TimerMetric(reservoir);
        }
    }
}