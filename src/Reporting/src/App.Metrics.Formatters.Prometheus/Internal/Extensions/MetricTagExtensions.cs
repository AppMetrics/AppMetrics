// <copyright file="MetricTagExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace App.Metrics.Formatters.Prometheus.Internal.Extensions
{
    public static class MetricTagExtensions
    {
        public static List<LabelPair> ToLabelPairs(this MetricTags tags, Func<string, string> labelNameFormatter)
        {
            var result = new List<LabelPair>(tags.Count);
            for (var i = 0; i < tags.Count; i++)
            {
                var formattedName = labelNameFormatter(tags.Keys[i]);
                result.Add(new LabelPair {name = formattedName, value = tags.Values[i]});
            }

            return result;
        }
    }
}