using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public abstract class BaseMetricsContext : IMetricsContext, IAdvancedMetricsContext
    {
        private readonly ConcurrentDictionary<string, IMetricsContext> _childContexts = new ConcurrentDictionary<string, IMetricsContext>();
        public const string InternalMetricsContextName = "App.Metrics.Internal";
        private bool _isDisabled;
        private IMetricsBuilder _metricsBuilder;
        private IMetricsRegistry _registry;

        protected BaseMetricsContext(string context, 
            IMetricsRegistry registry,
            IMetricsBuilder metricsBuilder, 
            HealthChecks healthChecks,
            IClock systemClock,
            Func<DateTime> timestampProvider)
        {
            _registry = registry;
            _metricsBuilder = metricsBuilder;
            DataProvider = new DefaultDataProvider(context, timestampProvider, _registry.DataProvider,
                () => _childContexts.Values.Select(c => c.DataProvider));
            HealthStatus = healthChecks.GetStatusAsync;
            SystemClock = systemClock;
        }

        public event EventHandler ContextDisabled;

        public event EventHandler ContextShuttingDown;

        public IAdvancedMetricsContext Advanced => this;

        public IMetricsContext Internal { get; }

        public IClock SystemClock { get; }

        public Func<Task<HealthStatus>> HealthStatus { get; }

        public IMetricsDataProvider DataProvider { get; }

        public bool AttachContext(string contextName, IMetricsContext context)
        {
            if (_isDisabled)
            {
                return true;
            }

            if (string.IsNullOrEmpty(contextName))
            {
                throw new ArgumentException("Context name can't be null or empty for attached contexts");
            }
            var attached = _childContexts.GetOrAdd(contextName, context);
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

            ForAllChildContexts(c => c.Advanced.CompletelyDisableMetrics());

            ContextShuttingDown?.Invoke(this, EventArgs.Empty);
            ContextDisabled?.Invoke(this, EventArgs.Empty);
        }

        public IMetricsContext Context(string contextName)
        {
            return Context(contextName, c => CreateChildContextInstance(contextName));
        }

        public IMetricsContext Context(string contextName, Func<string, IMetricsContext> contextCreator)
        {
            if (_isDisabled)
            {
                return this;
            }

            if (string.IsNullOrEmpty(contextName))
            {
                return this;
            }

            return _childContexts.GetOrAdd(contextName, contextCreator);
        }

        public ICounter Counter(string name, Unit unit, MetricTags tags)
        {
            return Counter(name, unit, () => _metricsBuilder.BuildCounter(name, unit), tags);
        }

        public ICounter Counter<T>(string name, Unit unit, Func<T> builder, MetricTags tags)
            where T : ICounterImplementation
        {
            return _registry.Counter(name, builder, unit, tags);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Gauge(string name, Func<double> valueProvider, Unit unit, MetricTags tags)
        {
            Gauge(name, () => _metricsBuilder.BuildGauge(name, unit, valueProvider), unit, tags);
        }

        public void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags)
        {
            _registry.Gauge(name, valueProvider, unit, tags);
        }

        public IHistogram Histogram(string name, Unit unit, SamplingType samplingType, MetricTags tags)
        {
            return Histogram(name, unit, () => _metricsBuilder.BuildHistogram(name, unit, samplingType), tags);
        }

        public IHistogram Histogram<T>(string name, Unit unit, Func<T> builder, MetricTags tags)
            where T : IHistogramImplementation
        {
            return _registry.Histogram(name, builder, unit, tags);
        }

        public IHistogram Histogram(string name, Unit unit, Func<IReservoir> builder, MetricTags tags)
        {
            return Histogram(name, unit, () => _metricsBuilder.BuildHistogram(name, unit, builder()), tags);
        }

        public IMeter Meter(string name, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            return Meter(name, unit, () => _metricsBuilder.BuildMeter(name, unit, rateUnit), rateUnit, tags);
        }

        public IMeter Meter<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, MetricTags tags)
            where T : IMeterImplementation
        {
            return _registry.Meter(name, builder, unit, rateUnit, tags);
        }

        public void ResetMetricsValues()
        {
            _registry.ResetMetricsValues();
            ForAllChildContexts(c => c.Advanced.ResetMetricsValues());
        }

        public void ShutdownContext(string contextName)
        {
            if (string.IsNullOrEmpty(contextName))
            {
                throw new ArgumentException("contextName must not be null or empty", contextName);
            }

            IMetricsContext context;
            if (_childContexts.TryRemove(contextName, out context))
            {
                using (context)
                {
                }
            }
        }

        public ITimer Timer(string name, Unit unit, SamplingType samplingType, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return _registry.Timer(name, () => _metricsBuilder.BuildTimer(name, unit, rateUnit, durationUnit, samplingType), unit, rateUnit,
                durationUnit, tags);
        }

        public ITimer Timer<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : ITimerImplementation
        {
            return _registry.Timer(name, builder, unit, rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IHistogramImplementation> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, () => _metricsBuilder.BuildTimer(name, unit, rateUnit, durationUnit, builder()), rateUnit, durationUnit, tags);
        }

        public ITimer Timer(string name, Unit unit, Func<IReservoir> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, () => _metricsBuilder.BuildTimer(name, unit, rateUnit, durationUnit, builder()), rateUnit, durationUnit, tags);
        }

        public void WithCustomMetricsBuilder(IMetricsBuilder metricsBuilder)
        {
            _metricsBuilder = metricsBuilder;
            ForAllChildContexts(c => c.Advanced.WithCustomMetricsBuilder(metricsBuilder));
        }

        protected abstract IMetricsContext CreateChildContextInstance(string contextName);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_isDisabled)
                {
                    ContextShuttingDown?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void ForAllChildContexts(Action<IMetricsContext> action)
        {
            foreach (var context in _childContexts.Values)
            {
                action(context);
            }
        }
    }
}