// <copyright file="KeyValuePairHealthOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Health.Internal
{
    internal class KeyValuePairHealthOptions
    {
        internal static readonly string EnabledDirective = $"{nameof(HealthOptions)}:{nameof(HealthOptions.Enabled)}";
        internal static readonly string ApplicationNameDirective = $"{nameof(HealthOptions)}:{nameof(HealthOptions.ApplicationName)}";
        internal static readonly string ReportingEnabledDirective = $"{nameof(HealthOptions)}:{nameof(HealthOptions.ReportingEnabled)}";

        private readonly HealthOptions _options;

        private readonly Dictionary<string, string> _optionValues;

        public KeyValuePairHealthOptions(HealthOptions options, IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            _options = options;
            _optionValues = optionValues.ToDictionary(o => o.Key, o => o.Value);
        }

        public KeyValuePairHealthOptions(IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            _optionValues = optionValues.ToDictionary(o => o.Key, o => o.Value);
        }

        public HealthOptions AsOptions()
        {
            var options = _options ?? new HealthOptions();

            foreach (var key in _optionValues.Keys)
            {
                if (string.Compare(key, EnabledDirective, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    if (!bool.TryParse(_optionValues[key], out var metricsEnabled))
                    {
                        throw new InvalidCastException($"Attempted to bind {key} to {EnabledDirective} but it's not a boolean");
                    }

                    options.Enabled = metricsEnabled;
                }
                else if (string.Compare(key, ApplicationNameDirective, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    options.ApplicationName = _optionValues[key];
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