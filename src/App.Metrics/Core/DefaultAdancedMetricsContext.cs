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

        public IHistogram Histogram(string name, Unit unit, SamplingType samplingType, MetricTags tags)
        {
            return Histogram(name, unit, () => _builder.BuildHistogram(name, unit, samplingType), tags);
        }

        public IHistogram Histogram(string name, Unit unit, MetricTags tags)
        {
            return Histogram(name, unit, _defaultSamplingType, tags);
        }

        public IHistogram Histogram<T>(string name, Unit unit, Func<T> builder, MetricTags tags)
            where T : IHistogramImplementation
        {
            return _registry.Histogram(name, builder, unit, tags);
        }

        public IHistogram Histogram(string name, Unit unit, Func<IReservoir> builder, MetricTags tags)
        {
            return Histogram(name, unit, () => _builder.BuildHistogram(name, unit, builder()), tags);
        }

        public IMeter Meter(MeterOptions options)
        {
            return Meter(options.Name, options.MeasurementUnit,
                () => _builder.BuildMeter(options.Name, options.MeasurementUnit, options.RateUnit),
                options.RateUnit, options.Tags);
        }

        public IMeter Meter<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, MetricTags tags)
            where T : IMeterImplementation
        {
            return _registry.Meter(name, builder, unit, rateUnit, tags);
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

        public ITimer Timer(string name, Unit unit, SamplingType samplingType, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return _registry.Timer(name, () => _builder.BuildTimer(name, unit, rateUnit, durationUnit, samplingType), unit, rateUnit,
                durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, _defaultSamplingType, rateUnit, durationUnit, tags);
        }

        public ITimer Timer<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : ITimerImplementation
        {
            return _registry.Timer(name, builder, unit, rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IHistogramImplementation> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, () => _builder.BuildTimer(name, unit, rateUnit, durationUnit, builder()), rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IReservoir> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, () => _builder.BuildTimer(name, unit, rateUnit, durationUnit, builder()), rateUnit, durationUnit, tags);
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