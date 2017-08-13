// <copyright file="MetricsTextOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Formatters.Ascii.Internal
{
    /// <summary>
    ///     Sets up default ASCII options for <see cref="MetricsOptions"/>.
    /// </summary>
    public class MetricsTextOptionsSetup : IConfigureOptions<MetricsOptions>
    {
        private readonly MetricsTextOptions _textOptions;

        public MetricsTextOptionsSetup(IOptions<MetricsTextOptions> asciiOptionsAccessor)
        {
            _textOptions = asciiOptionsAccessor.Value ?? throw new ArgumentNullException(nameof(asciiOptionsAccessor));
        }

        public void Configure(MetricsOptions options)
        {
            var formatter = new MetricsTextOutputFormatter(_textOptions);
            var envFormatter = new EnvironmentInfoTextOutputFormatter(_textOptions);

            options.OutputMetricsFormatters.Add(formatter);
            options.OutputEnvFormatters.Add(envFormatter);
        }
    }
}