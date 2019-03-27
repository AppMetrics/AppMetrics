// <copyright file="MetricsInfluxDbLineProtocolFormatterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.InfluxDB;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricsInfluxDbLineProtocolFormatterBuilder
    {
        /// <summary>
        ///     Add the <see cref="MetricsInfluxDbLineProtocolOutputFormatter" /> allowing metrics to optionally be reported to
        ///     InfluxDB using the LineProtocol.
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
        public static IMetricsBuilder AsInfluxDbLineProtocol(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            Action<MetricsInfluxDbLineProtocolOptions> setupAction,
            MetricFields fields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var options = new MetricsInfluxDbLineProtocolOptions();

            setupAction?.Invoke(options);

            var formatter = new MetricsInfluxDbLineProtocolOutputFormatter(options, fields);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsInfluxDbLineProtocolOutputFormatter" /> allowing metrics to optionally be reported to
        ///     InfluxDB using the LineProtocol.
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure InfluxDB Lineprotocol formatting
        ///     options.
        /// </param>
        /// <param name="options">The InfluxDB LineProtocol formatting options to use.</param>
        /// <param name="fields">The metric fields to report as well as thier names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsInfluxDbLineProtocol(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            MetricsInfluxDbLineProtocolOptions options,
            MetricFields fields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsInfluxDbLineProtocolOutputFormatter(options, fields);

            return metricFormattingBuilder.Using(formatter, false);
        }

        /// <summary>
        ///     Add the <see cref="MetricsInfluxDbLineProtocolOutputFormatter" /> allowing metrics to optionally be reported to
        ///     InfluxDB using the LineProtocol.
        /// </summary>
        /// <param name="metricFormattingBuilder">s
        ///     The <see cref="IMetricsOutputFormattingBuilder" /> used to configure InfluxDB Lineprotocol formatting
        ///     options.
        /// </param>
        /// <param name="fields">The metric fields to report as well as thier names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsInfluxDbLineProtocol(
            this IMetricsOutputFormattingBuilder metricFormattingBuilder,
            MetricFields fields = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var formatter = new MetricsInfluxDbLineProtocolOutputFormatter(fields);

            return metricFormattingBuilder.Using(formatter, false);
        }
    }
}