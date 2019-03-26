// <copyright file="MetricsOutputFormattingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring <see cref="IMetricsOutputFormatter" />s used for formatting <see cref="MetricType" />s when
    ///     they are reported.
    /// </summary>
    public class MetricsOutputFormattingBuilder : IMetricsOutputFormattingBuilder
    {
        private readonly Action<bool, IMetricsOutputFormatter> _metricsFormatter;

        internal MetricsOutputFormattingBuilder(
            IMetricsBuilder metricsBuilder,
            Action<bool, IMetricsOutputFormatter> metricsFormatter)
        {
            Builder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _metricsFormatter = metricsFormatter ?? throw new ArgumentNullException(nameof(metricsFormatter));
        }

        /// <inheritdoc />
        public IMetricsBuilder Builder { get; }

        /// <inheritdoc />
        public IMetricsBuilder Using(IMetricsOutputFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            _metricsFormatter(true, formatter);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TMetricsOutputFormatter>(MetricFields fields = null)
            where TMetricsOutputFormatter : IMetricsOutputFormatter, new()
        {
            var formatter = new TMetricsOutputFormatter { MetricFields = fields };

            _metricsFormatter(true, formatter);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Using(IMetricsOutputFormatter formatter, bool replaceExisting)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            _metricsFormatter(replaceExisting, formatter);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TMetricsOutputFormatter>(bool replaceExisting, MetricFields fields = null)
            where TMetricsOutputFormatter : IMetricsOutputFormatter, new()
        {
            var formatter = new TMetricsOutputFormatter { MetricFields = fields };

            _metricsFormatter(replaceExisting, formatter);

            return Builder;
        }
    }
}