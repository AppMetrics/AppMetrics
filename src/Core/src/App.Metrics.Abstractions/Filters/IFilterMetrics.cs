// <copyright file="IFilterMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Filters
{
    public interface IFilterMetrics
    {
        /// <summary>
        ///     Determines whether the specified apdex is match.
        /// </summary>
        /// <param name="apdex">The apdex.</param>
        /// <returns>True if the metric type is an apdex, the name matches and tags match</returns>
        bool IsApdexMatch(ApdexValueSource apdex);

        /// <summary>
        ///     Determines whether the specified counter is match.
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>True if the metric type is a counter, the name matches and tags match</returns>
        bool IsCounterMatch(CounterValueSource counter);

        /// <summary>
        ///     Determines whether the specified gauge is match.
        /// </summary>
        /// <param name="gauge">The gauge.</param>
        /// <returns>True if the metric type is a gauge, the name matches and tags match</returns>
        bool IsGaugeMatch(GaugeValueSource gauge);

        /// <summary>
        ///     Determines whether the specified histogram is match.
        /// </summary>
        /// <param name="histogram">The histogram.</param>
        /// <returns>True if the metric type is a histogram, the name matches and tags match</returns>
        bool IsHistogramMatch(HistogramValueSource histogram);

        /// <summary>
        ///     Determines whether the specified bucket histogram is match.
        /// </summary>
        /// <param name="histogram">The bucket histogram.</param>
        /// <returns>True if the metric type is a bucket histogram, the name matches and tags match</returns>
        bool IsBucketHistogramMatch(BucketHistogramValueSource histogram);

        /// <summary>
        ///     Determines whether the specified context is match.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>true if the context matches</returns>
        bool IsContextMatch(string context);

        /// <summary>
        ///     Determines whether the specified meter is match.
        /// </summary>
        /// <param name="meter">The meter.</param>
        /// <returns>True if the metric type is a meter, the name matches and tags match</returns>
        bool IsMeterMatch(MeterValueSource meter);

        /// <summary>
        ///     Determines whether the specified timer is match.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <returns>True if the metric type is a timer, the name matches and tags match</returns>
        bool IsTimerMatch(TimerValueSource timer);

        /// <summary>
        ///     Determines whether the specified timer is match.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <returns>True if the metric type is a timer, the name matches and tags match</returns>
        bool IsBucketTimerMatch(BucketTimerValueSource timer);

        /// <summary>
        ///     Filters metrics where the specified predicate on the metrics context is <c>true</c>
        /// </summary>
        /// <param name="condition">The predicate on the context to filter on.</param>
        /// <returns>A filter where the metric context should match</returns>
        IFilterMetrics WhereContext(Predicate<string> condition);

        /// <summary>
        ///     Filters metrics where the specified context matches
        /// </summary>
        /// <param name="context">The metrics context to filter on.</param>
        /// <returns>A filter where the metric context should match</returns>
        IFilterMetrics WhereContext(string context);

        /// <summary>
        ///     Filters metrics where the specified predicate on the metric name is <c>true</c>
        /// </summary>
        /// <param name="name">The metric name to filter on.</param>
        /// <returns>A filter where the metric name should match</returns>
        IFilterMetrics WhereName(string name);

        /// <summary>
        ///     Filters metrics where the specified predicate on the metric name is <c>true</c>
        /// </summary>
        /// <param name="condition">The predicate on the metric name to filter on.</param>
        /// <returns>A filter where the metric name should match</returns>
        IFilterMetrics WhereName(Predicate<string> condition);

        /// <summary>
        ///     Filters metrics where the metric name starts with the specified name
        /// </summary>
        /// <param name="name">The metrics name to filter on.</param>
        /// <returns>A filter where the metric name starts with the specified name</returns>
        IFilterMetrics WhereNameStartsWith(string name);

        /// <summary>
        ///     Filters metrics where the metrics contain the specified tags keys
        /// </summary>
        /// <param name="tagKeys">The metrics tag keys to filter on.</param>
        /// <returns>A filter where the metric tags keys should match</returns>
        IFilterMetrics WhereTaggedWithKey(params string[] tagKeys);

        /// <summary>
        ///     Filters metrics where the metrics contain the specified tags key/value pair
        /// </summary>
        /// <param name="tags">The metrics tag key/values to filter on.</param>
        /// <returns>A filter where the metric tags key and value should match</returns>
        IFilterMetrics WhereTaggedWithKeyValue(TagKeyValueFilter tags);

        /// <summary>
        ///     Filters metrics by matching types
        /// </summary>
        /// <param name="types">The metric types to filter on.</param>
        /// <returns>A filter where metrics types should match</returns>
        IFilterMetrics WhereType(params MetricType[] types);
    }
}