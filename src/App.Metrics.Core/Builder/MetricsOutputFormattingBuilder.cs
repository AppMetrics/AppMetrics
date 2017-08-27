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
    public class MetricsOutputFormattingBuilder
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

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IMetricsOutputFormatter"/> as one of the available formatters when reporting metric values.
        ///     </para>
        ///     <para>
        ///         Mulitple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputMetricsFormatter"/> will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <param name="formatter">An <see cref="IMetricsOutputFormatter"/> instance used to format metric values when reporting.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder Using(IMetricsOutputFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            _metricsFormatter(formatter);

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IMetricsOutputFormatter"/> as one of the available formatters when reporting metric values.
        ///     </para>
        ///     <para>
        ///         Mulitple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputMetricsFormatter"/> will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <typeparam name="TMetricsOutputFormatter">An <see cref="IMetricsOutputFormatter"/> type used to format metric values when reporting.</typeparam>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder Using<TMetricsOutputFormatter>()
            where TMetricsOutputFormatter : IMetricsOutputFormatter, new()
        {
            _metricsFormatter(new TMetricsOutputFormatter());

            return _metricsBuilder;
        }
    }
}
