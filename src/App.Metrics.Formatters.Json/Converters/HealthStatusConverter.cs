using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Utils;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public class HealthStatusConverter : JsonConverter
    {
        private readonly IClock _clock;

        public HealthStatusConverter(IClock clock)
        {
            _clock = clock;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(HealthStatus) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<HealthStatusData>(reader);
            var healthy = source.Healthy.Keys.Select(k => new HealthCheck.Result(k, HealthCheckResult.Healthy(source.Healthy[k])));
            var unhealthy = source.Unhealthy.Keys.Select(k => new HealthCheck.Result(k, HealthCheckResult.Unhealthy(source.Unhealthy[k])));
            var target = new HealthStatus(healthy.Concat(unhealthy));
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (HealthStatus)value;

            var target = new HealthStatusData
            {
                IsHealthy = source.IsHealthy,
                Timestamp = _clock.FormatTimestamp(_clock.UtcDateTime)
            };

            var healthy = source.Results.Where(r => r.Check.IsHealthy)
                .Select(c => new KeyValuePair<string, string>(c.Name, c.Check.Message))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            var unhealthy = source.Results.Where(r => !r.Check.IsHealthy)
                .Select(c => new KeyValuePair<string, string>(c.Name, c.Check.Message))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            if (healthy.Any())
            {
                target.Healthy = healthy;
            }

            if (unhealthy.Any())
            {
                target.Unhealthy = unhealthy;
            }

            serializer.Serialize(writer, target);
        }
    }
}