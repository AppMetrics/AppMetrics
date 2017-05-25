// <copyright file="AsciiMetricsResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Extensions.Middleware.Abstractions;
using App.Metrics.Formatting;
using App.Metrics.Formatting.Ascii;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.Formatters.Ascii
{
    public class AsciiMetricsResponseWriter : IMetricsResponseWriter
    {
        /// <inheritdoc />
        public string ContentType => "text/vnd.app.metrics.v1.metrics+plain";

        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken))
        {
            var payloadBuilder = new AsciiMetricPayloadBuilder();

            var formatter = new MetricDataValueSourceFormatter();
            formatter.Build(metricsData, payloadBuilder);

            return context.Response.WriteAsync(payloadBuilder.PayloadFormatted(), token);
        }
    }
}