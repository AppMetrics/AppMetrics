// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace App.Metrics.Reporting
{
    public class TextFileReporterSettings : ITextFileReporterSettings
    {
        public bool Disabled { get; set; } = false;

        public IMetricsFilter Filter { get; set; }

        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);

        public string FileName { get; set; }
    }
}