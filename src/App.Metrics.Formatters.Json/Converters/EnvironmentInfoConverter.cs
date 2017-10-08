// <copyright file="EnvironmentInfoConverter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Infrastructure;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Converters
{
    public class EnvironmentInfoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) { return typeof(EnvironmentInfo) == objectType; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<Dictionary<string, string>>(reader);

            var target = new EnvironmentInfo(source);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (EnvironmentInfo)value;

            var target = source.Entries;

            serializer.Serialize(writer, target);
        }
    }
}