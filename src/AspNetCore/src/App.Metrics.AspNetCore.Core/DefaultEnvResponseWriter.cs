// <copyright file="DefaultEnvResponseWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;
using App.Metrics.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;

namespace App.Metrics.AspNetCore
{
    public class DefaultEnvResponseWriter : IEnvResponseWriter
    {
        private readonly IEnvOutputFormatter _fallbackFormatter;
        private readonly IEnvOutputFormatter _formatter;
        private readonly EnvFormatterCollection _formatters;

        public DefaultEnvResponseWriter(
            IEnvOutputFormatter fallbackFormatter,
            IReadOnlyCollection<IEnvOutputFormatter> formatters)
        {
            if (formatters == null)
            {
                throw new ArgumentNullException(nameof(formatters));
            }

            _formatters = new EnvFormatterCollection(formatters.ToList());
            _fallbackFormatter = fallbackFormatter ?? throw new ArgumentNullException(nameof(fallbackFormatter));
        }

        public DefaultEnvResponseWriter(IEnvOutputFormatter formatter)
        {
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public Task WriteAsync(HttpContext context, EnvironmentInfo environmentInfo, CancellationToken token = default)
        {
            var formatter = _formatter ?? context.Request.GetTypedHeaders().ResolveFormatter(
                                _fallbackFormatter,
                                metricsMediaTypeValue => _formatters.GetType(metricsMediaTypeValue));

            context.SetNoCacheHeaders();

            if (formatter == default(IEnvOutputFormatter))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                context.Response.Headers[HeaderNames.ContentType] = new[] { context.Request.ContentType };
                return Task.CompletedTask;
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.Headers[HeaderNames.ContentType] = new[] { formatter.MediaType.ContentType };

            return formatter.WriteAsync(context.Response.Body, environmentInfo, token);
        }
    }
}