// <copyright file="HostedMetricsReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics;
using App.Metrics.Logging;
using App.Metrics.Reporting.GrafanaCloudHostedMetrics.Client;

namespace App.Metrics.Reporting.GrafanaCloudHostedMetrics
{
    public class HostedMetricsReporter : IReportMetrics
    {
        private static readonly ILog Logger = LogProvider.For<HostedMetricsReporter>();
        private readonly IHostedMetricsClient _hostedMetricsClient;

        public HostedMetricsReporter(
            MetricsReportingHostedMetricsOptions options,
            IHostedMetricsClient hostedMetricsClient)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.FlushInterval < TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingHostedMetricsOptions.FlushInterval)} must not be less than zero");
            }

            _hostedMetricsClient = hostedMetricsClient ?? throw new ArgumentNullException(nameof(hostedMetricsClient));

            Formatter = options.MetricsOutputFormatter ?? new MetricsHostedMetricsJsonOutputFormatter(options.FlushInterval);

            FlushInterval = options.FlushInterval > TimeSpan.Zero
                ? options.FlushInterval
                : AppMetricsConstants.Reporting.DefaultFlushInterval;

            Filter = options.Filter;
            Logger.Info($"Using Metrics Reporter {this}. Url: {options.HostedMetrics.BaseUri} FlushInterval: {FlushInterval}");
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; set; }

        /// <inheritdoc />
        public TimeSpan FlushInterval { get; set; }

        /// <inheritdoc />
        public IMetricsOutputFormatter Formatter { get; set; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            Logger.Trace("Flushing metrics snapshot");

            HostedMetricsWriteResult result;

            using (var stream = new MemoryStream())
            {
                await Formatter.WriteAsync(stream, metricsData, cancellationToken);

                result = await _hostedMetricsClient.WriteAsync(Encoding.UTF8.GetString(stream.ToArray()), cancellationToken);
            }

            if (result.Success)
            {
                Logger.Trace("Flushed metrics snapshot");
                return true;
            }

            Logger.Error(result.ErrorMessage);

            return false;
        }
    }
}