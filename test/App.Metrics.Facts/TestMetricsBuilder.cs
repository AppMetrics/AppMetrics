using System;
using App.Metrics.Core;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Facts
{
    public class TestMetricsBuilder : MetricsBuilder
    {
        private readonly Clock clock;
        private readonly Scheduler scheduler;

        public TestMetricsBuilder(Clock clock, Scheduler scheduler)
        {
            this.clock = clock;
            this.scheduler = scheduler;
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
            return new MeterMetric(this.clock, this.scheduler);
        }

        public TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, SamplingType samplingType)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(this.clock, this.scheduler), this.clock);
        }

        public TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, HistogramImplementation histogram)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(this.clock, this.scheduler), this.clock);
        }

        public TimerImplementation BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, Reservoir reservoir)
        {
            return new TimerMetric(new HistogramMetric(new UniformReservoir()), new MeterMetric(this.clock, this.scheduler), this.clock);
        }
    }
}