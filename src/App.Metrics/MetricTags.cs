// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;

namespace App.Metrics
{
    /// <summary>
    ///     Collection of tags that can be attached to a metric.
    /// </summary>
    public class MetricTags : Dictionary<string, string>
    {
        public MetricTags(Dictionary<string, string> tags)
            : base(tags)
        {
        }

        public MetricTags()
        {
        }

        public static MetricTags None => new MetricTags();

        public MetricTags With(string tag, string value)
        {
            base[tag] = value;

            return this;
        }

        public MetricTags With(Dictionary<string, string> tags)
        {
            return new MetricTags(this.Concat(tags).ToDictionary(t => t.Key, t => t.Value));
        }
    }
}