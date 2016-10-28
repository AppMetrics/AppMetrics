// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    public sealed class JsonMetricsGroup
    {
        public string GroupName { get; set; }
        public JsonCounter[] Counters { get; set; } = new JsonCounter[0];
        public JsonGauge[] Gauges { get; set; } = new JsonGauge[0];
        public JsonHistogram[] Histograms { get; set; } = new JsonHistogram[0];
        public JsonMeter[] Meters { get; set; } = new JsonMeter[0];
        public JsonTimer[] Timers { get; set; } = new JsonTimer[0];

        public IEnumerable<JsonProperty> ToJsonProperties()
        {
            if (!string.IsNullOrEmpty(GroupName))
            {
                yield return new JsonProperty("Name", GroupName);
            }

            if (Gauges.Length > 0)
            {
                yield return new JsonProperty("Gauges", Gauges.Select(g => g.ToJsonObject()));
            }

            if (Counters.Length > 0)
            {
                yield return new JsonProperty("Counters", Counters.Select(c => c.ToJsonObject()));
            }

            if (Meters.Length > 0)
            {
                yield return new JsonProperty("Meters", Meters.Select(m => m.ToJsonObject()));
            }

            if (Histograms.Length > 0)
            {
                yield return new JsonProperty("Histograms", Histograms.Select(h => h.ToJsonObject()));
            }

            if (Timers.Length > 0)
            {
                yield return new JsonProperty("Timers", Timers.Select(t => t.ToJsonTimer()));
            }
        }

        public JsonObject ToJsonObject()
        {
            return new JsonObject(ToJsonProperties());
        }
    }

    public sealed class JsonMetricsContext
    {
        public string ContextName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Version { get; set; }
        public IDictionary<string, string> Environment { get; set; } = new Dictionary<string, string>();
        public JsonMetricsGroup[] Groups { get; set; } = new JsonMetricsGroup[0];

        public static JsonMetricsContext FromContext(MetricsData contextData)
        {
            return FromContext(contextData, null);
        }

        public static JsonMetricsContext FromContext(MetricsData contextData, string version)
        {
            var groups = contextData.Groups.Select(g => new JsonMetricsGroup
            {
                GroupName = g.GroupName,
                Counters = g.Counters.Select(JsonCounter.FromCounter).ToArray(),
                Gauges = g.Gauges.Select(JsonGauge.FromGauge).ToArray(),
                Meters = g.Meters.Select(JsonMeter.FromMeter).ToArray(),
                Histograms = g.Histograms.Select(JsonHistogram.FromHistogram).ToArray(),
                Timers = g.Timers.Select(JsonTimer.FromTimer).ToArray()

            });

            return new JsonMetricsContext
            {
                ContextName = contextData.ContextName,
                Version = version,
                Environment =  contextData.Environment.Entries.ToDictionary(entry => entry.Name, entry => entry.Value),
                Timestamp = contextData.Timestamp,
                Groups = groups.ToArray()
            };
        }

        public JsonObject ToJsonObject(IClock clock)
        {
            return new JsonObject(ToJsonProperties(clock));
        }

        public MetricsData ToMetricsData()
        {
            var groups = Groups.Select(group => new MetricsDataGroup(group.GroupName,
                group.Gauges.Select(g => g.ToValueSource()),
                group.Counters.Select(c => c.ToValueSource()),
                group.Meters.Select(m => m.ToValueSource()),
                group.Histograms.Select(h => h.ToValueSource()),
                group.Timers.Select(t => t.ToValueSource())));

            return new MetricsData(ContextName, Timestamp, new EnvironmentInfo(Environment), groups);
        }

        private IEnumerable<JsonProperty> EnvironmentJsonProperties(IClock clock)
        {
            if (!string.IsNullOrEmpty(Version))
            {
                yield return new JsonProperty("Version", Version);
            }

            if (Timestamp != default(DateTime))
            {
                yield return new JsonProperty("Timestamp", clock.FormatTimestamp(Timestamp));
            }

            if (Environment.Any())
            {
                yield return new JsonProperty("Environment", Environment.Select(e => new JsonProperty(e.Key, e.Value)));
            }
        }

        private IEnumerable<JsonProperty> ToJsonProperties(IClock clock)
        {
            if (!string.IsNullOrEmpty(ContextName))
            {
                yield return new JsonProperty("ContextName", ContextName);
            }

            if (!string.IsNullOrEmpty(Version))
            {
                yield return new JsonProperty("Version", Version);
            }

            if (Timestamp != default(DateTime))
            {
                yield return new JsonProperty("Timestamp", clock.FormatTimestamp(Timestamp));
            }


            foreach (var jsonProperty in EnvironmentJsonProperties(clock))
            {
                yield return jsonProperty;
            }

            if (Groups.Length > 0)
            {
                yield return new JsonProperty("Groups", Groups.Select(g => g.ToJsonObject()));
            }
        }
    }

   
}