// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace App.Metrics.Reporting.Console
{
    public class ConsoleReporterSettings : IConsoleReporterSettings
    {
        public TimeSpan Interval { get; set; }

        public bool Disabled { get; set; }
    }
}