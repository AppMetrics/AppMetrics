// <copyright file="KeyValuePairMetricsOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Internal
{
    internal class KeyValuePairMetricsOptions
    {
        internal static readonly string DefaultContextLabelDirective = $"{nameof(MetricsOptions)}:{nameof(MetricsOptions.DefaultContextLabel)}";
        internal static readonly string EnabledDirective = $"{nameof(MetricsOptions)}:{nameof(MetricsOptions.Enabled)}";
        internal static readonly string ReportingEnabledDirective = $"{nameof(MetricsOptions)}:{nameof(MetricsOptions.ReportingEnabled)}";
        internal static readonly string GlobalTagsDirective = $"{nameof(MetricsOptions)}:{nameof(MetricsOptions.GlobalTags)}:";
        internal static readonly string ContextualTagsDirective = $"{nameof(MetricsOptions)}:{nameof(MetricsOptions.ContextualTags)}:";
        private readonly MetricsOptions _options;

        private readonly Dictionary<string, string> _optionValues;

        public KeyValuePairMetricsOptions(MetricsOptions options, IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            _options = options;
            _optionValues = optionValues.ToDictionary(o => o.Key, o => o.Value);
        }

        public KeyValuePairMetricsOptions(IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            _optionValues = optionValues.ToDictionary(o => o.Key, o => o.Value);
        }

        public MetricsOptions AsOptions(bool mergeTags = false)
        {
            var options = _options ?? new MetricsOptions();

            if (!mergeTags)
            {
                options.GlobalTags = new GlobalMetricTags();
            }

            foreach (var key in _optionValues.Keys)
            {
                if (string.Compare(key, DefaultContextLabelDirective, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    options.DefaultContextLabel = _optionValues[key];
                }
                else if (key.StartsWith(GlobalTagsDirective, StringComparison.CurrentCultureIgnoreCase))
                {
                    var tagKey = key.Split(':').LastOrDefault()?.Trim();
                    var tagValue = _optionValues[key]?.Split(new[] { ",", ", " }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault()?.Trim();

                    if (string.IsNullOrWhiteSpace(tagKey) || string.IsNullOrWhiteSpace(tagValue))
                    {
                        throw new InvalidOperationException(
                            $"Attempted to bind {key} to {GlobalTagsDirective} but the value was not property formatted, format as: value='[MetricsOptions:GlobalTags:tagKey, tagValue]");
                    }

                    if (options.GlobalTags.ContainsKey(tagKey))
                    {
                        options.GlobalTags[tagKey] = tagValue;
                    }
                    else
                    {
                        options.GlobalTags.Add(tagKey, tagValue);
                    }
                }
                else if (string.Compare(key, EnabledDirective, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    if (!bool.TryParse(_optionValues[key], out var metricsEnabled))
                    {
                        throw new InvalidCastException($"Attempted to bind {key} to {EnabledDirective} but it's not a boolean");
                    }

                    options.Enabled = metricsEnabled;
                }
                else if (string.Compare(key, ReportingEnabledDirective, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    if (!bool.TryParse(_optionValues[key], out var reportingEnabled))
                    {
                        throw new InvalidCastException($"Attempted to bind {key} to {ReportingEnabledDirective} but it's not a boolean");
                    }

                    options.ReportingEnabled = reportingEnabled;
                }
            }

            return options;
        }
    }
}