// <copyright file="MetricsHttpContextExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.AspNetCore.Tracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http
    // ReSharper restore CheckNamespace
{
    internal static class MetricsHttpContextExtensions
    {
        public static string GetOAuthClientIdIfRequired(this HttpContext context)
        {
            var optionsAccessor = context.RequestServices.GetRequiredService<IOptions<MetricsWebTrackingOptions>>();
            return optionsAccessor.Value.OAuth2TrackingEnabled ? context.OAuthClientId() : null;
        }

        public static string OAuthClientId(this HttpContext httpContext)
        {
            var claimsPrincipal = httpContext.User;
            var clientId = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == "client_id");

            return clientId?.Value;
        }
    }
}