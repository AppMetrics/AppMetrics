// <copyright file="EnvironmentInfoConverter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Metrics.Infrastructure;

namespace App.Metrics.Formatters.Json.Converters
{
    public class EnvironmentInfoConverter : JsonConverter<EnvironmentInfo>
    {
        public override bool CanConvert(Type objectType) { return typeof(EnvironmentInfo) == objectType; }
        
        public override EnvironmentInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var source = JsonSerializer.Deserialize<Dictionary<string, string>>(reader.GetString(), options);

            return new EnvironmentInfo(source);
        }

        public override void Write(Utf8JsonWriter writer, EnvironmentInfo value, JsonSerializerOptions options)
        {
            var target = value.Entries;

            JsonSerializer.Serialize(writer, target, options);
        }
    }
}