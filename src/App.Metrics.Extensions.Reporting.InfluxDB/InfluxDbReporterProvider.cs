// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Internal;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    public class InfluxDbReporterProvider : IReporterProvider
    {
        private readonly InfluxDBReporterSettings _settings;

        public InfluxDbReporterProvider(InfluxDBReporterSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
            Filter = new NoOpMetricsFilter();
        }

        public InfluxDbReporterProvider(InfluxDBReporterSettings settings, IFilterMetrics fitler)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
            Filter = fitler ?? new NoOpMetricsFilter();
        }

        public IFilterMetrics Filter { get; }

        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory)
        {
            var lineProtocolClient = new DefaultLineProtocolClient(
                loggerFactory,
                _settings.InfluxDbSettings,
                _settings.HttpPolicy);
            var payloadBuilder = new LineProtocolPayloadBuilder();

            return new InfluxDbReporter(
                lineProtocolClient,
                payloadBuilder,
                _settings.ReportInterval,
                name,
                loggerFactory,
                _settings.MetricNameFormatter);
        }
    }
}