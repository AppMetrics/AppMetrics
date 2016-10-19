// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics
{
    public interface IAdvancedMetricsContext : IHideObjectMembers
    {
        /// <summary>
        ///     Event fired when the context CompletelyDisableMetrics is called.
        /// </summary>
        event EventHandler ContextDisabled;

        /// <summary>
        ///     Event fired when the context is disposed or shutdown or the CompletelyDisableMetrics is called.
        /// </summary>
        event EventHandler ContextShuttingDown;

        IReadOnlyDictionary<string, IMetricsContext> ChildContexts { get; }

        IClock Clock { get; }

        IHealthCheckDataProvider HealthCheckDataProvider { get; }

        IRegistryDataProvider RegistryDataProvider { get; }

        /// <summary>
        ///     Returns a metrics data provider capable of returning the metrics in this context and any existing child contexts.
        /// </summary>
        IMetricsDataProvider MetricsDataProvider { get; }

        /// <summary>
        ///     Attach a context that has already been created (ex: by a library exposing internal metrics)
        /// </summary>
        /// <param name="contextName">name of the context to attach</param>
        /// <param name="context">Existing context instance.</param>
        /// <returns>true if the context was attached, false otherwise.</returns>
        bool AttachContext(string contextName, IMetricsContext context);

        /// <summary>
        ///     All metrics operations will be NO-OP.
        ///     This is useful for measuring the impact of the metrics library on the application.
        ///     If you think the Metrics library is causing issues, this will disable all Metrics operations.
        /// </summary>
        void CompletelyDisableMetrics();

        /// <summary>
        ///     Register a custom Counter instance
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all counters in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="builder">Function used to build a custom instance.</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        ICounter Counter<T>(string name, Unit unit, Func<T> builder, MetricTags tags = default(MetricTags))
            where T : ICounterImplementation;

        /// <summary>
        ///     Register a custom Gauge instance.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all counters in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="valueProvider">Function used to build a custom instance.</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags = default(MetricTags));

        /// <summary>
        ///     Register a custom Histogram instance
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all histograms in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="builder">Function used to build a custom instance.</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        IHistogram Histogram<T>(string name, Unit unit, Func<T> builder, MetricTags tags = default(MetricTags))
            where T : IHistogramImplementation;

        /// <summary>
        ///     Register a Histogram metric with a custom Reservoir instance
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all histograms in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="builder">Function used to build a custom reservoir instance.</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        IHistogram Histogram(string name, Unit unit, Func<IReservoir> builder, MetricTags tags = default(MetricTags));

        /// <summary>
        ///     Register a custom Meter instance.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all meters in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="builder">Function used to build a custom instance.</param>
        /// <param name="rateUnit">Time unit for rates reporting. Defaults to Second ( occurrences / second ).</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        IMeter Meter<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit = TimeUnit.Seconds, MetricTags tags = default(MetricTags))
            where T : IMeterImplementation;

        /// <summary>
        ///     Clear all collected data for all the metrics in this context
        /// </summary>
        void ResetMetricsValues();

        /// <summary>
        ///     Register a custom Timer implementation.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all timers in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="builder">Function used to build a custom instance.</param>
        /// <param name="rateUnit">Time unit for rates reporting. Defaults to Second ( occurrences / second ).</param>
        /// <param name="durationUnit">Time unit for reporting durations. Defaults to Milliseconds. </param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        ITimer Timer<T>(string name, Unit unit, Func<T> builder, TimeUnit rateUnit = TimeUnit.Seconds, TimeUnit durationUnit = TimeUnit.Milliseconds,
            MetricTags tags = default(MetricTags))
            where T : ITimerImplementation;

        /// <summary>
        ///     Register a Timer metric with a custom Histogram implementation.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all timers in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="builder">Function used to build a custom histogram instance.</param>
        /// <param name="rateUnit">Time unit for rates reporting. Defaults to Second ( occurrences / second ).</param>
        /// <param name="durationUnit">Time unit for reporting durations. Defaults to Milliseconds. </param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        ITimer Timer(string name, Unit unit, Func<IHistogramImplementation> builder, TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags));

        /// <summary>
        ///     Register a Timer metric with a custom Reservoir implementation for the histogram.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all timers in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="builder">Function used to build a custom reservoir instance.</param>
        /// <param name="rateUnit">Time unit for rates reporting. Defaults to Second ( occurrences / second ).</param>
        /// <param name="durationUnit">Time unit for reporting durations. Defaults to Milliseconds. </param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        ITimer Timer(string name, Unit unit, Func<IReservoir> builder, TimeUnit rateUnit = TimeUnit.Seconds,
            TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags));
    }
}