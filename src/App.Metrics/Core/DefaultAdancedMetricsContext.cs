using System;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Registries;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class DefaultAdancedMetricsContext : IAdvancedMetricsContext
    {
        private readonly IMetricsBuilder _builder;
        private readonly IMetricsContext _context;
        private readonly SamplingType _defaultSamplingType;
        private bool _isDisabled = false;
        private IMetricsRegistry _registry;
        private readonly Func<IMetricsRegistry> _newRegistry;

        public DefaultAdancedMetricsContext(IMetricsContext context,
            IClock clock,
            SamplingType defaultSamplingType,
            IMetricsRegistry registry,
            Func<IMetricsRegistry> newRegistry,
            IMetricsBuilder builder,
            IHealthCheckManager healthCheckManager,
            IMetricsDataManager metricsDataManager)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            _context = context;
            _defaultSamplingType = defaultSamplingType;

            _registry = registry;
            _newRegistry = newRegistry;
            _builder = builder;
            HealthCheckManager = healthCheckManager;
            MetricsDataManager = metricsDataManager;
            Clock = clock;
        }


        public event EventHandler ContextDisabled;

        public event EventHandler ContextShuttingDown;

        public IClock Clock { get; }

        public IHealthCheckManager HealthCheckManager { get; }

        public IMetricRegistryManager MetricRegistryManager => _registry.DataProvider;

        public IMetricsDataManager MetricsDataManager { get; }

        public bool AttachGroup(string groupName, IMetricsContext context)
        {
            if (_isDisabled)
            {
                return true;
            }

            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentException("Group name can't be null or empty for attached contexts");
            }
            var attached = _context.Groups.GetOrAdd(groupName, context);
            return ReferenceEquals(attached, context);
        }

        public void CompletelyDisableMetrics()
        {
            if (_isDisabled)
            {
                return;
            }

            _isDisabled = true;

            var oldRegistry = _registry;
            _registry = new NullMetricsRegistry();
            oldRegistry.ClearAllMetrics();
            using (oldRegistry as IDisposable)
            {
            }

            ForAllGroups(c => c.Advanced.CompletelyDisableMetrics());

            ContextShuttingDown?.Invoke(this, EventArgs.Empty);
            ContextDisabled?.Invoke(this, EventArgs.Empty);
        }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterImplementation
        {
            return _registry.Counter(options.Name, builder, options.MeasurementUnit, options.Tags);
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
            _registry.Gauge(options.Name, valueProvider, options.MeasurementUnit, options.Tags);
        }

        public IMetricsContext Group(string groupName)
        {
            return Group(groupName, c => CreateChildContextInstance(groupName));
        }

        public IMetricsContext Group(string groupName, Func<string, IMetricsContext> groupCreator)
        {
            if (_isDisabled)
            {
                return _context;
            }

            return string.IsNullOrEmpty(groupName)
                ? _context
                : _context.Groups.GetOrAdd(groupName, groupCreator);
        }

        public IHistogram Histogram(HistogramOptions options)
        {
            var samplingType = options.SamplingType == SamplingType.Default ? _defaultSamplingType : options.SamplingType;

            return Histogram(options, () => _builder.BuildHistogram(options.Name, options.MeasurementUnit, samplingType));
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramImplementation
        {
            //NOTE: Options Resevoir will be ignored the builder should specify
            //TODO: AH - ^ bit confusing
            return _registry.Histogram(options.Name, builder, options.MeasurementUnit, options.Tags);
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

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterImplementation
        {
            return _registry.Meter(options.Name, builder, options.MeasurementUnit, options.RateUnit, options.Tags);
        }

        public void ResetMetricsValues()
        {
            _registry.ResetMetricsValues();
            ForAllGroups(c => c.Advanced.ResetMetricsValues());
        }

        public void ShutdownGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentException("groupName must not be null or empty", groupName);
            }

            IMetricsContext context;
            if (_context.Groups.TryRemove(groupName, out context))
            {
                context.Dispose();
            }
        }

        public ITimer Timer(TimerOptions options)
        {
            var samplingType = options.SamplingType == SamplingType.Default ? _defaultSamplingType : options.SamplingType;
            return _registry.Timer(options.Name, 
                () => _builder.BuildTimer(options.Name, options.MeasurementUnit, options.RateUnit, options.DurationUnit, samplingType), 
                options.MeasurementUnit, options.RateUnit, options.DurationUnit, options.Tags);
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerImplementation
        {
            //TODO: AH - ^ bit confusing
            return _registry.Timer(options, builder);
        }

        public ITimer Timer(TimerOptions options, Func<IHistogramImplementation> builder)
        {
            //TODO: AH - ^ bit confusing
            return Timer(options, () => _builder.BuildTimer(options.Name, options.MeasurementUnit, options.RateUnit, options.DurationUnit, builder()));
        }

        public ITimer Timer(TimerOptions options, Func<IReservoir> builder)
        {
            return Timer(options, () => _builder.BuildTimer(options.Name, options.MeasurementUnit, options.RateUnit, options.DurationUnit, builder()));
        }

        private IMetricsContext CreateChildContextInstance(string contextName)
        {
            return new DefaultMetricsContext(contextName, Clock, _defaultSamplingType,
                _newRegistry, _builder,
                HealthCheckManager, MetricsDataManager);
        }

        private void ForAllGroups(Action<IMetricsContext> action)
        {
            foreach (var context in _context.Groups.Values)
            {
                action(context);
            }
        }
    }
}