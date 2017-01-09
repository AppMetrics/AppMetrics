// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Owin.Middleware
{
    public class PerRequestTimerMiddleware : AppMetricsMiddleware<OwinMetricsOptions>
    {
        private const string TimerItemsKey = "__Mertics.PerRequestStartTime__";

        public PerRequestTimerMiddleware(OwinMetricsOptions owinOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(owinOptions, loggerFactory, metrics)
        {
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            if (PerformMetric(environment))
            {
                Logger.MiddlewareExecuting(GetType());

                var httpResponseStatusCode = int.Parse(environment["owin.ResponseStatusCode"].ToString());

                environment[TimerItemsKey] = Metrics.Advanced.Clock.Nanoseconds;

                await Next(environment);

                if (environment.HasMetricsCurrentRouteName() && httpResponseStatusCode != (int)HttpStatusCode.NotFound)
                {
                    var clientId = environment.OAuthClientId();

                    var startTime = (long)environment[TimerItemsKey];
                    var elapsed = Metrics.Advanced.Clock.Nanoseconds - startTime;

                    Metrics.RecordEndpointRequestTime(clientId, environment.GetMetricsCurrentRouteName(), elapsed);
                }

                Logger.MiddlewareExecuted(GetType());
            }
            else
            {
                await Next(environment);
            }
        }
    }
}