// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Internal;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Logging
// ReSharper restore CheckNamespace
{
    [AppMetricsExcludeFromCodeCoverage]
    internal static class OwinMetricsCoreLoggerExtensions
    {
        public static void MiddlewareExecuted(this ILogger logger, Type middleware)
        {
            logger.LogDebug(OWinMetricsEventIds.Middleware.MiddlewareExecutedId, $"Executed Owin Metrics Middleare {middleware.FullName}");
        }

        public static void MiddlewareExecuting(this ILogger logger, Type middleware)
        {
            logger.LogDebug(OWinMetricsEventIds.Middleware.MiddlewareExecutingId, $"Executing Owin Metrics Middleare {middleware.FullName}");
        }

        internal static class OWinMetricsEventIds
        {
            public static class Middleware
            {
                public const int MiddlewareExecutedId = MiddlewareStart + 1;
                public const int MiddlewareExecutingId = MiddlewareStart + 2;
                private const int MiddlewareStart = 19999;
            }
        }
    }
}