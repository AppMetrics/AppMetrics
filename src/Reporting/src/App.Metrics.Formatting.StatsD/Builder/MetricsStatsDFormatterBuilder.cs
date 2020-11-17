// <copyright file="MetricsStatsDFormatterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Formatting.StatsD.Builder
{
    public static class MetricsStatsDFormatterBuilder
    {
        /// <summary>
        ///     Add the <see cref="MetricsStatsDStringOutputFormatter" /> allowing metrics to optionally be reported to Datadog
        /// </summary>
        /// <param name="metricFormattingBuilder">
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure StatsD formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The StatsD formatting options to use.</param>
        /// <param name="fields">The metric fields to report as well as their names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsStatsDString(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            Action<MetricsStatsDOptions> setupAction,
            MetricFields fields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var options = new MetricsStatsDOptions();

            setupAction(options);

            var formatter = new MetricsStatsDStringOutputFormatter(options, fields);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsStatsDStringOutputFormatter" /> allowing metrics to optionally be reported to Datadog
        /// </summary>
        /// <param name="metricFormattingBuilder">
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Datadog formatting
        ///     options.
        /// </param>
        /// <param name="options">The Datadog formatting options to use.</param>
        /// <param name="fields">The metric fields to report as well as their names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsStatsDString(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            MetricsStatsDOptions options,
            MetricFields fields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsStatsDStringOutputFormatter(options, fields);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsStatsDStringOutputFormatter" /> allowing metrics to optionally be reported to Datadog
        /// </summary>
        /// <param name="metricFormattingBuilder">
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Datadog formatting
        ///     options.
        /// </param>
        /// <param name="fields">The metric fields to report as well as their names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsStatsDString(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            MetricFields fields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsStatsDStringOutputFormatter(fields);

            return metricFormattingBuilder.Using(formatter, false);
        }
    }
}