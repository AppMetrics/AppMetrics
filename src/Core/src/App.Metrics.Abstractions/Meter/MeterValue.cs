// <copyright file="MeterValue.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Meter
{
    /// <summary>
    ///     The value reported by a Meter Metric
    /// </summary>
    public sealed class MeterValue
    {
        public static readonly IComparer<SetItem> SetItemComparer = Comparer<SetItem>.Create(
            (x, y) =>
            {
                var percent = Comparer<double>.Default.Compare(x.Percent, y.Percent);
                return percent == 0 ? Comparer<string>.Default.Compare(x.Item, y.Item) : percent;
            });

        private static readonly SetItem[] NoItems = new SetItem[0];

        public MeterValue(
            long count,
            double meanRate,
            double oneMinuteRate,
            double fiveMinuteRate,
            double fifteenMinuteRate,
            TimeUnit rateUnit,
            SetItem[] items)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            Count = count;
            MeanRate = meanRate;
            OneMinuteRate = oneMinuteRate;
            FiveMinuteRate = fiveMinuteRate;
            FifteenMinuteRate = fifteenMinuteRate;
            RateUnit = rateUnit;
        }

        public MeterValue(
            long count,
            double meanRate,
            double oneMinuteRate,
            double fiveMinuteRate,
            double fifteenMinuteRate,
            TimeUnit rateUnit)
            : this(count, meanRate, oneMinuteRate, fiveMinuteRate, fifteenMinuteRate, rateUnit, NoItems)
        {
        }

        public long Count { get; }

        public double FifteenMinuteRate { get; }

        public double FiveMinuteRate { get; }

        public SetItem[] Items { get; }

        public double MeanRate { get; }

        public double OneMinuteRate { get; }

        public TimeUnit RateUnit { get; }

        public MeterValue Scale(TimeUnit unit)
        {
            if (unit == RateUnit)
            {
                return this;
            }

            var factor = unit.ScalingFactorFor(TimeUnit.Seconds);
            return new MeterValue(
                Count,
                MeanRate * factor,
                OneMinuteRate * factor,
                FiveMinuteRate * factor,
                FifteenMinuteRate * factor,
                unit,
                Items.Select(i => new SetItem(i.Item, i.Percent, i.Value.Scale(unit))).ToArray());
        }

        public struct SetItem
        {
            public SetItem(string item, double percent, MeterValue value)
            {
                Item = item;
                Percent = percent;
                Value = value;
            }

            public string Item { get; }

            public double Percent { get; }

            public MetricTags Tags => MetricTags.FromSetItemString(Item);

            public MeterValue Value { get; }

            public static bool operator ==(SetItem left, SetItem right) { return left.Equals(right); }

            public static bool operator !=(SetItem left, SetItem right) { return !left.Equals(right); }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                return obj is SetItem && Equals((SetItem)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Item?.GetHashCode() ?? 0;
                    hashCode = (hashCode * 397) ^ Percent.GetHashCode();
                    hashCode = (hashCode * 397) ^ (Value?.GetHashCode() ?? 0);
                    return hashCode;
                }
            }

            // ReSharper disable MemberCanBePrivate.Global
            public bool Equals(SetItem other)
                // ReSharper restore MemberCanBePrivate.Global
            {
                return string.Equals(Item, other.Item) && Percent.Equals(other.Percent) && Equals(Value, other.Value);
            }
        }
    }
}