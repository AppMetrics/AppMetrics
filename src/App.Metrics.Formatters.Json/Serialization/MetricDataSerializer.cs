// <copyright file="MetricDataSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.Serialization;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Serialization
{
    public class MetricDataSerializer : JsonSerializer, IMetricDataSerializer
    {
        private readonly JsonSerializerSettings _settings;

        // ReSharper disable UnusedMember.Global
        public MetricDataSerializer()
            // ReSharper restore UnusedMember.Global
        {
            _settings = new JsonSerializerSettings
                        {
                            ContractResolver = new MetricContractResolver(),
                            Formatting = Newtonsoft.Json.Formatting.Indented,
                            NullValueHandling = NullValueHandling.Ignore,
                        };
        }

        public MetricDataSerializer(JsonSerializerSettings jsonSerializerSettings) { _settings = jsonSerializerSettings; }

        public virtual T Deserialize<T>(string value) { return JsonConvert.DeserializeObject<T>(value, _settings); }

        public virtual string Serialize<T>(T value) { return JsonConvert.SerializeObject(value, _settings); }
    }
}