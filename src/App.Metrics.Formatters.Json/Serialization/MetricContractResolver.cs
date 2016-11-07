using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Metrics.Formatters.Json
{
    public class MetricContractResolver : CamelCasePropertyNamesContractResolver
    {
        public static readonly MetricContractResolver Instance = new MetricContractResolver();

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

            if (objectType == typeof(MetricsDataValueSource))
            {
                contract.Converter = new MetricDataConverter();
            }

            return contract;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = base.CreateProperties(type, memberSerialization);
            return props.Where(p => p.Writable).ToList();
        }
    }
}