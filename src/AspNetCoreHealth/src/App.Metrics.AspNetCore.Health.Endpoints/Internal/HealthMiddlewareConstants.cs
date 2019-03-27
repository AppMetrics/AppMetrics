// <copyright file="HealthMiddlewareConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.AspNetCore.Health.Endpoints.Internal
{
    internal static class HealthMiddlewareConstants
    {
        public static class DefaultRoutePaths
        {
            public const string HealthEndpoint = "/health";
            public const string PingEndpoint = "/ping";
        }
    }
}