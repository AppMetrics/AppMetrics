// <copyright file="AsciiEnvironmentInfoResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Infrastructure;
using App.Metrics.Middleware;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Formatters.Ascii
{
    public class AsciiEnvironmentInfoResponseWriter : IEnvironmentInfoResponseWriter
    {
        /// <inheritdoc />
        public string ContentType => "text/plain; app.metrics=vnd.app.metrics.v1.environment.info;";

        public Task WriteAsync(HttpContext context, EnvironmentInfo environmentInfo, CancellationToken token = default(CancellationToken))
        {
            var result = new StringWriter();
            var formatter = new AsciiEnvironmentInfoFormatter(environmentInfo);
            formatter.Format(result);

            return context.Response.WriteAsync(result.ToString(), token);
        }
    }
}