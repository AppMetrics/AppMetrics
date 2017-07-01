// <copyright file="HealthStatusConverter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace App.Metrics.Health.Formatters.Json.Converters
{
    public class HealthStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) { return typeof(HealthStatus) == objectType; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<HealthStatusData>(reader);
            var healthy = source.Healthy.Keys.Select(k => new HealthCheck.Result(k, HealthCheckResult.Healthy(source.Healthy[k])));
            var unhealthy = source.Unhealthy.Keys.Select(k => new HealthCheck.Result(k, HealthCheckResult.Unhealthy(source.Unhealthy[k])));
            var degraded = source.Degraded.Keys.Select(k => new HealthCheck.Result(k, HealthCheckResult.Degraded(source.Degraded[k])));
            var ignored = source.Ignored.Keys.Select(k => new HealthCheck.Result(k, HealthCheckResult.Ignore(source.Ignored[k])));
            var target = new HealthStatus(healthy.Concat(unhealthy).Concat(degraded).Concat(ignored));
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (HealthStatus)value;

            var target = new HealthStatusData
                         {
                             Status = HealthConstants.HealthStatusDisplay[source.Status]
                         };

            var healthy = source.Results.Where(r => r.Check.Status.IsHealthy())
                                .Select(c => new KeyValuePair<string, string>(c.Name, c.Check.Message))
                                .ToDictionary(pair => pair.Key, pair => pair.Value);

            var unhealthy = source.Results.Where(r => r.Check.Status.IsUnhealthy())
                                  .Select(c => new KeyValuePair<string, string>(c.Name, c.Check.Message))
                                  .ToDictionary(pair => pair.Key, pair => pair.Value);

            var degraded = source.Results.Where(r => r.Check.Status.IsDegraded())
                                 .Select(c => new KeyValuePair<string, string>(c.Name, c.Check.Message))
                                 .ToDictionary(pair => pair.Key, pair => pair.Value);

            var ignored = source.Results.Where(r => r.Check.Status.IsIgnored())
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

            if (degraded.Any())
            {
                target.Degraded = degraded;
            }

            if (ignored.Any())
            {
                target.Ignored = ignored;
            }

            serializer.Serialize(writer, target);
        }
    }
}