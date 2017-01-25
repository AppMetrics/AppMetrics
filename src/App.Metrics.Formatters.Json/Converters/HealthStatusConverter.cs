// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Health;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Converters
{
    public class HealthStatusConverter : JsonConverter
    {
        private readonly IClock _clock;

        public HealthStatusConverter(IClock clock) { _clock = clock; }

        public override bool CanConvert(Type objectType) { return typeof(HealthStatus) == objectType; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<HealthStatusData>(reader);
            var healthy = source.Healthy.Keys.Select(k => new HealthCheck.Result(k, HealthCheckResult.Healthy(source.Healthy[k])));
            var unhealthy = source.Unhealthy.Keys.Select(k => new HealthCheck.Result(k, HealthCheckResult.Unhealthy(source.Unhealthy[k])));
            var degraded = source.Degraded.Keys.Select(k => new HealthCheck.Result(k, HealthCheckResult.Degraded(source.Degraded[k])));
            var target = new HealthStatus(healthy.Concat(unhealthy).Concat(degraded));
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (HealthStatus)value;

            var target = new HealthStatusData
                         {
                             Status = source.Status.Hummanize(),
                             Timestamp = _clock.FormatTimestamp(_clock.UtcDateTime)
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

            serializer.Serialize(writer, target);
        }
    }
}