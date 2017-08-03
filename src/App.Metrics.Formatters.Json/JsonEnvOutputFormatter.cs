// <copyright file="JsonEnvOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public class JsonEnvOutputFormatter : IEnvOutputFormatter
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public JsonEnvOutputFormatter() { _serializerSettings = DefaultJsonSerializerSettings.CreateSerializerSettings(); }

        public JsonEnvOutputFormatter(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings ?? throw new ArgumentNullException(nameof(serializerSettings));
        }

        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("application", "vnd.appmetrics.env", "v1", "json");

        public Task WriteAsync(
            Stream output,
            EnvironmentInfo environmentInfo,
            Encoding encoding,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var json = JsonConvert.SerializeObject(environmentInfo, _serializerSettings);

            var bytes = encoding.GetBytes(json);

            return output.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }
    }
}
