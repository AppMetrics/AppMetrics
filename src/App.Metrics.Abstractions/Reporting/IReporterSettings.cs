// <copyright file="IReporterSettings.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Reporting
{
    public interface IReporterSettings
    {
        MetricValueDataKeys DataKeys { get; }

        TimeSpan ReportInterval { get; }
    }
}