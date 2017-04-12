// <copyright file="IReporterSettings.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Reporting;

namespace App.Metrics.Abstractions.Reporting
{
    public interface IReporterSettings
    {
        // ReSharper disable UnusedMember.Global
        CustomPackMetricDataKeys CustomDataKeys { get; }

        TimeSpan ReportInterval { get; }
        // ReSharper restore UnusedMember.Global
    }
}