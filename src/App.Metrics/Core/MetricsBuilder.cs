using System;
using App.Metrics.MetricData;
using App.Metrics.Sampling;

namespace App.Metrics.Core
{
    public interface MetricsBuilder
    {
        CounterImplementation BuildCounter(string name, Unit unit);

        MetricValueProvider<double> BuildGauge(string name, Unit unit, Func<double> valueProvider);

        HistogramImplementation BuildHistogram(string name, Unit unit, SamplingType samplingType);

        HistogramImplementation BuildHistogram(string name, Unit unit, Reservoir reservoir);

        MeterImplementation BuildMeter(string name, Unit unit, TimeUnit rateUnit);

        TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, SamplingType samplingType);

        TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, HistogramImplementation histogram);

        TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, Reservoir reservoir);
    }
}