// <copyright file="AppMetricsMiddlewareHealthChecksLoggerExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Logging
    // ReSharper restore CheckNamespace
{
    [ExcludeFromCodeCoverage]
    internal static class AppMetricsMiddlewareHealthChecksLoggerExtensions
    {
        public static void MiddlewareExecuted(this ILogger logger, Type middleware)
        {
            logger.LogTrace(AppMetricsEventIds.Middleware.MiddlewareExecutedId, $"Executed AspNet Metrics Middleware Health Checks {middleware.FullName}");
        }

        public static void MiddlewareExecuting(this ILogger logger, Type middleware)
        {
            logger.LogTrace(AppMetricsEventIds.Middleware.MiddlewareExecutingId, $"Executing AspNet Metrics Middleware Health Checks {middleware.FullName}");
        }

        private static class AppMetricsEventIds
        {
            public static class Middleware
            {
                public const int MiddlewareExecutedId = MiddlewareStart + 1;
                public const int MiddlewareExecutingId = MiddlewareStart + 2;
                private const int MiddlewareStart = 3000;
            }
        }
    }
}
