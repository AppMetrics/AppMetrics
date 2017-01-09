// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Owin.Middleware
{
    public class ActiveRequestCounterEndpointMiddleware : AppMetricsMiddleware<OwinMetricsOptions>
    {
        public ActiveRequestCounterEndpointMiddleware(OwinMetricsOptions owinOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(owinOptions, loggerFactory, metrics)
        {
            if (owinOptions == null)
            {
                throw new ArgumentNullException(nameof(owinOptions));
            }
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            if (PerformMetric(environment))
            {
                Logger.MiddlewareExecuting(GetType());

                Metrics.IncrementActiveRequests();

                await Next(environment);

                Metrics.DecrementActiveRequests();

                Logger.MiddlewareExecuted(GetType());
            }
            else
            {
                await Next(environment);
            }
        }
    }
}