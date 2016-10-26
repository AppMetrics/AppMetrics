// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Diagnostics;
using System.Threading;
using App.Metrics.Reporters;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Internal
{
    internal sealed class DefaultScheduledReporter : IScheduledReporter
    {
        private readonly TimeSpan _interval;
        private readonly IMetricsContext _metricsContext;
        private readonly IMetricReporter _reporter;
        private readonly IScheduler _scheduler;
        private readonly ILogger _logger;
        private bool _disposed = false;

        public DefaultScheduledReporter(
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext,
            IMetricReporter reporter,
            TimeSpan interval)
            : this(metricsContext, reporter, interval, new ActionScheduler())
        {
            _logger = loggerFactory.CreateLogger<DefaultScheduledReporter>();
        }

        public DefaultScheduledReporter(
            IMetricsContext metricsContext,
            IMetricReporter reporter,
            TimeSpan interval,
            IScheduler scheduler)
        {
            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            if (reporter == null)
            {
                throw new ArgumentNullException(nameof(reporter));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            _metricsContext = metricsContext;
            _reporter = reporter;
            _interval = interval;
            _scheduler = scheduler;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Start(CancellationToken token)
        {
            _scheduler.Start(_interval, t => ReportAction(t));
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                }

                _scheduler.Dispose();
                _reporter.Dispose();
            }

            _disposed = true;
        }

        private void ReportAction(CancellationToken token)
        {
            _logger.ReportRunning(_reporter);

            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            _reporter.RunReport(_metricsContext, token);

            _logger.ReportRan(_reporter, startTimestamp);
        }
    }
}