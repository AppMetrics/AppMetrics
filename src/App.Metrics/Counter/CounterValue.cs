// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using App.Metrics.Tagging;

namespace App.Metrics.Counter
{
    public struct CounterValue
    {
        public static readonly IComparer<SetItem> SetItemComparer = Comparer<SetItem>.Create(
            (x, y) =>
            {
                var percent = Comparer<double>.Default.Compare(x.Percent, y.Percent);
                return percent == 0 ? Comparer<string>.Default.Compare(x.Item, y.Item) : percent;
            });

        private static readonly SetItem[] NoItems = new SetItem[0];

        public CounterValue(long count, SetItem[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Count = count;
            Items = items;
        }

        internal CounterValue(long count)
            : this(count, NoItems) { }

        /// <summary>
        ///     Gets the total count of the counter instance.
        /// </summary>
        /// <value>
        ///     The count.
        /// </value>
        public long Count { get; }

        /// <summary>
        ///     Gets counters for each registered set item.
        /// </summary>
        /// <value>
        ///     The counter's set items.
        /// </value>
        public SetItem[] Items { get; }

        public static bool operator ==(CounterValue left, CounterValue right) { return left.Equals(right); }

        public static bool operator !=(CounterValue left, CounterValue right) { return !left.Equals(right); }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is CounterValue && Equals((CounterValue)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Count.GetHashCode() * 397) ^ (Items?.GetHashCode() ?? 0);
            }
        }

        public bool Equals(CounterValue other) { return Count == other.Count && Equals(Items, other.Items); }

        public struct SetItem
        {
            public SetItem(string item, long count, double percent)
            {
                Item = item;
                Count = count;
                Percent = percent;
            }

            /// <summary>
            ///     Gets the specific count for this item.
            /// </summary>
            /// <value>
            ///     The count.
            /// </value>
            public long Count { get; }

            /// <summary>
            ///     Gets the registered item name.
            /// </summary>
            /// <value>
            ///     The item.
            /// </value>
            public string Item { get; }

            /// <summary>
            ///     Gets the percent of this item from the total count.
            /// </summary>
            /// <value>
            ///     The percent.
            /// </value>
            public double Percent { get; }

            public MetricTags Tags => MetricTags.FromSetItemString(Item);

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
                    var hashCode = Count.GetHashCode();
                    hashCode = (hashCode * 397) ^ (Item?.GetHashCode() ?? 0);
                    hashCode = (hashCode * 397) ^ Percent.GetHashCode();
                    return hashCode;
                }
            }

            public bool Equals(SetItem other) { return Count == other.Count && string.Equals(Item, other.Item) && Percent.Equals(other.Percent); }
        }
    }
}