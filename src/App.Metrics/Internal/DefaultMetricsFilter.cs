// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Internal
{
    internal sealed class NoOpFilter : IMetricsFilter
    {
        public bool IsMatch(string @group)
        {
            return true;
        }

        public bool IsMatch(GaugeValueSource gauge)
        {
            return true;
        }

        public bool IsMatch(CounterValueSource counter)
        {
            return true;
        }

        public bool IsMatch(MeterValueSource meter)
        {
            return true;
        }

        public bool IsMatch(HistogramValueSource histogram)
        {
            return true;
        }

        public bool IsMatch(TimerValueSource timer)
        {
            return true;
        }
    }

    internal sealed class DefaultMetricsFilter : IMetricsFilter
    {
        public static IMetricsFilter All = new NoOpFilter();
        private Predicate<string> _group;
        private Predicate<string> _name;
        private HashSet<MetricType> _types;

        public bool IsMatch(string group)
        {
            return _group == null || _group(group);
        }

        public bool IsMatch(GaugeValueSource gauge)
        {
            if (_types != null && !_types.Contains(MetricType.Gauge))
            {
                return false;
            }
            return IsNameMatch(gauge.Name);
        }

        public bool IsMatch(CounterValueSource counter)
        {
            if (_types != null && !_types.Contains(MetricType.Counter))
            {
                return false;
            }
            return IsNameMatch(counter.Name);
        }

        public bool IsMatch(MeterValueSource meter)
        {
            if (_types != null && !_types.Contains(MetricType.Meter))
            {
                return false;
            }
            return IsNameMatch(meter.Name);
        }

        public bool IsMatch(HistogramValueSource histogram)
        {
            if (_types != null && !_types.Contains(MetricType.Histogram))
            {
                return false;
            }
            return IsNameMatch(histogram.Name);
        }

        public bool IsMatch(TimerValueSource timer)
        {
            if (_types != null && !_types.Contains(MetricType.Timer))
            {
                return false;
            }
            return IsNameMatch(timer.Name);
        }

        public DefaultMetricsFilter WhereGroup(Predicate<string> condition)
        {
            _group = condition;
            return this;
        }

        public DefaultMetricsFilter WhereGroup(string group)
        {
            return WhereGroup(c => c.Equals(group, StringComparison.OrdinalIgnoreCase));
        }

        public DefaultMetricsFilter WhereName(Predicate<string> condition)
        {
            _name = condition;
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
    }
}