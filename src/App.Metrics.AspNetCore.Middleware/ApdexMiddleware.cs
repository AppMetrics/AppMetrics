// <copyright file="ApdexMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.AspNetCore.Middleware.Internal;
using App.Metrics.AspNetCore.Middleware.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class ApdexMiddleware : AppMetricsMiddleware<AppMetricsMiddlewareOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private const string ApdexItemsKey = "__App.Metrics.Apdex__";
        private readonly IApdex _apdexTracking;

        public ApdexMiddleware(
            RequestDelegate next,
            AppMetricsMiddlewareOptions appMiddlewareOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, appMiddlewareOptions, loggerFactory, metrics)
        {
            _apdexTracking = Metrics.Provider
                                    .Apdex
                                    .Instance(HttpRequestMetricsRegistry.ApdexScores.Apdex(appMiddlewareOptions.ApdexTSeconds));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
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