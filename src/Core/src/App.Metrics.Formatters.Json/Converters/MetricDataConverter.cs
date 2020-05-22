// <copyright file="MetricDataConverter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Metrics.Formatters.Json.Extensions;

namespace App.Metrics.Formatters.Json.Converters
{
    public class MetricDataConverter : JsonConverter<MetricsDataValueSource>
    {
        public override bool CanConvert(Type objectType) { return typeof(MetricsDataValueSource) == objectType; }
        
        public override MetricsDataValueSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var source = JsonSerializer.Deserialize<MetricData>(reader.GetString(), options);
            
            return source.ToMetricValueSource();
        }

        public override void Write(Utf8JsonWriter writer, MetricsDataValueSource value, JsonSerializerOptions options)
        {
            var target = value.ToMetric();

            JsonSerializer.Serialize(writer, target, options);
        }
    }
}