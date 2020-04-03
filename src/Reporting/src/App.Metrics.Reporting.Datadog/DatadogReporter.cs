// <copyright file="DatadogReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatting.Datadog;
using App.Metrics.Logging;
using App.Metrics.Reporting.Datadog.Client;

namespace App.Metrics.Reporting.Datadog
{
    public class DatadogReporter : IReportMetrics
    {
        private static readonly ILog Logger = LogProvider.For<DatadogReporter>();
        private readonly IDatadogClient _datadogClient;

        public DatadogReporter(
            MetricsReportingDatadogOptions options,
            IDatadogClient datadogClient)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.FlushInterval < TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingDatadogOptions.FlushInterval)} must not be less than zero");
            }

            _datadogClient = datadogClient ?? throw new ArgumentNullException(nameof(datadogClient));

            Formatter = options.MetricsOutputFormatter ?? new MetricsDatadogJsonOutputFormatter(options.FlushInterval);

            FlushInterval = options.FlushInterval > TimeSpan.Zero
                ? options.FlushInterval
                : AppMetricsConstants.Reporting.DefaultFlushInterval;

            Filter = options.Filter;
            Logger.Info($"Using Metrics Reporter {this}. Url: {options.Datadog.BaseUri} FlushInterval: {FlushInterval}");
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

            DatadogWriteResult result;

            using (var stream = new MemoryStream())
            {
                await Formatter.WriteAsync(stream, metricsData, cancellationToken);

                result = await _datadogClient.WriteAsync(Encoding.UTF8.GetString(stream.ToArray()), cancellationToken);
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