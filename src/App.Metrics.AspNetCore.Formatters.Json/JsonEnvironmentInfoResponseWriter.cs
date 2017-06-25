// <copyright file="JsonEnvironmentInfoResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters.Json.Serialization;
using App.Metrics.Infrastructure;
using App.Metrics.Middleware;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Formatters.Json
{
    public class JsonEnvironmentInfoResponseWriter : IEnvironmentInfoResponseWriter
    {
        private readonly IEnvironmentInfoSerializer _serializer;

        public JsonEnvironmentInfoResponseWriter(IEnvironmentInfoSerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc />
        public string ContentType => "application/vnd.app.metrics.v1.environment.info+json";

        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, EnvironmentInfo environmentInfo, CancellationToken token = default(CancellationToken))
        {
            var json = _serializer.Serialize(environmentInfo);

            return context.Response.WriteAsync(json, token);
        }
    }
}