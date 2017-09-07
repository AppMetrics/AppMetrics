// <copyright file="MetricsOutputFormattingBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring <see cref="IMetricsOutputFormatter"/>s used for formatting <see cref="MetricType"/>s when they are reported.
    /// </summary>
    public class MetricsOutputFormattingBuilder : IMetricsOutputFormattingBuilder
    {
        private readonly IMetricsBuilder _metricsBuilder;
        private readonly Action<IMetricsOutputFormatter> _metricsFormatter;

        internal MetricsOutputFormattingBuilder(
            IMetricsBuilder metricsBuilder,
            Action<IMetricsOutputFormatter> metricsFormatter)
        {
            _metricsBuilder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _metricsFormatter = metricsFormatter ?? throw new ArgumentNullException(nameof(metricsFormatter));
        }

        /// <inheritdoc />
        public IMetricsBuilder Using(IMetricsOutputFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            _metricsFormatter(formatter);

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TMetricsOutputFormatter>()
            where TMetricsOutputFormatter : IMetricsOutputFormatter, new()
        {
            _metricsFormatter(new TMetricsOutputFormatter());

            return _metricsBuilder;
        }
    }
}
