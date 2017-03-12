// <copyright file="MetricContractResolver.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Core;
using App.Metrics.Counter;
using App.Metrics.Formatters.Json.Converters;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Metrics.Formatters.Json.Serialization
{
    public class MetricContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            if (objectType == typeof(CounterValueSource))
            {
                contract.Converter = new CounterConverter();
            }

            if (objectType == typeof(MeterValueSource))
            {
                contract.Converter = new MeterConverter();
            }

            if (objectType == typeof(GaugeValueSource))
            {
                contract.Converter = new GaugeConverter();
            }

            if (objectType == typeof(TimerValueSource))
            {
                contract.Converter = new TimerConverter();
            }

            if (objectType == typeof(HistogramValueSource))
            {
                contract.Converter = new HistogramConverter();
            }

            if (objectType == typeof(ApdexValueSource))
            {
                contract.Converter = new ApdexConverter();
            }

            if (objectType == typeof(MetricsDataValueSource))
            {
                contract.Converter = new MetricDataConverter();
            }

            return contract;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // DEVNOTE: Writable properties only
            var props = base.CreateProperties(type, memberSerialization);
            return props.Where(p => p.Writable).ToList();
        }
    }
}