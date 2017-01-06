// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace App.Metrics
{
    /// <summary>
    ///     Collection of tags that can be attached to a metric.
    /// </summary>
    public class MetricTags : ConcurrentDictionary<string, string>
    {
        public MetricTags(Dictionary<string, string> tags)
            : base(tags)
        {
        }

        public MetricTags()
        {
        }

        public static MetricTags None => new MetricTags();

        public MetricTags FromSetItemString(string setItem)
        {
            var tags = new MetricTags();

            if (setItem.IsMissing())
            {
                return None;
            }

            var tagPairs = setItem.Split('|');

            if (tagPairs.Length <= 1)
            {
                return tags.With("item", setItem);
            }

            foreach (var keyValue in tagPairs)
            {
                var tagKeyValue = keyValue.Split(':');
                if (tagKeyValue.Length <= 1)
                {
                    tags.With("item", keyValue);
                    continue;
                }

                tags.With(tagKeyValue[0], tagKeyValue[1]);
            }

            return tags;
        }

        public MetricTags With(string tag, string value)
        {
            TryAdd(tag, value);

            return this;
        }
    }
}