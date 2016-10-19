// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using App.Metrics.MetricData;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public sealed class TextFileReport : HumanReadableReport
    {
        private readonly string _fileName;
        private readonly ILogger _logger;
        private StringBuilder _buffer;
        private bool _disposed = false;

        public TextFileReport(string fileName,
            ILoggerFactory loggerFactory,
            IMetricsFilter filter,
            IClock clock)
            : base(loggerFactory, filter, clock)

        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            _fileName = fileName;
            _logger = loggerFactory.CreateLogger<ConsoleReport>();
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

        protected override void EndReport(string contextName)
        {
            try
            {
                File.WriteAllText(_fileName, _buffer.ToString());
            }
            catch (Exception x)
            {
                Logger.LogError(new EventId(), x, "Error writing text file " + _fileName);
                //TODO: Review enableing internal metrics
            }

            base.EndReport(contextName);
            _buffer = null;
        }


        protected override void StartReport(string contextName)
        {
            _logger.ReportStarting<TextFileReport>();

            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            _buffer = new StringBuilder();
            base.StartReport(contextName);

            _logger.ReportedStarted<TextFileReport>(startTimestamp);
        }

        protected override void WriteLine(string line, params string[] args)
        {
            _buffer.AppendFormat(line, args);
            _buffer.AppendLine();
        }
    }
}