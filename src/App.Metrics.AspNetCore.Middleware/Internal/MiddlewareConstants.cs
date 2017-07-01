// <copyright file="MiddlewareConstants.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace App.Metrics.AspNetCore.Middleware.Internal
{
    [ExcludeFromCodeCoverage]
    internal static class MiddlewareConstants
    {
        public static class DefaultRoutePaths
        {
            public const string MetricsEndpoint = "/metrics";
            public const string MetricsTextEndpoint = "/metrics-text";
            public const string PingEndpoint = "/ping";
            public const string EnvironmentInfoEndpoint = "/env";
        }

        public static class DefaultTagKeys
        {
            public const string Route = "route";
            public const string Exception = "exception";
            public const string ClientId = "client_id";
            public const string HttpStatusCode = "http_status_code";
        }
    }
}