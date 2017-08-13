// <copyright file="MetricsJsonOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Formatters.Json.Internal
{
    /// <summary>
    ///     Sets up default JSON formatting options for <see cref="MetricsOptions"/>.
    /// </summary>
    public class MetricsJsonOptionsSetup : IConfigureOptions<MetricsOptions>
    {
        private readonly MetricsJsonOptions _jsonOptions;

        public MetricsJsonOptionsSetup(IOptions<MetricsJsonOptions> asciiOptionsAccessor)
        {
            _jsonOptions = asciiOptionsAccessor.Value ?? throw new ArgumentNullException(nameof(asciiOptionsAccessor));
        }

        /// <inheritdoc />
        public void Configure(MetricsOptions options)
        {
            var formatter = new MetricsJsonOutputFormatter(_jsonOptions.SerializerSettings);
            var envFormatter = new EnvInfoJsonOutputFormatter(_jsonOptions.SerializerSettings);

            options.OutputMetricsFormatters.Add(formatter);
            options.OutputEnvFormatters.Add(envFormatter);
        }
    }
}