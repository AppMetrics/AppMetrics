// <copyright file="ApdexConverter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Metrics.Apdex;

namespace App.Metrics.Formatters.Json.Converters
{
    public class ApdexConverter : JsonConverter<ApdexValueSource>
    {
        public override bool CanConvert(Type objectType) { return typeof(ApdexValueSource) == objectType; }
        
        public override ApdexValueSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var source = JsonSerializer.Deserialize<ApdexMetric>(reader.GetString());
            return source.FromSerializableMetric();
        }

        public override void Write(Utf8JsonWriter writer, ApdexValueSource value, JsonSerializerOptions options)
        {
            var target = value.ToSerializableMetric();

            JsonSerializer.Serialize(writer, target);
        }
    }
}