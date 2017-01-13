// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;

namespace App.Metrics
{
    /// <summary>
    ///     Provides the ability to filter metrics by Context, Type, Name etc.
    /// </summary>
    /// <seealso cref="App.Metrics.IMetricsFilter" />
    public sealed class DefaultMetricsFilter : IMetricsFilter
    {
        private Predicate<string> _context;
        private Predicate<string> _name;
        private HashSet<string> _tagKeys;
        private Dictionary<string, string> _tags;
        private HashSet<MetricType> _types;

        public DefaultMetricsFilter()
        {
            ReportHealthChecks = true;
            ReportEnvironment = true;
        }

        /// <summary>
        ///     Gets a value indicating whether [report environment].
        /// </summary>
        /// <remarks>
        ///     If <c>false</c> when metrics data is retrieved the environment information will not be populated
        /// </remarks>
        /// <value>
        ///     <c>true</c> if [report environment]; otherwise, <c>false</c>.
        /// </value>
        public bool ReportEnvironment { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [report health checks].
        /// </summary>
        /// <remarks>
        ///     If <c>false</c> when metrics data is retrieved the health information will not be populated
        /// </remarks>
        /// <value>
        ///     <c>true</c> if [report health checks]; otherwise, <c>false</c>.
        /// </value>
        public bool ReportHealthChecks { get; private set; }

        /// <summary>
        ///     Determines whether the specified context is match.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>true if the context matches</returns>
        public bool IsMatch(string context)
        {
            return _context == null || _context(context);
        }

        /// <summary>
        ///     Determines whether the specified gauge is match.
        /// </summary>
        /// <param name="gauge">The gauge.</param>
        /// <returns>True if the metric type is a gauge, the name matches and tags match</returns>
        public bool IsMatch(GaugeValueSource gauge)
        {
            if (_types != null && !_types.Contains(MetricType.Gauge))
            {
                return false;
            }

            return IsMetricNameMatch(gauge.Name) && IsTagMatch(gauge.Tags);
        }

        /// <summary>
        ///     Determines whether the specified counter is match.
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>True if the metric type is a counter, the name matches and tags match</returns>
        public bool IsMatch(CounterValueSource counter)
        {
            if (_types != null && !_types.Contains(MetricType.Counter))
            {
                return false;
            }

            return IsMetricNameMatch(counter.Name) && IsTagMatch(counter.Tags);
        }

        /// <summary>
        ///     Determines whether the specified meter is match.
        /// </summary>
        /// <param name="meter">The meter.</param>
        /// <returns>True if the metric type is a meter, the name matches and tags match</returns>
        public bool IsMatch(MeterValueSource meter)
        {
            if (_types != null && !_types.Contains(MetricType.Meter))
            {
                return false;
            }

            return IsMetricNameMatch(meter.Name) && IsTagMatch(meter.Tags);
        }

        /// <summary>
        ///     Determines whether the specified histogram is match.
        /// </summary>
        /// <param name="histogram">The histogram.</param>
        /// <returns>True if the metric type is a histogram, the name matches and tags match</returns>
        public bool IsMatch(HistogramValueSource histogram)
        {
            if (_types != null && !_types.Contains(MetricType.Histogram))
            {
                return false;
            }

            return IsMetricNameMatch(histogram.Name) && IsTagMatch(histogram.Tags);
        }

        /// <summary>
        ///     Determines whether the specified timer is match.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <returns>True if the metric type is a timer, the name matches and tags match</returns>
        public bool IsMatch(TimerValueSource timer)
        {
            if (_types != null && !_types.Contains(MetricType.Timer))
            {
                return false;
            }

            return IsMetricNameMatch(timer.Name) && IsTagMatch(timer.Tags);
        }

        /// <summary>
        ///     Determines whether the specified apdex is match.
        /// </summary>
        /// <param name="apdex">The apdex.</param>
        /// <returns>True if the metric type is an apdex, the name matches and tags match</returns>
        public bool IsMatch(ApdexValueSource apdex)
        {
            if (_types != null && !_types.Contains(MetricType.Apdex))
            {
                return false;
            }

            return IsMetricNameMatch(apdex.Name) && IsTagMatch(apdex.Tags);
        }

        /// <summary>
        ///     Filters metrics where the specified predicate on the metrics context is <c>true</c>
        /// </summary>
        /// <param name="condition">The predicate on the context to filter on.</param>
        /// <returns>A filter where the metric context should match</returns>
        public DefaultMetricsFilter WhereContext(Predicate<string> condition)
        {
            _context = condition;
            return this;
        }

        /// <summary>
        ///     Filters metrics where the specified context matches
        /// </summary>
        /// <param name="context">The metrics context to filter on.</param>
        /// <returns>A filter where the metric context should match</returns>
        public DefaultMetricsFilter WhereContext(string context)
        {
            return WhereContext(c => c.Equals(context, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///     Filters metrics where the specified predicate on the metric name is <c>true</c>
        /// </summary>
        /// <param name="condition">The predicate on the metric name to filter on.</param>
        /// <returns>A filter where the metric name should match</returns>
        public DefaultMetricsFilter WhereMetricName(Predicate<string> condition)
        {
            _name = condition;
            return this;
        }

        /// <summary>
        ///     Filters metrics where the metric name starts with the specified name
        /// </summary>
        /// <param name="name">The metrics name to filter on.</param>
        /// <returns>A filter where the metric name starts with the specified name</returns>
        public DefaultMetricsFilter WhereMetricNameStartsWith(string name)
        {
            return WhereMetricName(n => n.StartsWith(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///     Filters metrics where the metrics contain the specified tags keys
        /// </summary>
        /// <param name="tagKeys">The metrics tag keys to filter on.</param>
        /// <returns>A filter where the metric tags keys should match</returns>
        public DefaultMetricsFilter WhereMetricTaggedWithKey(params string[] tagKeys)
        {
            _tagKeys = new HashSet<string>(tagKeys);
            return this;
        }

        /// <summary>
        ///     Filters metrics where the metrics contain the specified tags key/value pair
        /// </summary>
        /// <param name="tags">The metrics tag key/values to filter on.</param>
        /// <returns>A filter where the metric tags key and value should match</returns>
        public DefaultMetricsFilter WhereMetricTaggedWithKeyValue(TagKeyValueFilter tags)
        {
            _tags = tags;
            return this;
        }

        /// <summary>
        ///     Fitlers metrics by matching types
        /// </summary>
        /// <param name="types">The metric types to filter on.</param>
        /// <returns>A filter where metrics types should match</returns>
        public DefaultMetricsFilter WhereType(params MetricType[] types)
        {
            _types = new HashSet<MetricType>(types);
            return this;
        }

        public DefaultMetricsFilter WithEnvironmentInfo(bool report)
        {
            ReportEnvironment = report;
            return this;
        }

        public DefaultMetricsFilter WithHealthChecks(bool report)
        {
            ReportHealthChecks = report;
            return this;
        }

        private bool IsMetricNameMatch(string name) { return _name == null || _name(name); }

        private bool IsTagMatch(MetricTags sourceTags)
        {
            var isMatch = false;

            if ((_tagKeys == null || !_tagKeys.Any()) && (_tags == null || !_tags.Any()))
            {
                return true;
            }

            if (_tagKeys != null && Array.Exists(_tagKeys.ToArray(), t => sourceTags.Keys.Any(m => m == t)))
            {
                isMatch = true;
            }

            if (_tags != null && Array.Exists(_tags.ToArray(), t => sourceTags.Any(m => m.Key == t.Key && m.Value == t.Value)))
            {
                isMatch = true;
            }

            return isMatch;
        }
    }
}