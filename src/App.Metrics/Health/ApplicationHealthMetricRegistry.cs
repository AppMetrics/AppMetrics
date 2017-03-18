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
        public static readonly string Context = "Application.Health";
        // ReSharper restore MemberCanBePrivate.Global

        public static GaugeOptions Checks => new GaugeOptions
                                                     {
                                                         Context = Context,
                                                         Name = "Results",
                                                         MeasurementUnit = Unit.Items
                                                     };

        public static GaugeOptions HealthGauge => new GaugeOptions
                                                  {
                                                      Context = Context,
                                                      Name = "Score",
                                                      MeasurementUnit = Unit.Custom("Health Score")
                                                  };
    }
}