// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Net;

// ReSharper disable CheckNamespace

namespace Microsoft.AspNetCore.Http
// ReSharper restore CheckNamespace
{
    internal static class HttpContextExtensions
    {
        private static readonly string MetricsCurrentRouteName = "__Mertics.CurrentRouteName__";

        public static void AddMetricsCurrentRouteName(this IDictionary<string, object> environment, string metricName)
        {
            environment.Add(MetricsCurrentRouteName, metricName);
        }

        public static string GetMetricsCurrentRouteName(this IDictionary<string, object> environment)
        {
            var httpMethod = environment["owin.RequestMethod"].ToString().ToUpper();

            if (environment.ContainsKey(MetricsCurrentRouteName))
            {
                return httpMethod + " " + environment[MetricsCurrentRouteName];
            }
            return httpMethod;
        }

        public static bool HasMetricsCurrentRouteName(this IDictionary<string, object> environment)
        {
            return environment.ContainsKey(MetricsCurrentRouteName);
        }

        public static bool IsSuccessfulResponse(this IDictionary<string, object> environment)
        {
            var httpResponseStatusCode = int.Parse(environment["owin.ResponseStatusCode"].ToString());

            return (httpResponseStatusCode >= (int)HttpStatusCode.OK) && (httpResponseStatusCode <= 299);
        }

        public static string OAuthClientId(this IDictionary<string, object> environment)
        {
            //TODO: AH - owin get user from env
            //var claimsPrincipal = httpContext.User;
            //var clientId = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == "client_id");

            //return clientId?.Value;
            return string.Empty;
        }
    }
}