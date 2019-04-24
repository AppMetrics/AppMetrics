// <copyright file="GraphiteReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Graphite;
using App.Metrics.Logging;
using App.Metrics.Reporting.Graphite.Client;

namespace App.Metrics.Reporting.Graphite
{
    public class GraphiteReporter : IReportMetrics
    {
        private static readonly ILog Logger = LogProvider.For<GraphiteReporter>();
        private readonly IGraphiteClient _graphiteClient;

        public GraphiteReporter(
            MetricsReportingGraphiteOptions options,
            IGraphiteClient graphiteClient)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.FlushInterval < TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingGraphiteOptions.FlushInterval)} must not be less than zero");
            }

            _graphiteClient = graphiteClient ?? throw new ArgumentNullException(nameof(graphiteClient));

            Formatter = options.MetricsOutputFormatter ?? new MetricsGraphitePlainTextProtocolOutputFormatter();

            FlushInterval = options.FlushInterval > TimeSpan.Zero
                ? options.FlushInterval
                : AppMetricsConstants.Reporting.DefaultFlushInterval;

            Filter = options.Filter;
            Logger.Debug($"Using Metrics Reporter {this}. Url: {options.Graphite.BaseUri} FlushInterval: {FlushInterval}");
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

            GraphiteWriteResult result;

            using (var stream = new MemoryStream())
            {
                await Formatter.WriteAsync(stream, metricsData, cancellationToken);

                result = await _graphiteClient.WriteAsync(Encoding.UTF8.GetString(stream.ToArray()), cancellationToken);
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
