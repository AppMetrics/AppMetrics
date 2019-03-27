// <copyright file="PingEndpointMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health.Endpoints.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class PingEndpointMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly ILogger<PingEndpointMiddleware> _logger;

        // ReSharper disable UnusedParameter.Local - next required by middleware components
        public PingEndpointMiddleware(
                RequestDelegate next,
                ILogger<PingEndpointMiddleware> logger)
            // ReSharper restore UnusedParameter.Local
        {
            _logger = logger;
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting<PingEndpointMiddleware>();

            context.Response.Headers["Content-Type"] = new[] { "text/plain" };
            context.SetNoCacheHeaders();

            context.Response.StatusCode = StatusCodes.Status200OK;

            await context.Response.WriteAsync("pong");

            _logger.MiddlewareExecuted<PingEndpointMiddleware>();
        }
    }
}