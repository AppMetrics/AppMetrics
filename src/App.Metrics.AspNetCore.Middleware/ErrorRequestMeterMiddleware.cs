// <copyright file="ErrorRequestMeterMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Middleware.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Middleware
{
    /// <summary>
    ///     Measures the overall error request rate as well as the rate per endpoint.
    ///     Also measures these error rates per OAuth2 Client as a separate metric
    /// </summary>
    // ReSharper disable ClassNeverInstantiated.Global
    public class ErrorRequestMeterMiddleware : AppMetricsMiddleware<AppMetricsMiddlewareOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        public ErrorRequestMeterMiddleware(
            RequestDelegate next,
            AppMetricsMiddlewareOptions appMiddlewareOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, appMiddlewareOptions, loggerFactory, metrics)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (appMiddlewareOptions == null)
            {
                throw new ArgumentNullException(nameof(appMiddlewareOptions));
            }

            if (metrics == null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            try
            {
                Logger.MiddlewareExecuting(GetType());

                await Next(context);

                if (PerformMetric(context))
                {
                    var routeTemplate = context.GetMetricsCurrentRouteName();

                    if (!context.Response.IsSuccessfulResponse() && ShouldTrackHttpStatusCode(context.Response.StatusCode))
                    {
                        Metrics.RecordHttpRequestError(routeTemplate, context.Response.StatusCode);
                    }
                }
            }
            catch (Exception exception)
            {
                if (!PerformMetric(context))
                {
                    throw;
                }

                var routeTemplate = context.GetMetricsCurrentRouteName();

                Metrics.RecordHttpRequestError(routeTemplate, (int)HttpStatusCode.InternalServerError);
                Metrics.RecordException(routeTemplate, exception.GetType().FullName);

                throw;
            }
            finally
            {
                Logger.MiddlewareExecuted(GetType());
            }
        }
    }
}