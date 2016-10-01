using System;
using System.Collections.Generic;
using App.Metrics.Utils;

namespace App.Metrics.MetricData
{
    public enum MetricType
    {
        Gauge,
        Counter,
        Meter,
        Histogram,
        Timer
    }

    public interface MetricsFilter : IHideObjectMembers
    {
        bool IsMatch(string context);

        bool IsMatch(GaugeValueSource gauge);

        bool IsMatch(CounterValueSource counter);

        bool IsMatch(MeterValueSource meter);

        bool IsMatch(HistogramValueSource histogram);

        bool IsMatch(TimerValueSource timer);
    }

    public class Filter : MetricsFilter
    {
        public static MetricsFilter All = new NoOpFilter();
        private Predicate<string> context;
        private Predicate<string> name;
        private HashSet<MetricType> types;

        private Filter()
        {
        }

        public static Filter New
        {
            get { return new Filter(); }
        }

        public bool IsMatch(string context)
        {
            if (this.context != null && !this.context(context))
            {
                return false;
            }

            return true;
        }

        public bool IsMatch(GaugeValueSource gauge)
        {
            if (types != null && !types.Contains(MetricType.Gauge))
            {
                return false;
            }
            return IsNameMatch(gauge.Name);
        }

        public bool IsMatch(CounterValueSource counter)
        {
            if (types != null && !types.Contains(MetricType.Counter))
            {
                return false;
            }
            return IsNameMatch(counter.Name);
        }

        public bool IsMatch(MeterValueSource meter)
        {
            if (types != null && !types.Contains(MetricType.Meter))
            {
                return false;
            }
            return IsNameMatch(meter.Name);
        }

        public bool IsMatch(HistogramValueSource histogram)
        {
            if (types != null && !types.Contains(MetricType.Histogram))
            {
                return false;
            }
            return IsNameMatch(histogram.Name);
        }

        public bool IsMatch(TimerValueSource timer)
        {
            if (types != null && !types.Contains(MetricType.Timer))
            {
                return false;
            }
            return IsNameMatch(timer.Name);
        }

        public Filter WhereContext(Predicate<string> condition)
        {
            this.context = condition;
            return this;
        }

        public Filter WhereContext(string context)
        {
            return WhereContext(c => c.Equals(context, StringComparison.OrdinalIgnoreCase));
        }

        public Filter WhereName(Predicate<string> condition)
        {
            this.name = condition;
            return this;
        }

        public Filter WhereNameStartsWith(string name)
        {
            return WhereName(n => n.StartsWith(name, StringComparison.OrdinalIgnoreCase));
        }

        public Filter WhereType(params MetricType[] types)
        {
            this.types = new HashSet<MetricType>(types);
            return this;
        }

        private bool IsNameMatch(string name)
        {
            if (this.name != null && !this.name(name))
            {
                return false;
            }

            return true;
        }

        private class NoOpFilter : MetricsFilter
        {
            public bool IsMatch(string context)
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
    }
}