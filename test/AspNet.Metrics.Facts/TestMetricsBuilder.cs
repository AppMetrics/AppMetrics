using System;
using Metrics;
using Metrics.Core;
using Metrics.MetricData;
using Metrics.PerfCounters;
using Metrics.Sampling;
using Metrics.Utils;

namespace AspNet.Metrics.Facts
{
    public class TestMetricsBuilder : MetricsBuilder
    {
        private readonly Clock _clock;
        private readonly Scheduler _scheduler;

        public TestMetricsBuilder(Clock clock, Scheduler scheduler)
        {
            _clock = clock;
            _scheduler = scheduler;
        }

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
            return new HistogramMetric(new UniformReservoir());
        }

        public HistogramImplementation BuildHistogram(string name, Unit unit, Reservoir reservoir)
        {
            return new HistogramMetric(new UniformReservoir());
        }

        public MeterImplementation BuildMeter(string name, Unit unit, TimeUnit rateUnit)
        {
            return new MeterMetric(_clock, _scheduler);
        }

        public MetricValueProvider<double> BuildPerformanceCounter(string name, Unit unit, string counterCategory, string counterName,
            string counterInstance)
        {
            return new PerformanceCounterGauge(counterCategory, counterName, counterInstance);
        }

        public TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, SamplingType samplingType)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(_clock, _scheduler), _clock);
        }

        public TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, HistogramImplementation histogram)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(_clock, _scheduler), _clock);
        }

        public TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, Reservoir reservoir)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(_clock, _scheduler), _clock);
        }
    }
}