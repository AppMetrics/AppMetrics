// <copyright file="BaseMetricsReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Logging;

namespace App.Metrics.Reporting.Base
{
    public abstract class BaseMetricsReporter : IReportMetrics
    {
        private static readonly ILog Logger = LogProvider.For<BaseMetricsReporter>();
        private readonly IMetricsOutputFormatter _defaultMetricsOutputFormatter = new MetricsTextOutputFormatter();

        // ReSharper disable UnusedMember.Global
        public BaseMetricsReporter()
            // ReSharper restore UnusedMember.Global
        {
            FlushInterval = AppMetricsConstants.Reporting.DefaultFlushInterval;
            Formatter = _defaultMetricsOutputFormatter;

            Logger.Info($"Using Metrics Reporter {this}. FlushInterval: {FlushInterval}. Formatter: {Formatter}");
        }

        public BaseMetricsReporter(MetricsReportingBaseOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.FlushInterval < TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingBaseOptions.FlushInterval)} must not be less than zero");
            }

            Formatter = options.MetricsOutputFormatter ?? _defaultMetricsOutputFormatter;

            FlushInterval = options.FlushInterval > TimeSpan.Zero
                ? options.FlushInterval
                : AppMetricsConstants.Reporting.DefaultFlushInterval;

            Filter = options.Filter;

            Logger.Info($"Using Metrics Reporter {this}. FlushInterval: {FlushInterval}. Formatter: {Formatter}");
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
            var result = false;

            Logger.Trace("Flushing metrics snapshot");

            using (var stream = new MemoryStream())
            {
                await Formatter.WriteAsync(stream, metricsData, cancellationToken);

                result = await this.FlushImplAsync(stream, cancellationToken);
            }

            Logger.Trace("Flushed metrics snapshot");

            return result;
        }

        public virtual async Task<bool> FlushImplAsync(MemoryStream stream, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            throw new InvalidOperationException($"{nameof(BaseMetricsReporter.FlushImplAsync)} should be overriden");
        }
    }
}