// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Counter;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using Microsoft.DotNet.PlatformAbstractions;

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
        public static readonly MetricItem Empty = new MetricItem(EmptyArray, EmptyArray);

        private static readonly string[] EmptyArray = new string[0];
        private readonly string[] _tags;
        private readonly string[] _values;

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

            _tags = new[] { tag };
            _values = new[] { value };
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

            _tags = tags;
            _values = values;
        }

        public static bool operator ==(MetricItem left, MetricItem right) { return left.Equals(right); }

        public static bool operator !=(MetricItem left, MetricItem right) { return !left.Equals(right); }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return Equals(this, Empty);
            }

            if (obj is MetricItem)
            {
                return Equals(this, (MetricItem)obj);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (_tags == null)
            {
                return _tags?.GetHashCode() ?? 0;
            }
#pragma warning disable SA1129
            var hcc = new HashCodeCombiner();
#pragma warning restore SA1129

            for (var i = 0; i < _tags.Length; i++)
            {
                hcc.Add(_tags[i]);
            }

            return hcc.CombinedHash;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = StringBuilderCache.Acquire();

            if (_tags.Length == 1)
            {
                sb.Append(_tags[0]);
                sb.Append(":");
                sb.Append(_values[0]);

                return StringBuilderCache.GetStringAndRelease(sb);
            }

            for (var i = 0; i < _tags.Length; i++)
            {
                sb.Append(_tags[i]);
                sb.Append(":");
                sb.Append(_values[i]);

                if (i < _tags.Length - 1)
                {
                    sb.Append("|");
                }
            }

            return StringBuilderCache.GetStringAndRelease(sb);
        }

        /// <inheritdoc />
        public bool Equals(MetricItem other) { return Equals(this, other); }
    }
}