// <copyright file="EnvironmentInfoSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Formatters.Json.Converters;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Serialization
{
    public class EnvironmentInfoSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public EnvironmentInfoSerializer()
        {
            _settings = new JsonSerializerSettings
                        {
                            ContractResolver = new MetricContractResolver(),
                            Formatting = Newtonsoft.Json.Formatting.Indented,
                            NullValueHandling = NullValueHandling.Ignore
                        };

            _settings.Converters.Add(new EnvironmentInfoConverter());
        }

        public virtual T Deserialize<T>(string json) { return JsonConvert.DeserializeObject<T>(json, _settings); }

        public virtual string Serialize<T>(T value) { return JsonConvert.SerializeObject(value, _settings); }
    }
}