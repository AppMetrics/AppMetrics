// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Meter;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Converters
{
    public class MeterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) { return typeof(MeterValueSource) == objectType; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<MeterMetric>(reader);

            return source.FromSerializableMetric();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (MeterValueSource)value;

            var target = source.ToSerializableMetric();

            serializer.Serialize(writer, target);
        }
    }
}