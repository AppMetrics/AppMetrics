// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Internal;
using App.Metrics.Reporting.Interfaces;
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

        public InfluxDbReporterProvider(InfluxDBReporterSettings settings, IMetricsFilter fitler)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
            Filter = fitler ?? new NoOpMetricsFilter();
        }

        public IMetricsFilter Filter { get; }

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