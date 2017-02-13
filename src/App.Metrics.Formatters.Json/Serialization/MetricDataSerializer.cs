// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.Serialization;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Serialization
{
    public class MetricDataSerializer : JsonSerializer, IMetricDataSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public MetricDataSerializer()
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