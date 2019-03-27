// <copyright file="MetricTagExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Formatters.Prometheus.Internal.Extensions
{
    public static class MetricTagExtensions
    {
        public static List<LabelPair> ToLabelPairs(this MetricTags tags)
        {
            var result = new List<LabelPair>(tags.Count);
            for (var i = 0; i < tags.Count; i++)
            {
                result.Add(new LabelPair { name = tags.Keys[i], value = tags.Values[i] });
            }

            return result;
        }
    }
}
