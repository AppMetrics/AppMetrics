// <copyright file="MetricsDatadogFormatterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatting.Datadog;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricsDatadogFormatterBuilder
    {
        /// <summary>
        ///     Add the <see cref="MetricsDatadogJsonOutputFormatter" /> allowing metrics to optionally be reported to Datadog
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Datadog formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The Datadog formatting options to use.</param>
        /// <param name="flushInterval">Interval is required my hosted metrics.</param>
        /// <param name="fields">The metric fields to report as well as their names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsDatadogJson(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            Action<MetricsDatadogOptions> setupAction,
            TimeSpan flushInterval,
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

            var options = new MetricsDatadogOptions();

            setupAction.Invoke(options);

            var formatter = new MetricsDatadogJsonOutputFormatter(flushInterval, options, fields);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsDatadogJsonOutputFormatter" /> allowing metrics to optionally be reported to Datadog
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Datadog formatting
        ///     options.
        /// </param>
        /// <param name="options">The Datadog formatting options to use.</param>
        /// <param name="flushInterval">Interval is required my hosted metrics.</param>
        /// <param name="fields">The metric fields to report as well as their names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsDatadogJson(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            MetricsDatadogOptions options,
            TimeSpan flushInterval,
            MetricFields fields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsDatadogJsonOutputFormatter(flushInterval, options, fields);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsDatadogJsonOutputFormatter" /> allowing metrics to optionally be reported to Datadog
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Datadog formatting
        ///     options.
        /// </param>
        /// <param name="flushInterval">Interval is required my hosted metrics.</param>
        /// <param name="fields">The metric fields to report as well as their names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsDatadogJson(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            TimeSpan flushInterval,
            MetricFields fields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsDatadogJsonOutputFormatter(flushInterval, fields);

            return metricFormattingBuilder.Using(formatter, false);
        }
    }
}