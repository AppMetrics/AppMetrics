// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Owin.Middleware
{
    public class PostAndPutRequestSizeHistogramMiddleware : AppMetricsMiddleware<OwinMetricsOptions>
    {
        public PostAndPutRequestSizeHistogramMiddleware(
            OwinMetricsOptions owinOptions,
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

                var httpMethod = environment["owin.RequestMethod"].ToString().ToUpper();

                if (httpMethod == "POST" || httpMethod == "PUT")
                {
                    var headers = (IDictionary<string, string[]>)environment["owin.RequestHeaders"];
                    if (headers != null && headers.ContainsKey("Content-Length"))
                    {
                        Metrics.UpdatePostAndPutRequestSize(long.Parse(headers["Content-Length"].First()));
                    }
                }

                Logger.MiddlewareExecuted(GetType());
            }

            await Next(environment);
        }
    }
}