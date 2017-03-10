// <copyright file="Constants.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Extensions.Middleware.Internal
{
    internal static class Constants
    {
        public static class DefaultRoutePaths
        {
            public const string HealthEndpoint = "/health";
            public const string MetricsEndpoint = "/metrics";
            public const string MetricsTextEndpoint = "/metrics-text";
            public const string PingEndpoint = "/ping";
        }
    }
}