// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    public class InfluxDbReporterSettings : IInfluxDbReporterSettings
    {
        public TimeSpan ReportInterval { get; set; } = TimeSpan.FromSeconds(5);

        public string RetentionPolicy { get; set; }

        public string Consistency { get; set; }

        public string BreakerRate { get; set; } = "3 / 00:00:30";

        public string BaseAddress { get; set; }

        public string Database { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}