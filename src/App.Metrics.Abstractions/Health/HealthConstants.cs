// <copyright file="HealthConstants.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Metrics.Health
{
    public class HealthConstants
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

        public static class TagKeys
        {
            public const string HealthCheckName = "health_check_name";
        }
    }
}
