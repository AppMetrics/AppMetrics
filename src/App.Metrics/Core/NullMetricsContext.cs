using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Registries;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class NullMetricsContext : IMetricsContext, IAdvancedMetricsContext
    {
        private readonly SamplingType _defaultSamplingType;
        internal const string InternalMetricsContextName = "App.Metrics.Internal";
        private readonly ConcurrentDictionary<string, IMetricsContext> _childContexts = new ConcurrentDictionary<string, IMetricsContext>();
        private readonly IMetricsContext _metricsContext;

        public NullMetricsContext(string context, IClock systemClock, SamplingType defaultSamplingType)
        {
            _defaultSamplingType = defaultSamplingType;
            Func<IMetricsRegistry> setupMetricsRegistry = () => new NullMetricsRegistry();
            var metricsBuilder = new DefaultMetricsBuilder(systemClock, defaultSamplingType);
            var healthCheckDataProvider = new NullHealthCheckDataProvider();
            IMetricsDataProvider metricsDataProvider
                = new NullMetricsDataProvider();

            HealthCheckDataProvider = healthCheckDataProvider;
            Name = context;
            MetricsDataProvider = metricsDataProvider;
            RegistryDataProvider = setupMetricsRegistry().DataProvider;

            _metricsContext = new MetricsContext(context, systemClock, defaultSamplingType,
                setupMetricsRegistry, metricsBuilder,
                healthCheckDataProvider, metricsDataProvider);
        }

        public event EventHandler ContextDisabled;

        public event EventHandler ContextShuttingDown;

        public IAdvancedMetricsContext Advanced => this;

        public IReadOnlyDictionary<string, IMetricsContext> ChildContexts => _childContexts;

        public IClock Clock => _metricsContext.Advanced.Clock;

        public IHealthCheckDataProvider HealthCheckDataProvider { get; }

        public IRegistryDataProvider RegistryDataProvider { get; }

        public IMetricsContext Internal => _metricsContext.Internal;

        public IMetricsDataProvider MetricsDataProvider { get; }

        public string Name { get; }

        public bool AttachContext(string contextName, IMetricsContext context)
        {
            return _metricsContext.Advanced.AttachContext(contextName, context);
        }

        public void CompletelyDisableMetrics()
        {
            _metricsContext.Advanced.CompletelyDisableMetrics();
        }

        public IMetricsContext Context(string contextName)
        {
            return _metricsContext.Context(contextName);
        }

        public IMetricsContext Context(string contextName, Func<string, IMetricsContext> contextCreator)
        {
            return _metricsContext.Context(contextName, contextCreator);
        }

        public ICounter Counter(string name, Unit unit, MetricTags tags = default(MetricTags))
        {
            return _metricsContext.Counter(name, unit, tags);
        }

        public ICounter Counter<T>(string name, Unit unit, Func<T> builder, MetricTags tags = default(MetricTags)) where T : ICounterImplementation
        {
            return _metricsContext.Advanced.Counter(name, unit, builder, tags);
        }

        public void Dispose()
        {
            _metricsContext.Dispose();
        }

        public void Gauge(string name, Func<double> valueProvider, Unit unit, MetricTags tags = default(MetricTags))
        {
            _metricsContext.Gauge(name, valueProvider, unit, tags);
        }

        public void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags = default(MetricTags))
        {
            _metricsContext.Advanced.Gauge(name, valueProvider, unit, tags);
        }

        public IHistogram Histogram(string name, Unit unit, SamplingType samplingType, MetricTags tags = default(MetricTags))
        {
            return _metricsContext.Histogram(name, unit, samplingType, tags);
        }

        public IHistogram Histogram(string name, Unit unit, MetricTags tags = default(MetricTags))
        {
            return Histogram(name, unit, _defaultSamplingType, tags);
        }

        public IHistogram Histogram<T>(string name, Unit unit, Func<T> builder, MetricTags tags = default(MetricTags))
            where T : IHistogramImplementation
        {
            return _metricsContext.Advanced.Histogram(name, unit, builder, tags);
        }

        public IHistogram Histogram(string name, Unit unit, Func<IReservoir> builder, MetricTags tags = default(MetricTags))
        {
            return _metricsContext.Advanced.Histogram(name, unit, builder, tags);
        }

        public IMeter Meter(string name, Unit unit, TimeUnit rateUnit = TimeUnit.Seconds, MetricTags tags = default(MetricTags))
        {
            return _metricsContext.Meter(name, unit, rateUnit, tags);
        }

        public IMeter Meter<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit = TimeUnit.Seconds, MetricTags tags = default(MetricTags))
            where T : IMeterImplementation
        {
            return _metricsContext.Advanced.Meter(name, unit, builder, rateUnit, tags);
        }

        public void ResetMetricsValues()
        {
            _metricsContext.Advanced.ResetMetricsValues();
        }

        public void ShutdownContext(string contextName)
        {
            _metricsContext.ShutdownContext(contextName);
        }

        public ITimer Timer(string name, Unit unit, SamplingType samplingType, TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags))
        {
            return _metricsContext.Timer(name, unit, samplingType, rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, TimeUnit rateUnit = TimeUnit.Seconds,
           TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags))
        {
            return Timer(name, unit, _defaultSamplingType, rateUnit, durationUnit, tags);
        }

        public ITimer Timer<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags)) where T : ITimerImplementation
        {
            return _metricsContext.Advanced.Timer(name, unit, builder, rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IHistogramImplementation> builder, TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags))
        {
            return _metricsContext.Advanced.Timer(name, unit, builder, rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IReservoir> builder, TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags))
        {
            return _metricsContext.Advanced.Timer(name, unit, builder, rateUnit, durationUnit, tags);
        }
    }
}