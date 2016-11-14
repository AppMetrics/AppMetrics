// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Middleware
{
    public abstract class AppMetricsMiddleware<TOptions> where TOptions : AspNetMetricsOptions, new()
    {
        private static IReadOnlyList<Regex> _ignoredRoutes;

        private static Func<PathString, bool> _shouldRecordMetric;

        protected AppMetricsMiddleware(RequestDelegate next,
            TOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
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

            if (metrics == null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }

            Options = aspNetOptions;
            Logger = loggerFactory.CreateLogger(this.GetType().FullName);
            Metrics = metrics;

            Next = next;

            _ignoredRoutes = Options.IgnoredRoutesRegexPatterns
                .Select(p => new Regex(p, RegexOptions.Compiled | RegexOptions.IgnoreCase)).ToList();

            if (_ignoredRoutes.Any())
            {
                _shouldRecordMetric = path => !_ignoredRoutes.Any(ignorePattern => ignorePattern.IsMatch(path.ToString()
                    .RemoveLeadingSlash()));
            }
            else
            {
                _shouldRecordMetric = path => true;
            }
        }

        public ILogger Logger { get; set; }

        public IMetrics Metrics { get; set; }

        public RequestDelegate Next { get; set; }

        public TOptions Options { get; set; }

        protected bool PerformMetric(HttpContext context)
        {
            if (Options.IgnoredRoutesRegexPatterns == null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(context.Request.Path))
            {
                return false;
            }

            return _shouldRecordMetric(context.Request.Path);
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