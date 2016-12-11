// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Concurrent;
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
        public static readonly Dictionary<string, string> None = new Dictionary<string, string>();

        private static readonly Dictionary<string, string> Empty = new Dictionary<string, string>();
        private ConcurrentDictionary<string, string> _tags;

        public MetricTags(Dictionary<string, string> tags)
        {
            _tags = new ConcurrentDictionary<string, string>();

            foreach (var tag in tags)
            {
                if (!_tags.ContainsKey(tag.Key))
                {
                    _tags.TryAdd(tag.Key, tag.Value);
                }
            }            
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

            if (_tags == null)
            {
                return false;
            }

            return tags.ToDictionary().OrderBy(kvp => kvp.Key)
                .SequenceEqual(_tags.OrderBy(kvp => kvp.Key));
        }

        public override int GetHashCode()
        {
            return ToDictionary()?.GetHashCode() ?? 0;
        }

        public bool Equals(MetricTags other)
        {
            return Equals(_tags.ToDictionary(t => t.Key, t => t.Value), other.ToDictionary());
        }

        public Dictionary<string, string> ToDictionary()
        {
            return _tags != null ? _tags.ToDictionary(t => t.Key, t => t.Value) : Empty;
        }

        public MetricTags With(string tag, string value)
        {
            if (_tags == null)
            {
                _tags = new ConcurrentDictionary<string, string>();
            }

            if (!_tags.ContainsKey(tag))
            {
                _tags.TryAdd(tag, value);
            }

            return _tags.ToDictionary(t => t.Key, t => t.Value);
        }

        public MetricTags With(Dictionary<string, string> tags)
        {
            if (_tags == null)
            {
                _tags = new ConcurrentDictionary<string, string>(tags);
            }
            else
            {
                foreach (var tag in tags)
                {
                    if (!_tags.ContainsKey(tag.Key))
                    {
                        _tags.TryAdd(tag.Key, tag.Value);
                    }
                }
            }

            return _tags.ToDictionary(t => t.Key, t => t.Value);
        }
    }
}