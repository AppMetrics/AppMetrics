// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Owin.Middleware
{
    public class PingEndpointMiddleware : AppMetricsMiddleware<OwinMetricsOptions>
    {
        public PingEndpointMiddleware(OwinMetricsOptions owinOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(owinOptions, loggerFactory, metrics)
        {
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var requestPath = environment["owin.RequestPath"] as string;

            if (Options.PingEndpointEnabled && Options.PingEndpoint.IsPresent() && Options.PingEndpoint == requestPath)
            {
                Logger.MiddlewareExecuting(GetType());

                await WriteResponseAsync(environment, "pong", "text/plain");

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(environment);
        }
    }
}