// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using App.Metrics.Extensions.Middleware.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Middleware
{
    public class ApdexMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private const string ApdexItemsKey = "__App.Metrics.Apdex__";
        private readonly IApdex _apdexTracking;

        public ApdexMiddleware(
            RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, aspNetOptions, loggerFactory, metrics)
        {
            _apdexTracking = Metrics.Provider
                                    .Apdex
                                    .Instance(HttpRequestMetricsRegistry.ApdexScores.Apdex(aspNetOptions.ApdexTSeconds));
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context) && Options.ApdexTrackingEnabled)
            {
                Logger.MiddlewareExecuting(GetType());

                context.Items[ApdexItemsKey] = _apdexTracking.NewContext();

                await Next(context);

                var apdex = context.Items[ApdexItemsKey];

                using (apdex as IDisposable)
                {
                }

                context.Items.Remove(ApdexItemsKey);

                Logger.MiddlewareExecuted(GetType());
            }
            else
            {
                await Next(context);
            }
        }
    }
}