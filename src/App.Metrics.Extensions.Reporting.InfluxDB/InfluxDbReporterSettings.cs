// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Reporting.Interfaces;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    public class InfluxDBReporterSettings : IReporterSettings
    {
        public InfluxDBReporterSettings()
        {
            InfluxDbSettings = new InfluxDBSettings();
            HttpPolicy = new HttpPolicy
            {
                FailuresBeforeBackoff = Constants.DefaultFailuresBeforeBackoff,
                BackoffPeriod = Constants.DefaultBackoffPeriod,
                Timeout = Constants.DefaultTimeout
            };
            ReportInterval = TimeSpan.FromSeconds(5);
        }

        public HttpPolicy HttpPolicy { get; set; }

        public InfluxDBSettings InfluxDbSettings { get; set; }

        public TimeSpan ReportInterval { get; set; }
    }
}