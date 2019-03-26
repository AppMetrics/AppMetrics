// <copyright file="DefaultHealthResponseWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;

namespace App.Metrics.AspNetCore.Health.Core
{
    public class DefaultHealthResponseWriter : IHealthResponseWriter
    {
        private readonly IHealthOutputFormatter _fallbackFormatter;
        private readonly IHealthOutputFormatter _formatter;
        private readonly HealthFormatterCollection _formatters;

        public DefaultHealthResponseWriter(
            IHealthOutputFormatter fallbackFormatter,
            IReadOnlyCollection<IHealthOutputFormatter> formatters)
        {
            if (formatters == null)
            {
                throw new ArgumentNullException(nameof(formatters));
            }

            _formatters = new HealthFormatterCollection(formatters.ToList());
            _fallbackFormatter = fallbackFormatter;
        }

        // ReSharper disable UnusedMember.Global
        public DefaultHealthResponseWriter(IHealthOutputFormatter formatter)
            // ReSharper restore UnusedMember.Global
        {
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, HealthStatus healthStatus, CancellationToken token = default)
        {
            var formatter = _formatter ?? context.Request.GetTypedHeaders().ResolveFormatter(
                                _fallbackFormatter,
                                metricsMediaTypeValue => _formatters.GetType(metricsMediaTypeValue));

            context.SetNoCacheHeaders();

            if (formatter == default(IHealthOutputFormatter))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                context.Response.Headers[HeaderNames.ContentType] = new[] { context.Request.ContentType };
                return Task.CompletedTask;
            }

            SetResponseStatusCode(context, healthStatus);

            context.Response.Headers[HeaderNames.ContentType] = new[] { formatter.MediaType.ContentType };

            return formatter.WriteAsync(context.Response.Body, healthStatus, token);
        }

        private static void SetResponseStatusCode(HttpContext context, HealthStatus healthStatus)
        {
            var responseStatusCode = StatusCodes.Status200OK;

            if (healthStatus.Status.IsUnhealthy())
            {
                responseStatusCode = StatusCodes.Status503ServiceUnavailable;
            }

            if (healthStatus.Status.IsDegraded())
            {
                responseStatusCode = StatusCodes.Status200OK;
                context.Response.Headers[HeaderNames.Warning] = new[] { "Warning: 100 'Degraded'" };
            }

            context.Response.StatusCode = responseStatusCode;
        }
    }
}