using System;
using System.Threading.Tasks;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics
{
    /// <summary>
    ///     Represents a logical grouping of metrics
    /// </summary>
    public interface IMetricsContext : IDisposable, IHideObjectMembers
    {
        /// <summary>
        ///     Exposes advanced operations that are possible on this metrics context.
        /// </summary>
        IAdvancedMetricsContext Advanced { get; }

        IMetricsContext Internal { get; }

        IClock SystemClock { get; }

        Func<Task<HealthStatus>> HealthStatus { get; }

        /// <summary>
        ///     Returns a metrics data provider capable of returning the metrics in this context and any existing child contexts.
        /// </summary>
        IMetricsDataProvider DataProvider { get; }

        /// <summary>
        ///     Create a new child metrics context. Metrics added to the child context are kept separate from the metrics in the
        ///     parent context.
        /// </summary>
        /// <param name="contextName">Name of the child context.</param>
        /// <returns>Newly created child context.</returns>
        IMetricsContext Context(string contextName);

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
        IMetricsContext Context(string contextName, Func<string, IMetricsContext> contextCreator);

        /// <summary>
        ///     A counter is a simple incrementing and decrementing 64-bit integer. Ex number of active requests.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all counters in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        ICounter Counter(string name, Unit unit, MetricTags tags = default(MetricTags));

        /// <summary>
        ///     A gauge is the simplest metric type. It just returns a value. This metric is suitable for instantaneous values.
        /// </summary>
        /// <param name="name">Name of this gauge metric. Must be unique across all gauges in this context.</param>
        /// <param name="valueProvider">Function that returns the value for the gauge.</param>
        /// <param name="unit">Description of want the value represents ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        void Gauge(string name, Func<double> valueProvider, Unit unit, MetricTags tags = default(MetricTags));

        /// <summary>
        ///     A Histogram measures the distribution of values in a stream of data: e.g., the number of results returned by a
        ///     search.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all histograms in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="samplingType">Type of the sampling to use (see SamplingType for details ).</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        IHistogram Histogram(string name,
            Unit unit,
            SamplingType samplingType = SamplingType.Default,
            MetricTags tags = default(MetricTags));

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
        IMeter Meter(string name, Unit unit, TimeUnit rateUnit = TimeUnit.Seconds, MetricTags tags = default(MetricTags));

        /// <summary>
        ///     Remove a child context. The metrics for the child context are removed from the MetricsData of the parent context.
        /// </summary>
        /// <param name="contextName">Name of the child context to shutdown.</param>
        void ShutdownContext(string contextName);

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
        ITimer Timer(string name,
            Unit unit,
            SamplingType samplingType = SamplingType.Default,
            TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds,
            MetricTags tags = default(MetricTags));
    }
}