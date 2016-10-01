using System;
using System.Collections.Concurrent;
using System.Linq;
using App.Metrics.MetricData;
using App.Metrics.Sampling;

namespace App.Metrics.Core
{
    public abstract class BaseMetricsContext : MetricsContext, AdvancedMetricsContext
    {
        private readonly ConcurrentDictionary<string, MetricsContext> _childContexts = new ConcurrentDictionary<string, MetricsContext>();

        private bool _isDisabled;
        private MetricsBuilder _metricsBuilder;

        private MetricsRegistry _registry;

        protected BaseMetricsContext(string context, MetricsRegistry registry, MetricsBuilder metricsBuilder, Func<DateTime> timestampProvider)
        {
            _registry = registry;
            _metricsBuilder = metricsBuilder;
            DataProvider = new DefaultDataProvider(context, timestampProvider, _registry.DataProvider,
                () => _childContexts.Values.Select(c => c.DataProvider));
        }

        public event EventHandler ContextDisabled;

        public event EventHandler ContextShuttingDown;

        public AdvancedMetricsContext Advanced => this;

        public MetricsDataProvider DataProvider { get; }

        public bool AttachContext(string contextName, MetricsContext context)
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

        public MetricsContext Context(string contextName)
        {
            return Context(contextName, c => CreateChildContextInstance(contextName));
        }

        public MetricsContext Context(string contextName, Func<string, MetricsContext> contextCreator)
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

        public Counter Counter(string name, Unit unit, MetricTags tags)
        {
            return Counter(name, unit, () => _metricsBuilder.BuildCounter(name, unit), tags);
        }

        public Counter Counter<T>(string name, Unit unit, Func<T> builder, MetricTags tags)
            where T : CounterImplementation
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

        public void Gauge(string name, Func<MetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags)
        {
            _registry.Gauge(name, valueProvider, unit, tags);
        }

        public Histogram Histogram(string name, Unit unit, SamplingType samplingType, MetricTags tags)
        {
            return Histogram(name, unit, () => _metricsBuilder.BuildHistogram(name, unit, samplingType), tags);
        }

        public Histogram Histogram<T>(string name, Unit unit, Func<T> builder, MetricTags tags)
            where T : HistogramImplementation
        {
            return _registry.Histogram(name, builder, unit, tags);
        }

        public Histogram Histogram(string name, Unit unit, Func<Reservoir> builder, MetricTags tags)
        {
            return Histogram(name, unit, () => _metricsBuilder.BuildHistogram(name, unit, builder()), tags);
        }

        public Meter Meter(string name, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            return Meter(name, unit, () => _metricsBuilder.BuildMeter(name, unit, rateUnit), rateUnit, tags);
        }

        public Meter Meter<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, MetricTags tags)
            where T : MeterImplementation
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

            MetricsContext context;
            if (_childContexts.TryRemove(contextName, out context))
            {
                using (context)
                {
                }
            }
        }

        public Timer Timer(string name, Unit unit, SamplingType samplingType, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return _registry.Timer(name, () => _metricsBuilder.BuildTimer(name, unit, rateUnit, durationUnit, samplingType), unit, rateUnit,
                durationUnit, tags);
        }

        public Timer Timer<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : TimerImplementation
        {
            return _registry.Timer(name, builder, unit, rateUnit, durationUnit, tags);
        }

        public Timer Timer(string name, Unit unit, Func<HistogramImplementation> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, () => _metricsBuilder.BuildTimer(name, unit, rateUnit, durationUnit, builder()), rateUnit, durationUnit, tags);
        }

        public Timer Timer(string name, Unit unit, Func<Reservoir> builder, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            return Timer(name, unit, () => _metricsBuilder.BuildTimer(name, unit, rateUnit, durationUnit, builder()), rateUnit, durationUnit, tags);
        }

        public void WithCustomMetricsBuilder(MetricsBuilder metricsBuilder)
        {
            _metricsBuilder = metricsBuilder;
            ForAllChildContexts(c => c.Advanced.WithCustomMetricsBuilder(metricsBuilder));
        }

        protected abstract MetricsContext CreateChildContextInstance(string contextName);

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

        private void ForAllChildContexts(Action<MetricsContext> action)
        {
            foreach (var context in _childContexts.Values)
            {
                action(context);
            }
        }
    }
}