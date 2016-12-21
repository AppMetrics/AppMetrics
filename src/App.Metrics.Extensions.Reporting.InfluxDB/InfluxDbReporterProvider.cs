// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using App.Metrics.Reporting.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    public class InfluxDbReporterProvider : IReporterProvider
    {
        private readonly ConcurrentDictionary<string, InfluxDbReporter> _reporters = new ConcurrentDictionary<string, InfluxDbReporter>();
        private readonly IInfluxDbReporterSettings _settings;

        public InfluxDbReporterProvider(IInfluxDbReporterSettings settings, IMetricsFilter fitler)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
            Filter = fitler;
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

        private InfluxDbReporter CreateReporterImplementation(string name, ILoggerFactory loggerFactory)
        {
            return new InfluxDbReporter(name, new Uri(_settings.BaseAddress), _settings.Username, _settings.Password, _settings.Database,
                _settings.BreakerRate, _settings.ReportInterval,
                _settings.RetentionPolicy, _settings.Consistency, loggerFactory);
        }
    }
}