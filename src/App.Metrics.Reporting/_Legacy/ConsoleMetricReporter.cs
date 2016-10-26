// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting._Legacy
{
    public sealed class ConsoleMetricReporter : HumanReadableReporter
    {
        private bool _disposed = false;
        private readonly ILogger _logger;

        public ConsoleMetricReporter(IMetricsContext metricsContext,
            IMetricsFilter filter,
            ILoggerFactory loggerFactory)
            : base(loggerFactory, filter, metricsContext.Advanced.Clock)
        {
            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<ConsoleMetricReporter>();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Release managed resources
                }

                // Release unmanaged resources.
                // Set large fields to null.
                _disposed = true;
            }

            base.Dispose(disposing);
        }

        public override void StartReport(string contextName)
        {
            _logger.ReportStarting<ConsoleMetricReporter>();

            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            System.Console.Clear();

            base.StartReport(contextName);

            _logger.ReportedStarted<ConsoleMetricReporter>(startTimestamp);
        }

        protected override void WriteLine(string line, params string[] args)
        {
            System.Console.WriteLine(line, args);
        }
    }
}