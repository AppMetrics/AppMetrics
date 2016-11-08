// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Middleware
{
    public abstract class AppMetricsMiddleware<TOptions> where TOptions : AspNetMetricsOptions, new()
    {
        protected AppMetricsMiddleware(RequestDelegate next,
            TOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (aspNetOptions == null)
            {
                throw new ArgumentNullException(nameof(aspNetOptions));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            Options = aspNetOptions;
            Logger = loggerFactory.CreateLogger(this.GetType().FullName);
            MetricsContext = metricsContext;

            Next = next;
        }

        public ILogger Logger { get; set; }

        public IMetricsContext MetricsContext { get; set; }

        public RequestDelegate Next { get; set; }

        public TOptions Options { get; set; }

        protected bool PerformMetric(HttpContext context)
        {
            if (Options.IgnoredRequestPatterns == null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(context.Request.Path))
            {
                return false;
            }

            return !Options.IgnoredRequestPatterns.Any(ignorePattern => ignorePattern.IsMatch(context.Request.Path.ToString().TrimStart('/')));
        }

        protected Task WriteResponseAsync(HttpContext context, string content, string contentType,
            HttpStatusCode code = HttpStatusCode.OK)
        {
            context.Response.Headers["Content-Type"] = new[] { contentType };
            context.Response.Headers["Cache-Control"] = new[] { "no-cache, no-store, must-revalidate" };
            context.Response.Headers["Pragma"] = new[] { "no-cache" };
            context.Response.Headers["Expires"] = new[] { "0" };
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(content);
        }
    }
}