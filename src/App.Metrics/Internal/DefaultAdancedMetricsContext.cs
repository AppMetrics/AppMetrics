using System;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;
using Microsoft.Extensions.Options;

namespace App.Metrics.Internal
{
    internal sealed class DefaultAdancedMetricsContext : IAdvancedMetricsContext
    {
        private readonly IMetricsBuilder _builder;
        private bool _isDisabled;
        private IMetricsRegistry _registry;

        public DefaultAdancedMetricsContext(IMetricsContext context,
            IOptions<AppMetricsOptions> options,
            IMetricsRegistry registry,
            IMetricsBuilder builder,
            IHealthCheckManager healthCheckManager,
            IMetricsDataManager dataManagerManager)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            _registry = registry;
            _builder = builder;
            HealthCheckManager = healthCheckManager;
            DataManager = dataManagerManager;
            Clock = options.Value.Clock;
            _isDisabled = options.Value.DisableMetrics;

            if (_isDisabled)
            {
                CompletelyDisableMetrics();
            }
        }


        public IClock Clock { get; }

        public IMetricsDataManager DataManager { get; }

        public IHealthCheckManager HealthCheckManager { get; }

        public void CompletelyDisableMetrics()
        {
            if (_isDisabled)
            {
                return;
            }

            _isDisabled = true;

            var oldRegistry = _registry;
            _registry = new NullMetricsRegistry();
            oldRegistry.Clear();
            //TODO: AH - does the registry need to be disposable
            using (oldRegistry as IDisposable)
            {
            }
        }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric
        {
            return _registry.Counter(options, builder);
        }

        public ICounter Counter(CounterOptions options)
        {
            return Counter(options, () => _builder.BuildCounter(options.Name, options.MeasurementUnit));
        }

        public void Gauge(GaugeOptions options, Func<double> valueProvider)
        {
            Gauge(options, () => _builder.BuildGauge(options.Name, options.MeasurementUnit, valueProvider));
        }

        public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider)
        {
            _registry.Gauge(options, valueProvider);
        }

        public IHistogram Histogram(HistogramOptions options)
        {
            return Histogram(options, () => _builder.BuildHistogram(options.Name, options.MeasurementUnit, options.SamplingType));
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric
        {
            //NOTE: Options Resevoir will be ignored the builder should specify
            //TODO: AH - ^ bit confusing
            return _registry.Histogram(options, builder);
        }

        public IHistogram Histogram(HistogramOptions options, Func<IReservoir> builder)
        {
            //NOTE: Options Resevoir will be ignored since we're defining it with the builder
            //TODO: AH - ^ bit confusing
            return Histogram(options, () => _builder.BuildHistogram(options.Name, options.MeasurementUnit, builder()));
        }

        public IMeter Meter(MeterOptions options)
        {
            return Meter(options, () => _builder.BuildMeter(options.Name, options.MeasurementUnit, options.RateUnit));
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric
        {
            return _registry.Meter(options, builder);
        }

        public void ResetMetricsValues()
        {
            _registry.Clear();
        }

        public void ShutdownGroup(string groupName)
        {
            _registry.RemoveGroup(groupName);
        }

        public ITimer Timer(TimerOptions options)
        {
            return _registry.Timer(options,
                () => _builder.BuildTimer(options.Name, options.MeasurementUnit, options.RateUnit, options.DurationUnit, options.SamplingType));
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerMetric
        {
            return _registry.Timer(options, builder);
        }

        public ITimer Timer(TimerOptions options, Func<IHistogramMetric> builder)
        {
            return Timer(options, () => _builder.BuildTimer(options.Name, options.MeasurementUnit, options.RateUnit, options.DurationUnit, builder()));
        }

        public ITimer Timer(TimerOptions options, Func<IReservoir> builder)
        {
            return Timer(options, () => _builder.BuildTimer(options.Name, options.MeasurementUnit, options.RateUnit, options.DurationUnit, builder()));
        }
    }
}