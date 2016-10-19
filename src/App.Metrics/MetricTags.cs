// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Utils;

namespace App.Metrics
{
    /// <summary>
    ///     Collection of tags that can be attached to a metric.
    /// </summary>
    public struct MetricTags : IHideObjectMembers
    {
        public static readonly MetricTags None = new MetricTags(Enumerable.Empty<string>());
        private static readonly string[] Empty = new string[0];

        private readonly string[] _tags;

        public MetricTags(params string[] tags)
        {
            _tags = tags.ToArray();
        }

        public MetricTags(IEnumerable<string> tags)
            : this(tags.ToArray())
        {
        }

        public MetricTags(string commaSeparatedTags)
            : this(ToTags(commaSeparatedTags))
        {
        }

        public string[] Tags => _tags ?? Empty;

        public static implicit operator MetricTags(string commaSeparatedTags)
        {
            return new MetricTags(commaSeparatedTags);
        }

        public static implicit operator MetricTags(string[] tags)
        {
            return new MetricTags(tags);
        }

        private static IEnumerable<string> ToTags(string commaSeparatedTags)
        {
            if (string.IsNullOrWhiteSpace(commaSeparatedTags))
            {
                return Enumerable.Empty<string>();
            }

            return commaSeparatedTags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim().ToLowerInvariant());
        }
    }
}