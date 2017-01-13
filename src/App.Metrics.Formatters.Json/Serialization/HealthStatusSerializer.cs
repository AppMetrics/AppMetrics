// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Formatters.Json.Converters;
using App.Metrics.Serialization.Interfaces;
using App.Metrics.Utils;
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

        public virtual T Deserialize<T>(string json) { return JsonConvert.DeserializeObject<T>(json, _settings); }

        public virtual string Serialize<T>(T value) { return JsonConvert.SerializeObject(value, _settings); }
    }
}