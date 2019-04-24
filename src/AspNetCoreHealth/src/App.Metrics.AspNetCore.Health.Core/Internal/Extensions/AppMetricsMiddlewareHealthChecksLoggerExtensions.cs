// <copyright file="AppMetricsMiddlewareHealthChecksLoggerExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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
        public static void MiddlewareExecuted<TMiddleware>(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(AppMetricsEventIds.Middleware.MiddlewareExecutedId, $"Executed App Metrics Health Middleware {typeof(TMiddleware).FullName}");
            }
        }

        public static void MiddlewareFailed<TMiddleware>(this ILogger logger, Exception e, string message)
        {
            logger.LogError(AppMetricsEventIds.Middleware.MiddlewareErrorId, e, $"[{typeof(TMiddleware).FullName}] {message}");
        }

        public static void MiddlewareExecuting<TMiddleware>(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(AppMetricsEventIds.Middleware.MiddlewareExecutingId, $"Executing App Metrics Health Middleware {typeof(TMiddleware).FullName}");
            }
        }

        private static class AppMetricsEventIds
        {
            public static class Middleware
            {
                public const int MiddlewareExecutedId = 1;
                public const int MiddlewareExecutingId = 2;
                public const int MiddlewareErrorId = 3;
            }
        }
    }
}
