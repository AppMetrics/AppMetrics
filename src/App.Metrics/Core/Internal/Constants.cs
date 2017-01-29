// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Metrics.Health;

namespace App.Metrics.Core.Internal
{
    internal static class Constants
    {
        public const string InternalMetricsContext = "appmetrics.internal";

        public static class Health
        {
            internal const string DegradedStatusDisplay = "Degraded";
            internal const string HealthyStatusDisplay = "Healthy";
            internal const string UnhealthyStatusDisplay = "Unhealthy";

            public static ReadOnlyDictionary<HealthCheckStatus, string> HealthStatusDisplay =>
                new ReadOnlyDictionary<HealthCheckStatus, string>(
                    new Dictionary<HealthCheckStatus, string>
                    {
                        { HealthCheckStatus.Healthy, HealthyStatusDisplay },
                        { HealthCheckStatus.Unhealthy, UnhealthyStatusDisplay },
                        { HealthCheckStatus.Degraded, DegradedStatusDisplay }
                    });
        }

        public static class ReservoirSampling
        {
            public const int ApdexRequiredSamplesBeforeCalculating = 100;
            public const double DefaultApdexTSeconds = 0.5;
            public const double DefaultExponentialDecayFactor = 0.015;
            public const int DefaultSampleSize = 1028;
        }
    }
}