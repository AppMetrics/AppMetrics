// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.



// ReSharper disable CheckNamespace

using AspNet.Metrics.Internal;

namespace App.Metrics
// ReSharper restore CheckNamespace
{
    internal static class MetricsContextExtensions
    {
        public static IMetricsContext GetOAuth2ClientWebRequestsContext(this IMetricsContext context)
        {
            return context.Group(AspNetMetricsRegistry.Groups.OAuth2.GroupName);
        }

        public static IMetricsContext GetWebApplicationContext(this IMetricsContext context)
        {
            //TODO: AH - Anyway to add this to the metrics options?
            return context.Group(AspNetMetricsRegistry.Groups.WebRequests.GroupName);
        }
    }
}