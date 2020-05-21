// <copyright file="MetricsOptionsExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Internal.Extensions
{
    public static class MetricsOptionsExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValue(this MetricsOptions options)
        {
            var result = new Dictionary<string, string>
                   {
                       [KeyValuePairMetricsOptions.DefaultContextLabelDirective] = options.DefaultContextLabel,
                       [KeyValuePairMetricsOptions.EnabledDirective] = options.Enabled.ToString()
                   };

            foreach (var globalTag in options.GlobalTags)
            {
                var key = $"{KeyValuePairMetricsOptions.GlobalTagsDirective}:{globalTag.Key}";

                if (!result.ContainsKey(key))
                {
                    result.Add(key, globalTag.Value);
                }
            }

            foreach (var contextualTag in options.ContextualTags)
            {
                var key = $"{KeyValuePairMetricsOptions.ContextualTagsDirective}:{contextualTag.Key}";

                if (!result.ContainsKey(key))
                {
                    result.Add(key, "<computed>");
                }
            }

            return result;
        }
    }
}
