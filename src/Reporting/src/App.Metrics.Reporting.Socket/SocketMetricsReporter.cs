// <copyright file="SocketMetricsReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Logging;
using App.Metrics.Reporting.Socket.Client;

namespace App.Metrics.Reporting.Socket
{
    public class SocketMetricsReporter : IReportMetrics
    {
        private static readonly ILog Logger = LogProvider.For<SocketMetricsReporter>();
        private readonly DefaultSocketClient _socketClient;
        private readonly MetricsReportingSocketOptions _options;

        public SocketMetricsReporter(MetricsReportingSocketOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.MetricsOutputFormatter == null)
            {
                throw new ArgumentNullException(nameof(options.MetricsOutputFormatter));
            }
            _options = options ?? throw new ArgumentNullException(nameof(options));

            if (options.FlushInterval < TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingSocketOptions.FlushInterval)} must not be less than zero");
            }

            Formatter = options.MetricsOutputFormatter;

            FlushInterval = options.FlushInterval > TimeSpan.Zero
                ? options.FlushInterval
                : AppMetricsConstants.Reporting.DefaultFlushInterval;

            Filter = options.Filter;

            _socketClient = new DefaultSocketClient(options);

            Logger.Info($"Using Metrics Reporter {this}. Output: {_socketClient.Endpoint} FlushInterval: {FlushInterval}");
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
            if (_socketClient.PreferChunkedOutput && Formatter is IMetricsChunkedOutputFormatter formatter)
                return await WriteChunkedOutput(formatter, metricsData, cancellationToken);
            return await WriteOutput(metricsData, cancellationToken);
        }

        private async Task<bool> WriteChunkedOutput(
            IMetricsChunkedOutputFormatter formatter,
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default)
        {
            Logger.Trace("Flushing chunked metrics snapshot");

            var chunks = await formatter.WriteAsync(
                metricsData, 
                _options.SocketSettings.MaxUdpDatagramSize,
                cancellationToken);

            var success = true;
            foreach (var chunk in chunks)
            {
                var result = await _socketClient.WriteAsync(chunk, cancellationToken);
                if (!result.Success)
                {
                    Logger.Error(result.ErrorMessage);
                    success = false;
                }
            }

            Logger.Trace(success 
                ? "Successfully flushed chunked metrics snapshot"
                : "Flushed chunked metrics snapshot with error(s)");
            return success;
        }

        private async Task<bool> WriteOutput(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            Logger.Trace("Flushing metrics snapshot");

            using var stream = new MemoryStream();

            await Formatter.WriteAsync(stream, metricsData, cancellationToken);
            var output = Encoding.UTF8.GetString(stream.ToArray());
            var result = await _socketClient.WriteAsync(output, cancellationToken);

            if (result.Success)
            {
                Logger.Trace("Successfully flushed metrics snapshot");
                return true;
            }

            Logger.Error(result.ErrorMessage);

            return false;
        }
    }
}
