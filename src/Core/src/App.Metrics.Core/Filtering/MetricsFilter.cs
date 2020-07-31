// <copyright file="MetricsFilter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Filters;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Filtering
{
    /// <summary>
    ///     Provides the ability to filter metrics by Context, Type, Name etc.
    /// </summary>
    /// <seealso cref="IFilterMetrics" />
    public sealed class MetricsFilter : IFilterMetrics
    {
        private Predicate<string> _context;
        private Predicate<string> _name;
        private HashSet<string> _tagKeys;
        private Dictionary<string, string> _tags;
        private HashSet<MetricType> _types;

        /// <inheritdoc />
        public bool IsApdexMatch(ApdexValueSource apdex)
        {
            if (_types != null && !_types.Contains(MetricType.Apdex))
            {
                return false;
            }

            return IsMetricNameMatch(apdex.Name) && IsTagMatch(apdex.Tags);
        }

        /// <inheritdoc />
        public bool IsCounterMatch(CounterValueSource counter)
        {
            if (_types != null && !_types.Contains(MetricType.Counter))
            {
                return false;
            }

            return IsMetricNameMatch(counter.Name) && IsTagMatch(counter.Tags);
        }

        /// <inheritdoc />
        public bool IsGaugeMatch(GaugeValueSource gauge)
        {
            if (_types != null && !_types.Contains(MetricType.Gauge))
            {
                return false;
            }

            return IsMetricNameMatch(gauge.Name) && IsTagMatch(gauge.Tags);
        }

        /// <inheritdoc />
        public bool IsHistogramMatch(HistogramValueSource histogram)
        {
            if (_types != null && !_types.Contains(MetricType.Histogram))
            {
                return false;
            }

            return IsMetricNameMatch(histogram.Name) && IsTagMatch(histogram.Tags);
        }

        /// <inheritdoc />
        public bool IsBucketHistogramMatch(BucketHistogramValueSource histogram)
        {
            if (_types != null && !_types.Contains(MetricType.Histogram))
            {
                return false;
            }

            return IsMetricNameMatch(histogram.Name) && IsTagMatch(histogram.Tags);
        }

        /// <inheritdoc />
        public bool IsContextMatch(string context)
        {
            return _context == null || _context(context);
        }

        /// <inheritdoc />
        public bool IsMeterMatch(MeterValueSource meter)
        {
            if (_types != null && !_types.Contains(MetricType.Meter))
            {
                return false;
            }

            return IsMetricNameMatch(meter.Name) && IsTagMatch(meter.Tags);
        }

        /// <inheritdoc />
        public bool IsTimerMatch(TimerValueSource timer)
        {
            if (_types != null && !_types.Contains(MetricType.Timer))
            {
                return false;
            }

            return IsMetricNameMatch(timer.Name) && IsTagMatch(timer.Tags);
        }

        /// <inheritdoc />
        public bool IsBucketTimerMatch(BucketTimerValueSource timer)
        {
            if (_types != null && !_types.Contains(MetricType.Timer))
            {
                return false;
            }

            return IsMetricNameMatch(timer.Name) && IsTagMatch(timer.Tags);
        }

        /// <inheritdoc />
        public IFilterMetrics WhereContext(Predicate<string> condition)
        {
            _context = condition;
            return this;
        }

        /// <inheritdoc />
        public IFilterMetrics WhereContext(string context)
        {
            return WhereContext(c => c.Equals(context, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        public IFilterMetrics WhereName(string name)
        {
            return WhereName(c => c.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        public IFilterMetrics WhereName(Predicate<string> condition)
        {
            _name = condition;
            return this;
        }

        /// <inheritdoc />
        public IFilterMetrics WhereNameStartsWith(string name)
        {
            return WhereName(n => n.StartsWith(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        public IFilterMetrics WhereTaggedWithKey(params string[] tagKeys)
        {
            _tagKeys = new HashSet<string>(tagKeys);
            return this;
        }

        /// <inheritdoc />
        public IFilterMetrics WhereTaggedWithKeyValue(TagKeyValueFilter tags)
        {
            _tags = tags;
            return this;
        }

        /// <inheritdoc />
        public IFilterMetrics WhereType(params MetricType[] types)
        {
            _types = new HashSet<MetricType>(types);
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

            if (_tags != null && Array.Exists(_tags.ToArray(), t => sourceTags.Keys.Any(m => m == t.Key)))
            {
                isMatch = true;
            }

            return isMatch;
        }
    }
}