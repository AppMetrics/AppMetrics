// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using AppMetrics;
using AspNet.Metrics.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Middleware
{
    public class PostAndPutRequestSizeHistogramMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public PostAndPutRequestSizeHistogramMiddleware(RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, aspNetOptions, loggerFactory, metrics)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                var httpMethod = context.Request.Method.ToUpperInvariant();

                if (httpMethod == "POST" || httpMethod == "PUT")
                {
                    if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                    {
                        Metrics.UpdatePostAndPutRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()));
                    }
                }

                Logger.MiddlewareExecuted(GetType());
            }

            await Next(context);
        }
    }
}