// <copyright file="MetricSetItem.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Internal;

// ReSharper disable MemberCanBePrivate.Global
namespace App.Metrics
{
    /// <summary>
    ///     <para>
    ///         Metric items provide the ability to track either a count or rate for each item in a counters or meters finite
    ///         set
    ///         respectively. They also track the overall percentage of each item in the set.
    ///     </para>
    ///     <para>
    ///         This is useful for example if we needed to track the total number of emails sent but also the count of each
    ///         type of emails sent or The total rate of emails sent but also the rate at which type of email was sent.
    ///     </para>
    /// </summary>
    public struct MetricSetItem : IEquatable<MetricSetItem>
    {
        private readonly string _key;
        private readonly string[] _keys;
        private readonly string _value;
        private readonly string[] _values;

        public MetricSetItem(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "key cannot be null");
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "value cannot be null");
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentOutOfRangeException(nameof(key), "key cannot be empty or whitespace");
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value cannot be empty or whitespace");
            }

            _key = key;
            _value = value;
            _keys = null;
            _values = null;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MetricSetItem" /> struct.
        /// </summary>
        /// <param name="keys">The keys to use to generate a name for the set item.</param>
        /// <param name="values">The values to use to generate a name for the set item.</param>
        /// <exception cref="System.InvalidOperationException">
        ///     keys length must be equal to values length
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     keys cannot be null
        ///     or
        ///     values items cannot be null
        ///     or
        ///     keys cannot be empty
        ///     or
        ///     values cannot be empty
        /// </exception>
        public MetricSetItem(string[] keys, string[] values)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys), "keys cannot be null");
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(keys), "values cannot be null");
            }

            if (keys.Length != values.Length)
            {
                throw new InvalidOperationException($"{nameof(keys)} length must be equal to {nameof(values)} length");
            }

            if (keys.Any(t => t == null))
            {
                throw new InvalidOperationException($"{nameof(keys)} cannot have null values");
            }

            if (values.Any(t => t == null))
            {
                throw new InvalidOperationException($"{nameof(values)} cannot have null values");
            }

            if (keys.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidOperationException($"{nameof(keys)} cannot have empty values");
            }

            if (values.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidOperationException($"{nameof(values)} cannot have empty values");
            }

            _key = null;
            _value = null;
            _keys = keys;
            _values = values;
        }

        public int Count => _key != null ? 1 : (_keys?.Length ?? 0);

        public static bool Equals(MetricSetItem left, MetricSetItem right)
        {
            var count = left.Count;

            if (count != right.Count)
            {
                return false;
            }

            for (var i = 0; i < count; i++)
            {
                if (left._keys[i] != right._keys[i] || left._values[i] != right._values[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator ==(MetricSetItem left, MetricSetItem right) { return left.Equals(right); }

        public static bool operator !=(MetricSetItem left, MetricSetItem right) { return !left.Equals(right); }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return Equals(this, Empty);
            }

            if (obj is MetricSetItem)
            {
                return Equals(this, (MetricSetItem)obj);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (_keys == null)
            {
                return _key?.GetHashCode() ?? 0;
            }

            var hashCodeCombiner = HashCodeCombiner.Start();

            // ReSharper disable ForCanBeConvertedToForeach
            for (var i = 0; i < _keys.Length; i++)
            {
                // ReSharper restore ForCanBeConvertedToForeach

                hashCodeCombiner.Add(_keys[i], StringComparer.Ordinal);
            }

            return hashCodeCombiner.CombinedHash;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (_keys == null)
            {
                return string.Concat(_key, ":", _value);
            }

            switch (Count)
            {
                case 0:
                    return null;
                case 1:
                    return string.Concat(_keys[0], ":", _values[0]);
                default:
                    {
                    var sb = StringBuilderCache.Acquire();

                    for (var i = 0; i < _keys.Length; i++)
                    {
                        sb.Append(_keys[i]);
                        sb.Append(":");
                        sb.Append(_values[i]);

                        if (i < _keys.Length - 1)
                        {
                            sb.Append("|");
                        }
                    }

                    return StringBuilderCache.GetStringAndRelease(sb);
                }
            }
        }

        /// <inheritdoc />
        public bool Equals(MetricSetItem other)
        {
            return Equals(this, other);
        }

#pragma warning disable SA1202
        private static readonly string[] EmptyArray = new string[0];
        public static readonly MetricSetItem Empty = new MetricSetItem(EmptyArray, EmptyArray);
#pragma warning restore SA1202

        // ReSharper restore MemberCanBePrivate.Global
    }
}
