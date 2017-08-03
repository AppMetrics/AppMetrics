// <copyright file="JsonOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public class JsonOutputFormatter : IMetricsOutputFormatter
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public JsonOutputFormatter() { _serializerSettings = DefaultJsonSerializerSettings.CreateSerializerSettings(); }

        public JsonOutputFormatter(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings ?? throw new ArgumentNullException(nameof(serializerSettings));
        }

        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("application", "vnd.appmetrics.metrics", "v1", "json");

        public Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricData,
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

            var json = JsonConvert.SerializeObject(metricData, _serializerSettings);

            var bytes = encoding.GetBytes(json);

            return output.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }
    }
}