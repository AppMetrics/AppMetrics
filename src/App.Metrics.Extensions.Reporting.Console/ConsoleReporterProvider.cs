// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using App.Metrics.Reporting.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.Console
{
    public class ConsoleReporterProvider : IReporterProvider
    {
        private readonly ConcurrentDictionary<string, ConsoleReporter> _reporters = new ConcurrentDictionary<string, ConsoleReporter>();
        private readonly IConsoleReporterSettings _settings;

        public ConsoleReporterProvider(IConsoleReporterSettings settings, IMetricsFilter filter)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;

            Filter = filter;
        }

        public IMetricsFilter Filter { get; }

        public IReporterSettings Settings => _settings;

        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory)
        {
            return _reporters.GetOrAdd(name, CreateReporterImplementation(name, loggerFactory));
        }

        public void Dispose()
        {
        }

        private ConsoleReporter CreateReporterImplementation(string name, ILoggerFactory loggerFactory)
        {
            return new ConsoleReporter(name, _settings.ReportInterval, loggerFactory);
        }
    }
}