// <copyright file="KeyValuePairMetricsOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Tagging;

namespace App.Metrics
{
    internal class KeyValuePairMetricsOptions
    {
        private static readonly string AddDefaultGlobalTagsDirective = $"{nameof(MetricsOptions)}:{nameof(MetricsOptions.AddDefaultGlobalTags)}";
        private static readonly string DefaultContextLabelDirective = $"{nameof(MetricsOptions)}:{nameof(MetricsOptions.DefaultContextLabel)}";
        private static readonly string GlobalTagsDirective = $"{nameof(MetricsOptions)}:{nameof(MetricsOptions.GlobalTags)}";
        private static readonly string EnabledDirective = $"{nameof(MetricsOptions)}:{nameof(MetricsOptions.Enabled)}";
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

        public MetricsOptions AsOptions()
        {
            var options = _options ?? new MetricsOptions();

            foreach (var key in _optionValues.Keys)
            {
                if (string.Compare(key, DefaultContextLabelDirective, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    options.DefaultContextLabel = _optionValues[key];
                }
                else if (string.Compare(key, AddDefaultGlobalTagsDirective, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    if (!bool.TryParse(_optionValues[key], out var addDefaultGlobalTags))
                    {
                        throw new InvalidCastException($"Attempted to bind {key} to {AddDefaultGlobalTagsDirective} but it's not a boolean");
                    }

                    options.AddDefaultGlobalTags = addDefaultGlobalTags;
                }
                else if (string.Compare(key, GlobalTagsDirective, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    options.GlobalTags = new GlobalMetricTags();

                    var globalTags = _optionValues[key].Split(new[] { ",", ", " }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var tagKeyValue in globalTags)
                    {
                        var keyValue = tagKeyValue.Split('=');

                        if (keyValue.Length != 2)
                        {
                            throw new InvalidOperationException(
                                $"Attempted to bind {key} to {GlobalTagsDirective} but the value was not property formatted, format as: value='tag1Name=tag1Value,tag2Name=tag2Value");
                        }

                        var tagKey = keyValue[0].Trim();
                        var tagValue = keyValue[1].Trim();
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
            }

            return options;
        }
    }
}