// <copyright file="HealthStatusSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.Serialization;
using App.Metrics.Formatters.Json.Converters;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Serialization
{
    public class HealthStatusSerializer : IHealthStatusSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public HealthStatusSerializer(IClock clock)
        {
            _settings = new JsonSerializerSettings
                        {
                            ContractResolver = new MetricContractResolver(),
                            Formatting = Newtonsoft.Json.Formatting.Indented,
                            NullValueHandling = NullValueHandling.Ignore
                        };

            _settings.Converters.Add(new HealthStatusConverter(clock));
        }

        public HealthStatusSerializer(JsonSerializerSettings serializerSettings) { _settings = serializerSettings; }

        public virtual T Deserialize<T>(string value) { return JsonConvert.DeserializeObject<T>(value, _settings); }

        public virtual string Serialize<T>(T value) { return JsonConvert.SerializeObject(value, _settings); }
    }
}