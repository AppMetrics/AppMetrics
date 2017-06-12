// <copyright file="HealthMetricsRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex;

namespace App.Metrics.Health.Internal
{
    internal static class HealthMetricsRegistry
    {
#pragma warning disable SA1401
        public static string HttpRequestsContextName = "Application.HttpRequests";

        public static class ApdexScores
        {
            public static readonly string ApdexMetricName = "Apdex";

            public static readonly Func<double, ApdexOptions> Apdex = apdexTSeconds => new ApdexOptions
                                                                                       {
                                                                                           Context = HttpRequestsContextName,
                                                                                           Name = ApdexMetricName,
                                                                                           ApdexTSeconds = apdexTSeconds
                                                                                       };
        }
#pragma warning restore SA1401
    }
}
