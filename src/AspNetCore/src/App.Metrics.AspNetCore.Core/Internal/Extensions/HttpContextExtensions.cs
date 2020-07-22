// <copyright file="HttpContextExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http
    // ReSharper restore CheckNamespace
{
    internal static class HttpContextExtensions
    {
        public static bool IsSuccessfulResponse(this HttpResponse httpResponse)
        {
            return httpResponse.StatusCode < StatusCodes.Status400BadRequest;
        }

        public static void SetNoCacheHeaders(this HttpContext context)
        {
            context.Response.Headers["Cache-Control"] = new[] { "no-cache, no-store, must-revalidate" };
            context.Response.Headers["Pragma"] = new[] { "no-cache" };
            context.Response.Headers["Expires"] = new[] { "0" };
        }
    }
}