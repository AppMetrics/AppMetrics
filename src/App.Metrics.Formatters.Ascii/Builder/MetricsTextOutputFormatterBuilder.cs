// <copyright file="MetricsTextOutputFormatterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Ascii;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metric plain text output formatting using an
    ///     <see cref="IMetricsBuilder" />.
    /// </summary>
    public static class MetricsTextOutputFormatterBuilder
    {
        /// <summary>
        ///     Add the <see cref="MetricsTextOutputFormatter" /> allowing metrics to optionally be reported as plain text.
        /// </summary>
        /// <param name="metricFormattingBuilder">
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The plain text formatting options to use.</param>
        /// <param name="metricFields">The metric fields to output as well as their names. This will override the globally configured metric fields.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsPlainText(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            Action<MetricsTextOptions> setupAction = null,
            MetricFields metricFields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var options = new MetricsTextOptions();

            setupAction?.Invoke(options);

            var formatter = new MetricsTextOutputFormatter(options) { MetricFields = metricFields };

            return metricFormattingBuilder.Using(formatter);
        }

        /// <summary>
        ///     Add the <see cref="MetricsTextOutputFormatter" /> allowing metrics to optionally be reported as plain text.
        /// </summary>
        /// <param name="metricFormattingBuilder">
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure formatting
        ///     options.
        /// </param>
        /// <param name="metricFields">The metric fields to output as well as their names. This will override the globally configured metric fields.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsPlainText(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            MetricFields metricFields)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsTextOutputFormatter(metricFields);

            return metricFormattingBuilder.Using(formatter);
        }
    }
}
