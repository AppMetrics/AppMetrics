// <copyright file="HistogramConverter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Metrics.Histogram;

namespace App.Metrics.Formatters.Json.Converters
{
    public class HistogramConverter : JsonConverter<HistogramValueSource>
    {
        public override bool CanConvert(Type objectType) { return typeof(HistogramValueSource) == objectType; }
        
        public override HistogramValueSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var source = JsonSerializer.Deserialize<HistogramMetric>(reader.GetString());

            return source.FromSerializableMetric();
        }

        public override void Write(Utf8JsonWriter writer, HistogramValueSource value, JsonSerializerOptions options)
        {
            var source = (HistogramValueSource)value;

            var target = source.ToSerializableMetric();

            JsonSerializer.Serialize(writer, target);
        }
    }
}