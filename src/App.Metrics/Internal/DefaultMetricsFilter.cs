// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;

namespace App.Metrics.Internal
{
    public sealed class DefaultMetricsFilter : IMetricsFilter
    {
        public static IMetricsFilter All = new NullMetricsFilter();
        private Predicate<string> _context;
        private Predicate<string> _name;
        private HashSet<string> _tagKeys;
        private HashSet<MetricType> _types;

        public bool IsMatch(string context)
        {
            return _context == null || _context(context);
        }

        public bool IsMatch(GaugeValueSource gauge)
        {
            if (_types != null && !_types.Contains(MetricType.Gauge))
            {
                return false;
            }



            return IsNameMatch(gauge.Name) && IsTagMatch(gauge.Tags);
        }

        public bool IsMatch(CounterValueSource counter)
        {
            if (_types != null && !_types.Contains(MetricType.Counter))
            {
                return false;
            }

            return IsNameMatch(counter.Name) && IsTagMatch(counter.Tags);
        }

        public bool IsMatch(MeterValueSource meter)
        {
            if (_types != null && !_types.Contains(MetricType.Meter))
            {
                return false;
            }

            return IsNameMatch(meter.Name) && IsTagMatch(meter.Tags);
        }

        public bool IsMatch(HistogramValueSource histogram)
        {
            if (_types != null && !_types.Contains(MetricType.Histogram))
            {
                return false;
            }
            return IsNameMatch(histogram.Name) && IsTagMatch(histogram.Tags);
        }

        public bool IsMatch(TimerValueSource timer)
        {
            if (_types != null && !_types.Contains(MetricType.Timer))
            {
                return false;
            }

            return IsNameMatch(timer.Name) && IsTagMatch(timer.Tags);
        }

        public bool ReportEnvironment { get; private set; }

        public bool ReportHealthChecks { get; private set; }

        public DefaultMetricsFilter WithHealthChecks(bool report)
        {
            ReportHealthChecks = report;
            return this;
        }

        public DefaultMetricsFilter WithEnvironmentInfo(bool report)
        {
            ReportEnvironment = report;
            return this;
        }

        public DefaultMetricsFilter WhereContext(Predicate<string> condition)
        {
            _context = condition;
            return this;
        }

        public DefaultMetricsFilter WhereContext(string context)
        {
            return WhereContext(c => c.Equals(context, StringComparison.OrdinalIgnoreCase));
        }

        public DefaultMetricsFilter WhereName(Predicate<string> condition)
        {
            _name = condition;
            return this;
        }

        public DefaultMetricsFilter WhereTaggedWith(params string[] tagKeys)
        {
            _tagKeys = new HashSet<string>(tagKeys);
            return this;
        }

        public DefaultMetricsFilter WhereNameStartsWith(string name)
        {
            return WhereName(n => n.StartsWith(name, StringComparison.OrdinalIgnoreCase));
        }

        public DefaultMetricsFilter WhereType(params MetricType[] types)
        {
            _types = new HashSet<MetricType>(types);
            return this;
        }

        private bool IsNameMatch(string name)
        {
            return _name == null || _name(name);
        }

        private bool IsTagMatch(MetricTags sourceTags)
        {
            var keys = sourceTags.ToDictionary().Keys;
            return _tagKeys == null || Array.Exists(_tagKeys.ToArray(), t => keys.Any(m => m == t));
        }
    }
}