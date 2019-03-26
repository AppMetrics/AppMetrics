// <copyright file="EnvOutputFormattingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring environment information output formatting using an <see cref="IMetricsBuilder" />.
    /// </summary>
    public class EnvOutputFormattingBuilder : IEnvOutputFormattingBuilder
    {
        private readonly Action<IEnvOutputFormatter> _formatter;

        internal EnvOutputFormattingBuilder(
            IMetricsBuilder metricsBuilder,
            Action<IEnvOutputFormatter> formatter)
        {
            Builder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        /// <inheritdoc />
        public IMetricsBuilder Builder { get; }

        /// <inheritdoc />
        public IMetricsBuilder Using(IEnvOutputFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            _formatter(formatter);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TEvnOutputFormatter>()
            where TEvnOutputFormatter : IEnvOutputFormatter, new()
        {
            _formatter(new TEvnOutputFormatter());

            return Builder;
        }
    }
}
