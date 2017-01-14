// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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