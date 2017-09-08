// <copyright file="MetricsOptionsExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Internal.Extensions
{
    public static class MetricsOptionsExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValue(this MetricsOptions options)
        {
            return new Dictionary<string, string>
                   {
                       [KeyValuePairMetricsOptions.DefaultContextLabelDirective] = options.DefaultContextLabel,
                       [KeyValuePairMetricsOptions.EnabledDirective] = options.Enabled.ToString(),
                       [KeyValuePairMetricsOptions.GlobalTagsDirective] = string.Join(", ", options.GlobalTags.Select(t => $"{t.Key}={t.Value}"))
                   };
        }
    }
}
