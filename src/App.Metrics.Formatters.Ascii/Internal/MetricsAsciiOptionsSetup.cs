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

            if (options.DefaultOutputFormatter == null)
            {
                options.DefaultOutputFormatter = formatter;
            }

            options.OutputFormatters.Add(formatter);
        }
    }
}