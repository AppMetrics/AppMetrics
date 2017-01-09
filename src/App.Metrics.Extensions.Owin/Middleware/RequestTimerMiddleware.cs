// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using App.Metrics.Extensions.Owin.Internal;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Owin.Middleware
{
    public class RequestTimerMiddleware : AppMetricsMiddleware<OwinMetricsOptions>
    {
        private const string TimerItemsKey = "__App.Mertics.RequestTimer__";
        private readonly ITimer _requestTimer;

        public RequestTimerMiddleware(
            OwinMetricsOptions owinOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(owinOptions, loggerFactory, metrics)
        {
            _requestTimer = Metrics.Advanced
                .Timer(OwinMetricsRegistry.Contexts.HttpRequests.Timers.WebRequestTimer);
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            if (PerformMetric(environment))
            {
                Logger.MiddlewareExecuting(GetType());

                environment[TimerItemsKey] = _requestTimer.NewContext();

                await Next(environment);

                var timer = environment[TimerItemsKey];
                using (timer as IDisposable)
                {
                }
                environment.Remove(TimerItemsKey);

                Logger.MiddlewareExecuted(GetType());
            }
            else
            {
                await Next(environment);
            }
        }
    }
}