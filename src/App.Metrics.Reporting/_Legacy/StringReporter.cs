// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting._Legacy
{
    public sealed class StringReporter : HumanReadableReporter
    {
        private readonly IMetricsContext _metricsContext;
        private StringBuilder _buffer;
        private bool _disposed = false;
        private ILogger _logger;

        public StringReporter(ILoggerFactory loggerFactory,
            IMetricsContext metricsContext,
            IMetricsFilter filter)
            : base(loggerFactory, filter, metricsContext.Advanced.Clock)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            _metricsContext = metricsContext;
            _logger = loggerFactory.CreateLogger<StringReporter>();
        }

        public string Result => _buffer.ToString();

        public async Task<string> RenderMetrics(IMetricsContext metricsContext)
        {
            await RunReport(metricsContext, CancellationToken.None);

            return Result;
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
                _buffer = null;
                _disposed = true;
            }

            base.Dispose(disposing);
        }

        public override void StartReport(string contextName)
        {
            _logger.ReportStarting<StringReporter>();

            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            _buffer = new StringBuilder();
            base.StartReport(contextName);

            _logger.ReportedStarted<StringReporter>(startTimestamp);
        }

        protected override void WriteLine(string line, params string[] args)
        {
            _buffer.AppendLine(string.Format(line, args));
        }
    }
}