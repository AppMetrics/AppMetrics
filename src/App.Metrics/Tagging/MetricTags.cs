// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.PlatformAbstractions;

namespace App.Metrics.Tagging
{
    public struct MetricTags : IEquatable<MetricTags>
    {
        private readonly string _key;
        private readonly string[] _keys;
        private readonly string _value;
        private readonly string[] _values;

        public MetricTags(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _key = key;
            _value = value;
            _keys = null;
            _values = null;
        }

        public MetricTags(string[] keys, string[] values)
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
                throw new InvalidOperationException($"{nameof(keys)} keys cannot contains nulls");
            }

            if (values.Any(t => t == null))
            {
                throw new InvalidOperationException($"{nameof(values)} values cannot contains nulls");
            }

            if (keys.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidOperationException($"{nameof(keys)} keys cannot contains empty or whitespace strings");
            }

            if (values.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidOperationException($"{nameof(values)} keys cannot contains empty or whitespace strings");
            }

            _key = null;
            _value = null;
            _keys = keys;
            _values = values;
        }

        public int Count => _key != null ? 1 : (_keys?.Length ?? 0);

        public string[] Keys => _key != null ? new[] { _key } : _keys;

        public string[] Values => _value != null ? new[] { _value } : _values;

        public static MetricTags Concat(MetricTags tags1, MetricTags tags2)
        {
            var count1 = tags1.Count;
            var count2 = tags2.Count;

            if (count1 == 0)
            {
                return tags2;
            }

            if (count2 == 0)
            {
                return tags1;
            }

            var combinedKeys = new string[count1 + count2];
            var combinedValues = new string[count1 + count2];

            tags1.Keys.CopyTo(combinedKeys, 0);
            tags2.Keys.CopyTo(combinedKeys, count1);

            tags1.Values.CopyTo(combinedValues, 0);
            tags2.Values.CopyTo(combinedValues, count1);

            return new MetricTags(combinedKeys, combinedValues);
        }

        public static MetricTags Concat(MetricTags tags1, Dictionary<string, string> tags2)
        {
            var count1 = tags1.Count;
            var count2 = tags2?.Count ?? 0;

            if (count1 == 0)
            {
                return new MetricTags(tags2.Select(t => t.Key).ToArray(), tags2.Select(t => t.Value).ToArray());
            }

            if (count2 == 0)
            {
                return tags1;
            }

            var combinedKeys = new string[count1 + count2];
            var combinedValues = new string[count1 + count2];

            tags1.Keys.CopyTo(combinedKeys, 0);
            tags2.Select(t => t.Key).ToArray().CopyTo(combinedKeys, count1);

            tags1.Values.CopyTo(combinedValues, 0);
            tags2.Select(t => t.Value).ToArray().CopyTo(combinedValues, count1);

            return new MetricTags(combinedKeys, combinedValues);
        }

        public static bool Equals(MetricTags left, MetricTags right)
        {
            var count = left.Count;

            if (count != right.Count)
            {
                return false;
            }

            for (var i = 0; i < count; i++)
            {
                if (left.Keys[i] != right.Keys[i] || left.Values[i] != right.Values[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static MetricTags FromSetItemString(string setItem)
        {
            if (setItem.IsMissing())
            {
                return Empty;
            }

            var tagPairs = setItem.Split('|');

            if (tagPairs.Length <= 1)
            {
                return new MetricTags("item", setItem);
            }

            var tags = new string[tagPairs.Length];
            var values = new string[tagPairs.Length];

            for (var i = 0; i < tagPairs.Length; i++)
            {
                var tagKeyValue = tagPairs[i].Split(':');
                if (tagKeyValue.Length <= 1)
                {
                    tags[i] = "item";
                    values[i] = tagPairs[i];
                    continue;
                }

                tags[i] = tagKeyValue[0];
                values[i] = tagKeyValue[1];
            }

            return new MetricTags(tags, values);
        }

        public static bool operator ==(MetricTags left, MetricTags right) { return left.Equals(right); }

        public static bool operator !=(MetricTags left, MetricTags right) { return !left.Equals(right); }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return Equals(this, Empty);
            }

            if (obj is MetricTags)
            {
                return Equals(this, (MetricTags)obj);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (_key == null)
            {
                return _key?.GetHashCode() ?? 0;
            }

#pragma warning disable SA1129
            var hcc = new HashCodeCombiner();
#pragma warning restore SA1129

            for (var i = 0; i < _keys.Length; i++)
            {
                hcc.Add(_keys[i]);
            }

            return hcc.CombinedHash;
        }

        /// <inheritdoc />
        public bool Equals(MetricTags other)
        {
            return Equals(this, other);
        }

#pragma warning disable SA1202
        private static readonly string[] EmptyArray = new string[0];
        public static readonly MetricTags Empty = new MetricTags(EmptyArray, EmptyArray);
#pragma warning restore SA1202
    }
}