// <copyright file="HealthConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Metrics.Health
{
    public static class HealthConstants
    {
        public const string DegradedStatusDisplay = "Degraded";
        public const string HealthyStatusDisplay = "Healthy";
        public const string UnhealthyStatusDisplay = "Unhealthy";
        public const string IgnoredStatusDisplay = "Ignored";

#pragma warning disable SA1008
        public static readonly (double healthy, double degraded, double unhealthy) HealthScore = (1.0, 0.5, 0.0);
#pragma warning restore SA1008

        public static ReadOnlyDictionary<HealthCheckStatus, string> HealthStatusDisplay =>
            new ReadOnlyDictionary<HealthCheckStatus, string>(
                new Dictionary<HealthCheckStatus, string>
                {
                    { HealthCheckStatus.Healthy, HealthyStatusDisplay },
                    { HealthCheckStatus.Unhealthy, UnhealthyStatusDisplay },
                    { HealthCheckStatus.Degraded, DegradedStatusDisplay },
                    { HealthCheckStatus.Ignored, IgnoredStatusDisplay }
                });

        public static class Reporting
        {
            public static readonly TimeSpan DefaultReportInterval = TimeSpan.FromSeconds(10);
            public static readonly int DefaultNumberOfRunsBeforeReAlerting = 6; // Every 60secs
        }
    }
}
