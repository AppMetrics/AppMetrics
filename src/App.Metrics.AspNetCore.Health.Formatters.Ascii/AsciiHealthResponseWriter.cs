// <copyright file="AsciiHealthResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Formatting;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Health.Formatters.Ascii
{
    public class AsciiHealthResponseWriter : IHealthResponseWriter
    {
        /// <inheritdoc />
        public string ContentType => "text/plain; app.metrics=vnd.app.metrics.v1.health;";

        public Task WriteAsync(HttpContext context, HealthStatus healthStatus, CancellationToken token = default(CancellationToken))
        {
            var payloadBuilder = new AsciiHealthStatusPayloadBuilder();

            var formatter = new HealthStatusPayloadFormatter();
            formatter.Build(healthStatus, payloadBuilder);

            return context.Response.WriteAsync(payloadBuilder.PayloadFormatted(), token);
        }
    }
}