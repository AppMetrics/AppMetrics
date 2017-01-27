// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Net;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http
{
    // ReSharper restore CheckNamespace
    internal static class HttpContextExtensions
    {
        private static readonly string MetricsCurrentRouteName = "__App.Metrics.CurrentRouteName__";

        public static void AddMetricsCurrentRouteName(this HttpContext context, string metricName)
        {
            context.Items.Add(MetricsCurrentRouteName, metricName);
        }

        public static string GetMetricsCurrentRouteName(this HttpContext context)
        {
            var route = context.Items[MetricsCurrentRouteName] as string;

            if (route.IsPresent())
            {
                return context.Request.Method + " " + context.Items[MetricsCurrentRouteName];
            }

            return context.Request.Method;
        }

        public static bool HasMetricsCurrentRouteName(this HttpContext context) { return context.Items.ContainsKey(MetricsCurrentRouteName); }

        public static bool IsSuccessfulResponse(this HttpResponse httpResponse)
        {
            return httpResponse.StatusCode >= (int)HttpStatusCode.OK && httpResponse.StatusCode <= 299;
        }

        public static string OAuthClientId(this HttpContext httpContext)
        {
            var claimsPrincipal = httpContext.User;
            var clientId = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == "client_id");

            return clientId?.Value;
        }
    }
}