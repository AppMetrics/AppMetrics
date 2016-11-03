// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using App.Metrics.Utils;

namespace App.Metrics
{
    //TODO: Review this and write unit tests for equality

    /// <summary>
    ///     Collection of tags that can be attached to a metric.
    /// </summary>
    public struct MetricTags : IHideObjectMembers
    {
        private static readonly Dictionary<string, string> Empty = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> None = new Dictionary<string, string>();

        private Dictionary<string, string> _tags;

        public MetricTags(Dictionary<string, string> tags)
        {
            _tags = tags ?? new Dictionary<string, string>();
        }

        public static bool operator ==(MetricTags left, MetricTags right)
        {
            return left.Equals(right);
        }

        public static implicit operator MetricTags(Dictionary<string, string> tags)
        {
            return new MetricTags(tags);
        }

        public static bool operator !=(MetricTags left, MetricTags right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MetricTags))
            {
                return false;
            }

            var tags = (MetricTags)obj;

            return tags.ToDictionary().OrderBy(kvp => kvp.Key)
                .SequenceEqual(_tags.OrderBy(kvp => kvp.Key));
        }

        public override int GetHashCode()
        {
            return _tags?.GetHashCode() ?? 0;
        }

        public bool Equals(MetricTags other)
        {
            return Equals(_tags, other._tags);
        }

        public Dictionary<string, string> ToDictionary()
        {
            return _tags ?? Empty;
        }

        public MetricTags With(string tag, string value)
        {
            if (_tags == null)
            {
                _tags = new Dictionary<string, string>();
            }

            _tags.Add(tag, value);
            return _tags;
        }
    }
}