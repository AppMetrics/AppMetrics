// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using App.Metrics.Health;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    public sealed class JsonHealthChecks
    {
        public const string HealthChecksMimeType = "application/vnd.app.metrics.v1.health+json";
        public const int Version = 1;

        private readonly List<JsonProperty> root = new List<JsonProperty>();

        public static string BuildJson(HealthStatus status, IClock clock, bool indented)
        {
            return new JsonHealthChecks()
                .AddVersion(Version)
                .AddTimestamp(clock)
                .AddObject(status)
                .GetJson();
        }

        public JsonHealthChecks AddObject(HealthStatus status)
        {
            var properties = new List<JsonProperty>() { new JsonProperty("IsHealthy", status.IsHealthy) };
            var unhealty = status.Results.Where(r => !r.Check.IsHealthy)
                .Select(r => new JsonProperty(r.Name, r.Check.Message));
            properties.Add(new JsonProperty("Unhealthy", unhealty));
            var healty = status.Results.Where(r => r.Check.IsHealthy)
                .Select(r => new JsonProperty(r.Name, r.Check.Message));
            properties.Add(new JsonProperty("Healthy", healty));
            root.AddRange(properties);
            return this;
        }

        public JsonHealthChecks AddTimestamp(IClock clock)
        {
            root.Add(new JsonProperty("Timestamp", clock.FormatTimestamp(clock.UtcDateTime)));
            return this;
        }

        public JsonHealthChecks AddVersion(int version)
        {
            root.Add(new JsonProperty("Version", version.ToString(CultureInfo.InvariantCulture)));
            return this;
        }

        public string GetJson(bool indented = true)
        {
            return new JsonObject(root).AsJson(indented);
        }
    }
}