// <copyright file="MetricsAsciiOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Formatters.Ascii.Internal
{
    /// <summary>
    ///     Sets up default ASCII options for <see cref="MetricsOptions"/>.
    /// </summary>
    public class MetricsAsciiOptionsSetup : IConfigureOptions<MetricsOptions>
    {
        private readonly MetricsAsciiOptions _asciiOptions;

        public MetricsAsciiOptionsSetup(IOptions<MetricsAsciiOptions> asciiOptionsAccessor)
        {
            _asciiOptions = asciiOptionsAccessor.Value ?? throw new ArgumentNullException(nameof(asciiOptionsAccessor));
        }

        public void Configure(MetricsOptions options)
        {
            var formatter = new AsciiMetricsOutputFormatter(_asciiOptions);
            var envFormatter = new AsciiEnvOutputFormatter(_asciiOptions);

            options.OutputMetricsFormatters.Add(formatter);
            options.OutputMetricsTextFormatters.Add(formatter);
            options.OutputEnvFormatters.Add(envFormatter);
        }
    }
}