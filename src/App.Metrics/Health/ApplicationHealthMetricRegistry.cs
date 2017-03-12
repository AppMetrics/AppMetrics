// <copyright file="ApplicationHealthMetricRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Internal;
using App.Metrics.Core.Options;

namespace App.Metrics.Health
{
    [AppMetricsExcludeFromCodeCoverage]
    public static class ApplicationHealthMetricRegistry
    {
        // ReSharper disable MemberCanBePrivate.Global
        public static readonly string Context = "Health";
        // ReSharper restore MemberCanBePrivate.Global

        public static CounterOptions DegradedCheckCounter => new CounterOptions
                                                             {
                                                                 Context = Context,
                                                                 Name = "Degraded",
                                                                 MeasurementUnit = Unit.Items,
                                                                 ResetOnReporting = true,
                                                                 ReportItemPercentages = true,
                                                                 ReportSetItems = true
                                                             };

        public static CounterOptions HealthyCheckCounter => new CounterOptions
                                                            {
                                                                Context = Context,
                                                                Name = "Healthy",
                                                                MeasurementUnit = Unit.Items,
                                                                ResetOnReporting = true,
                                                                ReportItemPercentages = true,
                                                                ReportSetItems = true
                                                            };

        public static CounterOptions UnhealthyCheckCounter => new CounterOptions
                                                              {
                                                                  Context = Context,
                                                                  Name = "UnHealthy",
                                                                  MeasurementUnit = Unit.Items,
                                                                  ResetOnReporting = true,
                                                                  ReportItemPercentages = true,
                                                                  ReportSetItems = true
                                                              };
    }
}