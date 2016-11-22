using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Metrics.Core;

namespace App.Metrics.Internal
{
    internal static class Constants
    {
        public static class Health
        {
            public const string DegradedStatusDisplay = "Degraded";
            public const string HealthyStatusDisplay = "Healthy";
            public const string UnhealthyStatusDisplay = "Unhealthy";

            public static ReadOnlyDictionary<HealthCheckStatus, string> HealthStatusDisplay =
                new ReadOnlyDictionary<HealthCheckStatus, string>(new Dictionary<HealthCheckStatus, string>
                {
                    { HealthCheckStatus.Healthy, HealthyStatusDisplay },
                    { HealthCheckStatus.Unhealthy, UnhealthyStatusDisplay },
                    { HealthCheckStatus.Degraded, DegradedStatusDisplay }
                });
        }
    }
}