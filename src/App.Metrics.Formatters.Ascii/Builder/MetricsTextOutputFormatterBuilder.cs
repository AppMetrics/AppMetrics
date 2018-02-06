// <copyright file="MetricsTextOutputFormatterBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsPlainText(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            Action<MetricsTextOptions> setupAction = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var options = new MetricsTextOptions();

            setupAction?.Invoke(options);

            var formatter = new MetricsTextOutputFormatter();

            return metricFormattingBuilder.Using(formatter);
        }
    }
}
