using System;
using App.Metrics;
using App.Metrics.Core;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace AspNet.Metrics.Facts
{
    public class TestMetricsBuilder : IMetricsBuilder
    {
        private readonly Clock _clock;
        private readonly IScheduler _scheduler;

        public TestMetricsBuilder(Clock clock, IScheduler scheduler)
        {
            _clock = clock;
            _scheduler = scheduler;
        }

        public ICounterImplementation BuildCounter(string name, Unit unit)
        {
            return new CounterMetric();
        }

        public IMetricValueProvider<double> BuildGauge(string name, Unit unit, Func<double> valueProvider)
        {
            return new FunctionGauge(valueProvider);
        }

        public IHistogramImplementation BuildHistogram(string name, Unit unit, SamplingType samplingType)
        {
            return new HistogramMetric(new UniformReservoir());
        }

        public IHistogramImplementation BuildHistogram(string name, Unit unit, IReservoir reservoir)
        {
            return new HistogramMetric(new UniformReservoir());
        }

        public IMeterImplementation BuildMeter(string name, Unit unit, TimeUnit rateUnit)
        {
            return new MeterMetric(_clock, _scheduler);
        }

        public ITimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, SamplingType samplingType)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(_clock, _scheduler), _clock);
        }

        public ITimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IHistogramImplementation histogram)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(_clock, _scheduler), _clock);
        }

        public ITimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IReservoir reservoir)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(_clock, _scheduler), _clock);
        }

        public void Dispose()
        {
        }
    }
}