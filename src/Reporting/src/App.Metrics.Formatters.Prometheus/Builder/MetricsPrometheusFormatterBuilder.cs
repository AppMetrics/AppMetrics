// <copyright file="MetricsPrometheusFormatterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Prometheus;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricsPrometheusFormatterBuilder
    {
        /// <summary>
        ///     Add the <see cref="MetricsPrometheusFormatterBuilder" /> allowing metrics to optionally be formatted in Prometheus plain text format
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Prometheus formatting
        ///     options.
        /// </param>
        /// <param name="options">The Prometheus formatting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsPrometheusPlainText(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            MetricsPrometheusOptions options)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsPrometheusTextOutputFormatter(options);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsPrometheusFormatterBuilder" /> allowing metrics to optionally be formatted in Prometheus plain text format
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Prometheus formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The Prometheus formatting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsPrometheusPlainText(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            Action<MetricsPrometheusOptions> setupAction)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var options = new MetricsPrometheusOptions();

            setupAction.Invoke(options);

            var formatter = new MetricsPrometheusTextOutputFormatter(options);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsPrometheusFormatterBuilder" /> allowing metrics to optionally be formatted in Prometheus plain text format
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Prometheus formatting
        ///     options.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsPrometheusPlainText(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsPrometheusTextOutputFormatter();

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsPrometheusFormatterBuilder" /> allowing metrics to optionally be formatted in Prometheus protobuf format
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Prometheus formatting
        ///     options.
        /// </param>
        /// <param name="options">The Prometheus formatting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsPrometheusProtobuf(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            MetricsPrometheusOptions options)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsPrometheusProtobufOutputFormatter(options);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsPrometheusFormatterBuilder" /> allowing metrics to optionally be formatted in Prometheus protobuf format
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Prometheus formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The Prometheus formatting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsPrometheusProtobuf(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            Action<MetricsPrometheusOptions> setupAction)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var options = new MetricsPrometheusOptions();

            setupAction.Invoke(options);

            var formatter = new MetricsPrometheusProtobufOutputFormatter(options);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsPrometheusFormatterBuilder" /> allowing metrics to optionally be formatted in Prometheus protobuf format
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure Prometheus formatting
        ///     options.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsPrometheusProtobuf(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsPrometheusProtobufOutputFormatter();

            return metricFormattingBuilder.Using(formatter, false);
        }
    }
}