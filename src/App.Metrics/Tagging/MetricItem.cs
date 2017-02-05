// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Metrics.Counter;
using App.Metrics.Meter;

namespace App.Metrics.Tagging
{
    /// <summary>
    ///     <para>
    ///         Metric items can be used with <see cref="CounterMetric" /> or <see cref="MeterMetric" />
    ///         <see cref="MetricType" />s
    ///     </para>
    ///     <para>
    ///         They provide the ability to track either a count or rate for each item in a counters or meters finite set
    ///         respectively. They also track the overall percentage of each item in the set.
    ///     </para>
    ///     <para>
    ///         This is useful for example if we needed to track the total number of emails sent but also the count of each
    ///         type of emails sent or The total rate of emails sent but also the rate at which type of email was sent.
    ///     </para>
    /// </summary>
    public struct MetricItem : IEquatable<MetricItem>
    {
        public MetricItem(string tag, string value)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentOutOfRangeException(nameof(tag));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            Tags = new[] { tag };
            Values = new[] { value };
        }

        public MetricItem(string[] tags, string[] values)
        {
            if (tags.Length != values.Length)
            {
                throw new InvalidOperationException($"{nameof(tags)} length must be equal to {nameof(values)} length");
            }

            if (tags.Any(t => t == null))
            {
                throw new ArgumentNullException(nameof(tags), "tag items cannot be null");
            }

            if (values.Any(t => t == null))
            {
                throw new ArgumentNullException(nameof(values), "value items cannot be null");
            }

            if (tags.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentNullException(nameof(tags), "tag items cannot be empty");
            }

            if (values.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentNullException(nameof(values), "value items cannot be empty");
            }

            Tags = tags;
            Values = values;
        }

        public static IEqualityComparer<MetricItem> TagComparer { get; } = new TagEqualityComparer();

        public string[] Tags { get; }

        public string[] Values { get; }

        public static bool operator ==(MetricItem left, MetricItem right) { return left.Equals(right); }

        public static bool operator !=(MetricItem left, MetricItem right) { return !left.Equals(right); }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is MetricItem && Equals((MetricItem)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Tags?.GetHashCode() ?? 0) * 397) ^ (Values?.GetHashCode() ?? 0);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Tags.Length == 1)
            {
                return $"{Tags[0]}:{Values[0]}";
            }

            var sb = new StringBuilder();

            for (var i = 0; i < Tags.Length; i++)
            {
                sb.Append($"{Tags[i]}:{Values[i]}|");
            }

            return sb.ToString().TrimEnd('|');
        }

        /// <inheritdoc />
        public bool Equals(MetricItem other)
        {
            var tags = string.Join(string.Empty, Tags);
            var values = string.Join(string.Empty, Values);
            var otherTags = string.Join(string.Empty, other.Tags);
            var otherValues = string.Join(string.Empty, other.Values);

            return string.Equals(tags, otherTags) && string.Equals(values, otherValues);
        }

        private sealed class TagEqualityComparer : IEqualityComparer<MetricItem>
        {
            public bool Equals(MetricItem x, MetricItem y)
            {
                var firstTags = string.Join(string.Empty, x);
                var secondTags = string.Join(string.Empty, y);

                return string.Equals(firstTags, secondTags);
            }

            public int GetHashCode(MetricItem obj) { return obj.Tags?.GetHashCode() ?? 0; }
        }
    }
}