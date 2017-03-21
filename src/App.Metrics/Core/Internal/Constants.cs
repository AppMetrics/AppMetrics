// <copyright file="Constants.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Metrics.Health;

namespace App.Metrics.Core.Internal
{
    internal static class Constants
    {
        public const string InternalMetricsContext = "appmetrics.internal";

        public static class Formatting
        {
            public static readonly string MetricNameDimensionSeparator = "|";
            public static readonly string MetricSetItemFallbackKey = "item";
            public static readonly char MetricSetItemKeyValueSeparator = ':';
            public static readonly char MetricSetItemSeparator = ',';
            public static readonly string MetricTagKeyValueSeparator = ":";
            public static readonly string MetricTagSeparator = ",";
        }

        public static class Health
        {
#pragma warning disable SA1008
            public static readonly (double healthy, double degraded, double unhealthy) HealthScore = (1.0, 0.5, 0.0);
#pragma warning restore SA1008

            internal const string DegradedStatusDisplay = "Degraded";
            internal const string HealthyStatusDisplay = "Healthy";
            internal const string UnhealthyStatusDisplay = "Unhealthy";

            public static class TagKeys
            {
                public const string HealthCheckName = "health_check_name";
                public const string HealthCheckStatus = "health_check_status";
            }

            public static ReadOnlyDictionary<HealthCheckStatus, string> HealthStatusDisplay =>
                new ReadOnlyDictionary<HealthCheckStatus, string>(
                    new Dictionary<HealthCheckStatus, string>
                    {
                        { HealthCheckStatus.Healthy, HealthyStatusDisplay },
                        { HealthCheckStatus.Unhealthy, UnhealthyStatusDisplay },
                        { HealthCheckStatus.Degraded, DegradedStatusDisplay },
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