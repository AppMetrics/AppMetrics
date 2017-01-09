// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using App.Metrics.Internal;
using App.Metrics.Serialization.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Owin.Middleware
{
    public class HealthCheckEndpointMiddleware : AppMetricsMiddleware<OwinMetricsOptions>
    {
        private readonly IHealthStatusSerializer _serializer;

        public HealthCheckEndpointMiddleware(OwinMetricsOptions owinOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IHealthStatusSerializer serializer)
            : base(owinOptions, loggerFactory, metrics)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            _serializer = serializer;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var requestPath = environment["owin.RequestPath"] as string;
            if (Options.HealthEndpointEnabled &&
                Options.HealthEndpoint.IsPresent() &&
                Options.HealthEndpoint == requestPath)
            {
                Logger.MiddlewareExecuting(GetType());

                //TODO: AH - confirm cancellation token is correct
                var cancellationToken = (CancellationToken)environment["owin.CallCancelled"];
                var healthStatus = await Metrics.Advanced.Health.ReadStatusAsync(cancellationToken);
                string warning = null;

                var responseStatusCode = HttpStatusCode.OK;

                if (healthStatus.Status.IsUnhealthy())
                {
                    responseStatusCode = HttpStatusCode.InternalServerError;
                }

                if (healthStatus.Status.IsDegraded())
                {
                    responseStatusCode = HttpStatusCode.OK;
                    warning = Constants.Health.DegradedStatusDisplay;
                }

                var json = _serializer.Serialize(healthStatus);

                await WriteResponseAsync(environment, json, "application/json", responseStatusCode, warning);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(environment);
        }
    }
}