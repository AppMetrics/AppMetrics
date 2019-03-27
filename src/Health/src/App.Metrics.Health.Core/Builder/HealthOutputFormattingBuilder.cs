// <copyright file="HealthOutputFormattingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Formatters;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring <see cref="IHealthOutputFormatter" />s used for formatting <see cref="HealthCheckResult" />s when
    ///     they are reported.
    /// </summary>
    public class HealthOutputFormattingBuilder : IHealthOutputFormattingBuilder
    {
        private readonly Action<bool, IHealthOutputFormatter> _metricsFormatter;

        internal HealthOutputFormattingBuilder(
            IHealthBuilder healthBuilder,
            Action<bool, IHealthOutputFormatter> metricsFormatter)
        {
            Builder = healthBuilder ?? throw new ArgumentNullException(nameof(healthBuilder));
            _metricsFormatter = metricsFormatter ?? throw new ArgumentNullException(nameof(metricsFormatter));
        }

        /// <inheritdoc />
        public IHealthBuilder Builder { get; }

        /// <inheritdoc />
        public IHealthBuilder Using(IHealthOutputFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            _metricsFormatter(true, formatter);

            return Builder;
        }

        /// <inheritdoc />
        public IHealthBuilder Using<TMetricsOutputFormatter>()
            where TMetricsOutputFormatter : IHealthOutputFormatter, new()
        {
            _metricsFormatter(true, new TMetricsOutputFormatter());

            return Builder;
        }

        /// <inheritdoc />
        public IHealthBuilder Using(IHealthOutputFormatter formatter, bool replaceExisting)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            _metricsFormatter(replaceExisting, formatter);

            return Builder;
        }

        /// <inheritdoc />
        public IHealthBuilder Using<THealthsOutputFormatter>(bool replaceExisting)
            where THealthsOutputFormatter : IHealthOutputFormatter, new()
        {
            _metricsFormatter(replaceExisting, new THealthsOutputFormatter());

            return Builder;
        }
    }
}