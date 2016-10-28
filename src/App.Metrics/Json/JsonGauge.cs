// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public sealed class JsonGauge : JsonMetric
    {
        private double _value;

        public double? Value
        {
            get { return _value; }
            set { _value = value ?? double.NaN; }
        }

        public static JsonGauge FromGauge(MetricValueSource<double> gauge)
        {
            return new JsonGauge
            {
                Name = gauge.Name,
                Value = gauge.Value,
                Unit = gauge.Unit.Name,
                Tags = gauge.Tags
            };
        }

        public JsonObject ToJsonObject()
        {
            return new JsonObject(ToJsonProperties());
        }

        public IEnumerable<JsonProperty> ToJsonProperties()
        {
            yield return new JsonProperty("Name", Name);
            yield return new JsonProperty("Value", Value.Value);
            yield return new JsonProperty("Unit", Unit);

            if (Tags.Length > 0)
            {
                yield return new JsonProperty("Tags", Tags);
            }
        }

        public GaugeValueSource ToValueSource()
        {
            return new GaugeValueSource(Name, ConstantValue.Provider(Value.Value), Unit, Tags);
        }
    }
}