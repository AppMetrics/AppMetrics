// <copyright file="AppMetricsMiddlewareLoggerExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Logging
    // ReSharper restore CheckNamespace
{
    [ExcludeFromCodeCoverage]
    internal static class AppMetricsMiddlewareLoggerExtensions
    {
        public static void MiddlewareExecuted<TMiddleware>(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(AppMetricsEventIds.Middleware.MiddlewareExecutedId, $"Executed App Metrics Middleware {typeof(TMiddleware).FullName}");
            }
        }

        public static void MiddlewareExecuting<TMiddleware>(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(AppMetricsEventIds.Middleware.MiddlewareExecutingId, $"Executing App Metrics Middleware {typeof(TMiddleware).FullName}");
            }
        }

        private static class AppMetricsEventIds
        {
            public static class Middleware
            {
                public const int MiddlewareExecutedId = 1;
                public const int MiddlewareExecutingId = 2;
            }
        }
    }
}
