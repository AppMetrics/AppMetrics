using System;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    /// <summary>
    ///     Read only metrics context ( ex: to capture metrics from a remote system )
    /// </summary>
    public abstract class ReadOnlyMetricsContext : MetricsContext
    {
        public abstract MetricsDataProvider DataProvider { get; }

        public AdvancedMetricsContext Advanced
        {
            get { throw new ReadOnlyMetricsContextException(); }
        }

        public MetricsContext Context(string contextName)
        {
            throw new ReadOnlyMetricsContextException();
        }

        public MetricsContext Context(string contextName, Func<string, MetricsContext> contextCreator)
        {
            throw new ReadOnlyMetricsContextException();
        }

        public Counter Counter(string name, Unit unit, MetricTags tags)
        {
            throw new ReadOnlyMetricsContextException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Gauge(string name, Func<double> valueProvider, Unit unit, MetricTags tags)
        {
            throw new ReadOnlyMetricsContextException();
        }

        public Histogram Histogram(string name, Unit unit, SamplingType samplingType, MetricTags tags)
        {
            throw new ReadOnlyMetricsContextException();
        }

        public Meter Meter(string name, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            throw new ReadOnlyMetricsContextException();
        }

        public void PerformanceCounter(string name, string counterCategory, string counterName, string counterInstance, Unit unit, MetricTags tags)
        {
            throw new ReadOnlyMetricsContextException();
        }

        public void ShutdownContext(string contextName)
        {
            throw new ReadOnlyMetricsContextException();
        }

        public Timer Timer(string name, Unit unit, SamplingType samplingType, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            throw new ReadOnlyMetricsContextException();
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }

    public sealed class ReadOnlyMetricsContextException : InvalidOperationException
    {
        public ReadOnlyMetricsContextException()
            : base("This context is read-only. You can only use the DataProvider property to read the metrics data")
        {
        }
    }
}