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

        public MetricsAsciiOptionsSetup(IOptions<MetricsAsciiOptions> asciiOptions)
        {
            _asciiOptions = asciiOptions.Value ?? throw new ArgumentNullException(nameof(asciiOptions));
        }

        public void Configure(MetricsOptions options)
        {
            var formatter = new AsciiOutputFormatter(_asciiOptions);
            var envFormatter = new AsciiEnvOutputFormatter(_asciiOptions);

            if (options.DefaultOutputFormatter == null)
            {
                options.DefaultOutputFormatter = formatter;
            }

            if (options.DefaultEnvOutputFormatter == null)
            {
                options.DefaultEnvOutputFormatter = envFormatter;
            }

            options.OutputFormatters.Add(formatter);
            options.EnvOutputFormatters.Add(envFormatter);
        }
    }
}