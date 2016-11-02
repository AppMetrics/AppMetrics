using System;
using App.Metrics.Core;
using App.Metrics.Internal.Test;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Scheduling;
using App.Metrics.Utils;

namespace App.Metrics.Facts
{
    public class TestMetricsBuilder : IMetricsBuilder
    {
        private readonly IClock _clock;
        private readonly IScheduler _scheduler;

        public TestMetricsBuilder(IClock clock)
        {
            _clock = clock;
            _scheduler = new TestTaskScheduler(_clock);
        }

        public ICounterMetric BuildCounter(string name, Unit unit)
        {
            return new CounterMetric();
        }

        public IMetricValueProvider<double> BuildGauge(string name, Unit unit, Func<double> valueProvider)
        {
            return new FunctionGauge(valueProvider);
        }

        public IHistogramMetric BuildHistogram(string name, Unit unit, SamplingType samplingType)
        {
            return new HistogramMetric(new UniformReservoir());
        }

        public IHistogramMetric BuildHistogram(string name, Unit unit, IReservoir reservoir)
        {
            return new HistogramMetric(new UniformReservoir());
        }

        public IMeterMetric BuildMeter(string name, Unit unit, TimeUnit rateUnit)
        {
            return new MeterMetric(_clock, _scheduler);
        }

        public ITimerMetric BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, SamplingType samplingType)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(_clock, _scheduler), _clock);
        }

        public ITimerMetric BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IHistogramMetric histogram)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(_clock, _scheduler), _clock);
        }

        public ITimerMetric BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IReservoir reservoir)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(_clock, _scheduler), _clock);
        }

        public void Dispose()
        {
        }
    }
}