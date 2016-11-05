// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Reporting;

namespace App.Metrics.Extensions.Reporting.TextFile
{
    public interface ITextFileReporterSettings : IReporterSettings
    {
        string FileName { get; set; }
    }
}