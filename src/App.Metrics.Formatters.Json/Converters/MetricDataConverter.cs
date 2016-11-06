using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Infrastructure;
using App.Metrics.Json;
using App.Metrics.MetricData;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public class MetricDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(MetricsData) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<JsonMetricsData>(reader);
            var groups = new HashSet<MetricsDataGroup>();

            foreach (var group in source.Groups)
            {
                var jsonCoutners = group.Counters.FromJson();
                var jsonMeters = group.Meters.FromJson();
                var jsonGauges = group.Gauges.FromJson();
                var jsonHistograms = group.Histograms.FromJson();
                var jsonTimers = group.Timers.FromJson();

                groups.Add(new MetricsDataGroup(group.GroupName, jsonGauges, jsonCoutners, jsonMeters, jsonHistograms, jsonTimers));
            }

            return new MetricsData(source.ContextName, source.Timestamp, new EnvironmentInfo(source.Environment), groups);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (MetricsData)value;

            var jsonGroups = new HashSet<JsonMetricsGroup>();

            foreach (var group in source.Groups)
            {
                var jsonCoutners = group.Counters.ToJson();
                var jsonMeters = group.Meters.ToJson();
                var jsonGauges = group.Gauges.ToJson();
                var jsonHistograms = group.Histograms.ToJson();
                var jsonTimers = group.Timers.ToJson();

                jsonGroups.Add(new JsonMetricsGroup
                {
                    Counters = jsonCoutners,
                    Meters = jsonMeters,
                    Gauges = jsonGauges,
                    Histograms = jsonHistograms,
                    Timers = jsonTimers,
                    GroupName = group.GroupName
                });
            }

            var target = new JsonMetricsData
            {
                ContextName = source.ContextName,
                Environment = source.Environment.ToEnvDictionary(),
                Timestamp = source.Timestamp,
                Version = "1",
                Groups = jsonGroups.ToArray()
            };

            serializer.Serialize(writer, target);
        }
    }
}