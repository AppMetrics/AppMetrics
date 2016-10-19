// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Logging
{
    internal static class MetricsRouteHandlerLoggerExtensions
    {
        public static void AttributeTemplateFound(this ILogger logger, string template)
        {
            logger.LogTrace(1, $"Found AttributeRouteTemplate {template}");
        }

        public static void TemplateRouteFound(this ILogger logger, string template)
        {
            logger.LogTrace(1, $"Found TemplateRoute {template}");
        }
    }
}