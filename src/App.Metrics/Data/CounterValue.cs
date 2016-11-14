// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Collections.Generic;

namespace App.Metrics.Data
{
    public struct CounterValue
    {
        public static readonly IComparer<SetItem> SetItemComparer = Comparer<SetItem>.Create((x, y) =>
        {
            var percent = Comparer<double>.Default.Compare(x.Percent, y.Percent);
            return percent == 0 ? Comparer<string>.Default.Compare(x.Item, y.Item) : percent;
        });

        /// <summary>
        ///     Total count of the counter instance.
        /// </summary>
        public readonly long Count;

        /// <summary>
        ///     Separate counters for each registered set item.
        /// </summary>
        public readonly SetItem[] Items;

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

        internal CounterValue(long count) : this(count, NoItems)
        {
        }

        public static bool operator ==(CounterValue left, CounterValue right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CounterValue left, CounterValue right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CounterValue && Equals((CounterValue)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Count.GetHashCode() * 397) ^ (Items != null ? Items.GetHashCode() : 0);
            }
        }

        public bool Equals(CounterValue other)
        {
            return Count == other.Count && Equals(Items, other.Items);
        }


        public struct SetItem
        {
            /// <summary>
            ///     Specific count for this item.
            /// </summary>
            public readonly long Count;

            /// <summary>
            ///     Registered item name.
            /// </summary>
            public readonly string Item;

            /// <summary>
            ///     Percent of this item from the total count.
            /// </summary>
            public readonly double Percent;

            public SetItem(string item, long count, double percent)
            {
                Item = item;
                Count = count;
                Percent = percent;
            }

            public static bool operator ==(SetItem left, SetItem right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(SetItem left, SetItem right)
            {
                return !left.Equals(right);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is SetItem && Equals((SetItem)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Count.GetHashCode();
                    hashCode = (hashCode * 397) ^ (Item != null ? Item.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ Percent.GetHashCode();
                    return hashCode;
                }
            }

            public bool Equals(SetItem other)
            {
                return Count == other.Count && string.Equals(Item, other.Item) && Percent.Equals(other.Percent);
            }
        }
    }
}