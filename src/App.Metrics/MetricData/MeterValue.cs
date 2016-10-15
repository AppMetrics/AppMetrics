using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.MetricData
{
    /// <summary>
    ///     The value reported by a Meter Metric
    /// </summary>
    public sealed class MeterValue
    {
        public static readonly IComparer<SetItem> SetItemComparer = Comparer<SetItem>.Create((x, y) =>
        {
            var percent = Comparer<double>.Default.Compare(x.Percent, y.Percent);
            return percent == 0 ? Comparer<string>.Default.Compare(x.Item, y.Item) : percent;
        });

        public readonly long Count;
        public readonly double FifteenMinuteRate;
        public readonly double FiveMinuteRate;
        public readonly SetItem[] Items;
        public readonly double MeanRate;
        public readonly double OneMinuteRate;
        public readonly TimeUnit RateUnit;
        private static readonly SetItem[] NoItems = new SetItem[0];

        public MeterValue(long count, double meanRate, double oneMinuteRate, double fiveMinuteRate, double fifteenMinuteRate, TimeUnit rateUnit,
            SetItem[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Count = count;
            MeanRate = meanRate;
            OneMinuteRate = oneMinuteRate;
            FiveMinuteRate = fiveMinuteRate;
            FifteenMinuteRate = fifteenMinuteRate;
            RateUnit = rateUnit;
            Items = items;
        }

        internal MeterValue(long count, double meanRate, double oneMinuteRate, double fiveMinuteRate, double fifteenMinuteRate, TimeUnit rateUnit)
            : this(count, meanRate, oneMinuteRate, fiveMinuteRate, fifteenMinuteRate, rateUnit, NoItems)
        {
        }

        public MeterValue Scale(TimeUnit unit)
        {
            if (unit == RateUnit)
            {
                return this;
            }

            var factor = unit.ScalingFactorFor(TimeUnit.Seconds);
            return new MeterValue(Count,
                MeanRate * factor,
                OneMinuteRate * factor,
                FiveMinuteRate * factor,
                FifteenMinuteRate * factor,
                unit,
                Items.Select(i => new SetItem(i.Item, i.Percent, i.Value.Scale(unit))).ToArray());
        }

        public struct SetItem
        {
            public readonly string Item;
            public readonly double Percent;
            public readonly MeterValue Value;

            public SetItem(string item, double percent, MeterValue value)
            {
                Item = item;
                Percent = percent;
                Value = value;
            }
        }
    }

    /// <summary>
    ///     Combines the value of the meter with the defined unit and the rate unit at which the value is reported.
    /// </summary>
    public sealed class MeterValueSource : MetricValueSource<MeterValue>
    {
        public MeterValueSource(string name, IMetricValueProvider<MeterValue> value, Unit unit, TimeUnit rateUnit, MetricTags tags)
            : base(name, new ScaledValueProvider<MeterValue>(value, v => v.Scale(rateUnit)), unit, tags)
        {
            RateUnit = rateUnit;
        }

        public TimeUnit RateUnit { get; private set; }
    }
}