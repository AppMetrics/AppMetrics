// <copyright file="EnvInfoMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Endpoints.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class EnvInfoMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly ILogger<EnvInfoMiddleware> _logger;
        private readonly EnvironmentInfo _environmentInfo;
        private readonly IEnvResponseWriter _envResponseWriter;

        // ReSharper disable UnusedParameter.Local - next required by middleware components
        public EnvInfoMiddleware(
            RequestDelegate next,
            ILogger<EnvInfoMiddleware> logger,
            IEnvResponseWriter environmentInfoResponseWriter,
            EnvironmentInfoProvider environmentInfoProvider)
            // ReSharper restore UnusedParameter.Local
        {
            _logger = logger;
            _environmentInfo = environmentInfoProvider.Build();
            _envResponseWriter = environmentInfoResponseWriter ?? throw new ArgumentNullException(nameof(environmentInfoResponseWriter));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting<EnvInfoMiddleware>();

            await _envResponseWriter.WriteAsync(context, _environmentInfo, context.RequestAborted).ConfigureAwait(false);

            _logger.MiddlewareExecuted<EnvInfoMiddleware>();
        }
    }
}
