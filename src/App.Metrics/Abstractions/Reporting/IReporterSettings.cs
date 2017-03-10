// <copyright file="IReporterSettings.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Abstractions.Reporting
{
    public interface IReporterSettings
    {
        TimeSpan ReportInterval { get; }
    }
}