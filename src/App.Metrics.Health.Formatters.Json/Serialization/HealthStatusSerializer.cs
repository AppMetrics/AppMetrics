// <copyright file="HealthStatusSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Health.Formatters.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Metrics.Health.Formatters.Json.Serialization
{
    public class HealthStatusSerializer : IHealthStatusSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public HealthStatusSerializer()
        {
            _settings = new JsonSerializerSettings
                        {
                            Formatting = Formatting.Indented,
                            NullValueHandling = NullValueHandling.Ignore
                        };

            _settings.Converters.Add(new HealthStatusConverter());
            _settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public HealthStatusSerializer(JsonSerializerSettings serializerSettings) { _settings = serializerSettings; }

        public virtual T Deserialize<T>(string value) { return JsonConvert.DeserializeObject<T>(value, _settings); }

        public virtual string Serialize<T>(T value) { return JsonConvert.SerializeObject(value, _settings); }
    }
}