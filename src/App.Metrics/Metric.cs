using System;
using System.Diagnostics;
using App.Metrics.Core;
using App.Metrics.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace App.Metrics
{
    /// <summary>
    ///     Static wrapper around a global MetricContext instance.
    /// </summary>
    public static class Metric
    {
        private static readonly IMetricsContext GlobalContext;
        private static IServiceProvider _serviceProvider;

        static Metric()
        {
            GlobalContext = _serviceProvider?.GetService<IMetricsContext>() ??
                            new DefaultMetricsContext(GetDefaultGlobalContextName(), Clock.Default);

            var options = _serviceProvider?.GetService<IOptions<AppMetricsOptions>>()?.Value ??
                          new AppMetricsOptions();

            if (options.DisableMetrics)
            {
                GlobalContext.CompletelyDisableMetrics();
            }
        }

        /// <summary>
        ///     Exposes advanced operations that are possible on this metrics context.
        /// </summary>
        public static IAdvancedMetricsContext Advanced => (IAdvancedMetricsContext)GlobalContext;

        /// <summary>
        ///     Entrypoint for Global Metrics Configuration.
        /// </summary>
        internal static IMetricsContext Internal { get; } = new DefaultMetricsContext("App.Metrics", Clock.Default);

        /// <summary>
        ///     Create a new child metrics context. Metrics added to the child context are kept separate from the metrics in the
        ///     parent context.
        /// </summary>
        /// <param name="contextName">Name of the child context.</param>
        /// <returns>Newly created child context.</returns>
        public static IMetricsContext Context(string contextName)
        {
            return GlobalContext.Context(contextName);
        }

        /// <summary>
        ///     Create a new child metrics context. Metrics added to the child context are kept separate from the metrics in the
        ///     parent context.
        /// </summary>
        /// <param name="contextName">Name of the child context.</param>
        /// <param name="contextCreator">
        ///     Function used to create the instance of the child context. (Use for creating custom
        ///     contexts)
        /// </param>
        /// <returns>Newly created child context.</returns>
        public static IMetricsContext Context(string contextName, Func<string, IMetricsContext> contextCreator)
        {
            return GlobalContext.Context(contextName, contextCreator);
        }

        /// <summary>
        ///     A counter is a simple incrementing and decrementing 64-bit integer. Ex number of active requests.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all counters in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="tags">
        ///     Optional set of tags that can be associated with the metric. Tags can be string array or comma separated values in
        ///     a string.
        ///     ex: tags: "tag1,tag2" or tags: new[] {"tag1", "tag2"}
        /// </param>
        /// <returns>Reference to the metric</returns>
        public static ICounter Counter(string name, Unit unit, MetricTags tags = default(MetricTags))
        {
            return GlobalContext.Counter(name, unit, tags);
        }

        /// <summary>
        ///     A gauge is the simplest metric type. It just returns a value. This metric is suitable for instantaneous values.
        /// </summary>
        /// <param name="name">Name of this gauge metric. Must be unique across all gauges in this context.</param>
        /// <param name="valueProvider">Function that returns the value for the gauge.</param>
        /// <param name="unit">Description of want the value represents ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the gauge</returns>
        public static void Gauge(string name, Func<double> valueProvider, Unit unit, MetricTags tags = default(MetricTags))
        {
            GlobalContext.Gauge(name, valueProvider, unit, tags);
        }

        /// <summary>
        ///     A Histogram measures the distribution of values in a stream of data: e.g., the number of results returned by a
        ///     search.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all histograms in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="samplingType">Type of the sampling to use (see SamplingType for details ).</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        public static IHistogram Histogram(string name, Unit unit, SamplingType samplingType = SamplingType.Default,
            MetricTags tags = default(MetricTags))
        {
            return GlobalContext.Histogram(name, unit, samplingType, tags);
        }

        /// <summary>
        ///     A meter measures the rate at which a set of events occur, in a few different ways.
        ///     This metric is suitable for keeping a record of now often something happens ( error, request etc ).
        /// </summary>
        /// <remarks>
        ///     The mean rate is the average rate of events. It’s generally useful for trivia,
        ///     but as it represents the total rate for your application’s entire lifetime (e.g., the total number of requests
        ///     handled,
        ///     divided by the number of seconds the process has been running), it doesn’t offer a sense of recency.
        ///     Luckily, meters also record three different exponentially-weighted moving average rates: the 1-, 5-, and 15-minute
        ///     moving averages.
        /// </remarks>
        /// <param name="name">Name of the metric. Must be unique across all meters in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="rateUnit">Time unit for rates reporting. Defaults to Second ( occurrences / second ).</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        public static IMeter Meter(string name, Unit unit, TimeUnit rateUnit = TimeUnit.Seconds, MetricTags tags = default(MetricTags))
        {
            return GlobalContext.Meter(name, unit, rateUnit, tags);
        }

        /// <summary>
        ///     Remove a child context. The metrics for the child context are removed from the MetricsData of the parent context.
        /// </summary>
        /// <param name="contextName">Name of the child context to shutdown.</param>
        public static void ShutdownContext(string contextName)
        {
            GlobalContext.ShutdownContext(contextName);
        }

        /// <summary>
        ///     A timer is basically a histogram of the duration of a type of event and a meter of the rate of its occurrence.
        ///     <seealso cref="Histogram" /> and <seealso cref="Meter" />
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all timers in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="samplingType">Type of the sampling to use (see SamplingType for details ).</param>
        /// <param name="rateUnit">Time unit for rates reporting. Defaults to Second ( occurrences / second ).</param>
        /// <param name="durationUnit">Time unit for reporting durations. Defaults to Milliseconds. </param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        public static ITimer Timer(string name, Unit unit, SamplingType samplingType = SamplingType.Default,
            TimeUnit rateUnit = TimeUnit.Seconds, TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags))
        {
            return GlobalContext.Timer(name, unit, samplingType, rateUnit, durationUnit, tags);
        }

        internal static void EnableInternalMetrics()
        {
            ((IAdvancedMetricsContext)GlobalContext).AttachContext("App.Metrics", Internal);
        }

        /// <summary>
        ///     Initializes the specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        internal static void Init(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        private static string CleanName(string name)
        {
            return name.Replace('.', '_');
        }

        private static string GetDefaultGlobalContextName()
        {
            return $@"{CleanName(Environment.MachineName)}.{CleanName(Process.GetCurrentProcess().ProcessName)}";
        }
    }
}