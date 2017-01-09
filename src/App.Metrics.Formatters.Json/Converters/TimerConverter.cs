// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Data;
using App.Metrics.Extensions;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Converters
{
    public class TimerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(TimerValueSource) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<Timer>(reader);

            return source.ToMetricValueSource();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (TimerValueSource)value;

            var target = source.ToMetric();

            serializer.Serialize(writer, target);
        }
    }
}