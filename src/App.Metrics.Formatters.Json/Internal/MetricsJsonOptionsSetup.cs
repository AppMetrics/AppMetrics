// <copyright file="MetricsJsonOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Formatters.Json.Internal
{
    /// <summary>
    ///     Sets up default ASCII options for <see cref="MetricsOptions"/>.
    /// </summary>
    public class MetricsJsonOptionsSetup : IConfigureOptions<MetricsOptions>
    {
        private readonly MetricsJsonOptions _jsonOptions;

        public MetricsJsonOptionsSetup(IOptions<MetricsJsonOptions> asciiOptionsAccessor)
        {
            _jsonOptions = asciiOptionsAccessor.Value ?? throw new ArgumentNullException(nameof(asciiOptionsAccessor));
        }

        public void Configure(MetricsOptions options)
        {
            var formatter = new JsonMetricsOutputFormatter(_jsonOptions.SerializerSettings);
            var envFormatter = new JsonEnvOutputFormatter(_jsonOptions.SerializerSettings);

            options.OutputMetricsFormatters.Add(formatter);
            options.OutputMetricsTextFormatters.Add(formatter);
            options.OutputEnvFormatters.Add(envFormatter);
        }
    }
}