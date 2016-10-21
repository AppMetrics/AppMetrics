// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.



// ReSharper disable CheckNamespace
namespace App.Metrics
// ReSharper restore CheckNamespace
{
    internal static class MetricsContextExtensions
    {
        private const string OAuth2ClientWebRequestsContextName = "Application.OAuth2Client.WebRequests";
        private const string WebApplicationRequestsContextName = "Application.WebRequests";

        public static IMetricsContext GetOAuth2ClientWebRequestsContext(this IMetricsContext context)
        {
            return context.Group(OAuth2ClientWebRequestsContextName);
        }

        public static IMetricsContext GetWebApplicationContext(this IMetricsContext context)
        {
            return context.Group(WebApplicationRequestsContextName);
        }
    }
}