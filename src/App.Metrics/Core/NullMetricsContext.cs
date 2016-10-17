using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.DataProviders;
using App.Metrics.Health;
using App.Metrics.MetricData;
using App.Metrics.Registries;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class NullMetricsContext : IMetricsContext, IAdvancedMetricsContext
    {
        internal const string InternalMetricsContextName = "App.Metrics.Internal";
        private readonly ConcurrentDictionary<string, IMetricsContext> _childContexts = new ConcurrentDictionary<string, IMetricsContext>();
        private readonly IMetricsContext _metricsContext;
        private bool _isDisabled;

        public NullMetricsContext(string context, IClock systemClock)
        {
            Func<IMetricsRegistry> setupMetricsRegistry = () => new NullMetricsRegistry();
            var metricsRegistry = setupMetricsRegistry();
            var metricsBuilder = new DefaultMetricsBuilder(systemClock);
            var healthCheckDataProvider = new NullHealthCheckDataProvider();

            DataProvider = new DefaultMetricsDataProvider(context,
                () => SystemClock.UtcDateTime,
                metricsRegistry.DataProvider,
                () => _childContexts.Values.Select(c => c.DataProvider));


            _metricsContext = new MetricsContext(context, systemClock, setupMetricsRegistry, metricsBuilder, healthCheckDataProvider);
        }

        public event EventHandler ContextDisabled;

        public event EventHandler ContextShuttingDown;

        public IAdvancedMetricsContext Advanced => this;

        public IMetricsDataProvider DataProvider { get; }

        public Func<Task<HealthStatus>> GetHealthStatusAsync => _metricsContext.GetHealthStatusAsync;

        public IMetricsContext Internal => _metricsContext.Internal;

        public IClock SystemClock => _metricsContext.SystemClock;

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

        public IHistogram Histogram(string name, Unit unit, SamplingType samplingType = SamplingType.Default, MetricTags tags = default(MetricTags))
        {
            return _metricsContext.Histogram(name, unit, samplingType, tags);
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

        public ITimer Timer(string name, Unit unit, SamplingType samplingType = SamplingType.Default, TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags))
        {
            return _metricsContext.Timer(name, unit, samplingType, rateUnit, durationUnit, tags);
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