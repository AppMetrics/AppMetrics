// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using System.Net;

// ReSharper disable CheckNamespace

namespace Microsoft.AspNetCore.Http
// ReSharper restore CheckNamespace
{
    internal static class HttpContextExtensions
    {
        private static readonly string MetricsCurrentRouteName = "__Mertics.CurrentRouteName__";

        public static void AddMetricsCurrentRouteName(this HttpContext context, string metricName)
        {
            context.Items.Add(MetricsCurrentRouteName, metricName);
        }

        public static string GetMetricsCurrentRouteName(this HttpContext context)
        {
            return context.Request.Method + " " + context.Items[MetricsCurrentRouteName];
        }

        public static bool HasMetricsCurrentRouteName(this HttpContext context)
        {
            return context.Items.ContainsKey(MetricsCurrentRouteName);
        }

        public static bool IsSuccessfulResponse(this HttpResponse httpResponse)
        {
            return (httpResponse.StatusCode >= (int)HttpStatusCode.OK) && (httpResponse.StatusCode <= 299);
        }

        public static string OAuthClientId(this HttpContext httpContext)
        {
            var claimsPrincipal = httpContext.User;
            var clientId = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == "client_id");

            return clientId?.Value;
        }
    }
}