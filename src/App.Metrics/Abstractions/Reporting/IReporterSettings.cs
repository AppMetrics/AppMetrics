// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace App.Metrics.Abstractions.Reporting
{
    public interface IReporterSettings
    {
        TimeSpan ReportInterval { get; }
    }
}