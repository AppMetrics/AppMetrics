// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Reporting;

namespace App.Metrics.Extensions.Reporting.TextFile
{
    public class TextFileReporterSettings : IReporterSettings
    {
        public string FileName { get; set; }

        public TimeSpan ReportInterval { get; set; } = TimeSpan.FromSeconds(5);
    }
}