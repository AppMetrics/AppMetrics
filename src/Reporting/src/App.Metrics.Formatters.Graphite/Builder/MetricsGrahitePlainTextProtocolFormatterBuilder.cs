// <copyright file="MetricsGrahitePlainTextProtocolFormatterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Graphite;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricsGrahitePlainTextProtocolFormatterBuilder
    {
        /// <summary>
        ///     Add the <see cref="MetricsGraphitePlainTextProtocolOutputFormatter" /> allowing metrics to optionally be reported to
        ///     Graphite using the Plain Text Protocol.
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure InfluxDB Lineprotocol formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The InfluxDB LineProtocol formatting options to use.</param>
        /// <param name="fields">The metric fields to report as well as thier names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsGraphitePlainTextProtocol(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            Action<MetricsGraphitePlainTextProtocolOptions> setupAction,
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

            var options = new MetricsGraphitePlainTextProtocolOptions();

            setupAction.Invoke(options);

            if (fields == null)
            {
                fields = new MetricFields();
                fields.DefaultGraphiteMetricFieldNames();
            }

            var formatter = new MetricsGraphitePlainTextProtocolOutputFormatter(options, fields);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsGraphitePlainTextProtocolOutputFormatter" /> allowing metrics to optionally be reported to
        ///     Graphite using the Plain Text Protocol.
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure InfluxDB Lineprotocol formatting
        ///     options.
        /// </param>
        /// <param name="options">The Graphite Plain Text Protocol formatting options to use.</param>
        /// <param name="fields">The metric fields to report as well as thier names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsGraphitePlainTextProtocol(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            MetricsGraphitePlainTextProtocolOptions options,
            MetricFields fields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            if (fields == null)
            {
                fields = new MetricFields();
                fields.DefaultGraphiteMetricFieldNames();
            }

            var formatter = new MetricsGraphitePlainTextProtocolOutputFormatter(options, fields);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsGraphitePlainTextProtocolOutputFormatter" /> allowing metrics to optionally be reported to
        ///     Graphite using the Plain Text Protocol.
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure InfluxDB Lineprotocol formatting
        ///     options.
        /// </param>
        /// <param name="fields">The metric fields to report as well as thier names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsGraphitePlainTextProtocol(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            MetricFields fields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            if (fields == null)
            {
                fields = new MetricFields();
                fields.DefaultGraphiteMetricFieldNames();
            }

            var formatter = new MetricsGraphitePlainTextProtocolOutputFormatter(fields);

            return metricFormattingBuilder.Using(formatter, false);
        }
    }
}