// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Internal;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Logging
{
    // ReSharper restore CheckNamespace
    [AppMetricsExcludeFromCodeCoverage]
    internal static class AspNetMetricsCoreLoggerExtensions
    {
        public static void MiddlewareExecuted(this ILogger logger, Type middleware)
        {
            logger.LogTrace(AspNetMetricsEventIds.Middleware.MiddlewareExecutedId, $"Executed AspNet Metrics Middleware {middleware.FullName}");
        }

        public static void MiddlewareExecuting(this ILogger logger, Type middleware)
        {
            logger.LogTrace(AspNetMetricsEventIds.Middleware.MiddlewareExecutingId, $"Executing AspNet Metrics Middleware {middleware.FullName}");
        }

        internal static class AspNetMetricsEventIds
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
