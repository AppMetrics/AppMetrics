using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    public class JsonMetricsContext
    {
        private JsonMetricsContext[] childContexts = new JsonMetricsContext[0];
        private JsonCounter[] counters = new JsonCounter[0];
        private Dictionary<string, string> environment = new Dictionary<string, string>();
        private JsonGauge[] gauges = new JsonGauge[0];
        private JsonHistogram[] histograms = new JsonHistogram[0];
        private JsonMeter[] meters = new JsonMeter[0];
        private JsonTimer[] timers = new JsonTimer[0];

        public JsonMetricsContext[] ChildContexts
        {
            get { return this.childContexts; }
            set { this.childContexts = value ?? new JsonMetricsContext[0]; }
        }

        public string Context { get; set; }

        public JsonCounter[] Counters
        {
            get { return this.counters; }
            set { this.counters = value ?? new JsonCounter[0]; }
        }

        public Dictionary<string, string> Environment
        {
            get { return this.environment; }
            set { this.environment = value ?? new Dictionary<string, string>(); }
        }

        public JsonGauge[] Gauges
        {
            get { return this.gauges; }
            set { this.gauges = value ?? new JsonGauge[0]; }
        }

        public JsonHistogram[] Histograms
        {
            get { return this.histograms; }
            set { this.histograms = value ?? new JsonHistogram[0]; }
        }

        public JsonMeter[] Meters
        {
            get { return this.meters; }
            set { this.meters = value ?? new JsonMeter[0]; }
        }

        public JsonTimer[] Timers
        {
            get { return this.timers; }
            set { this.timers = value ?? new JsonTimer[0]; }
        }

        public DateTime Timestamp { get; set; }

        public string Version { get; set; }

        public static JsonMetricsContext FromContext(MetricsData contextData)
        {
            return FromContext(contextData, null);
        }

        public static JsonMetricsContext FromContext(MetricsData contextData, string version)
        {
            return FromContext(contextData, Enumerable.Empty<EnvironmentEntry>(), version);
        }

        public static JsonMetricsContext FromContext(MetricsData contextData, IEnumerable<EnvironmentEntry> environment, string version)
        {
            return new JsonMetricsContext
            {
                Version = version,
                Timestamp = contextData.Timestamp,
                Environment = contextData.Environment.Union(environment).ToDictionary(e => e.Name, e => e.Value),
                Context = contextData.Context,
                Gauges = contextData.Gauges.Select(JsonGauge.FromGauge).ToArray(),
                Counters = contextData.Counters.Select(JsonCounter.FromCounter).ToArray(),
                Meters = contextData.Meters.Select(JsonMeter.FromMeter).ToArray(),
                Histograms = contextData.Histograms.Select(JsonHistogram.FromHistogram).ToArray(),
                Timers = contextData.Timers.Select(JsonTimer.FromTimer).ToArray(),
                ChildContexts = contextData.ChildMetrics.Select(FromContext).ToArray()
            };
        }

        public JsonObject ToJsonObject()
        {
            return new JsonObject(ToJsonProperties());
        }

        public MetricsData ToMetricsData()
        {
            return new MetricsData(this.Context, this.Timestamp,
                this.Environment.Select(e => new EnvironmentEntry(e.Key, e.Value)),
                this.Gauges.Select(g => g.ToValueSource()),
                this.Counters.Select(c => c.ToValueSource()),
                this.Meters.Select(m => m.ToValueSource()),
                this.Histograms.Select(h => h.ToValueSource()),
                this.Timers.Select(t => t.ToValueSource()),
                this.ChildContexts.Select(c => c.ToMetricsData()));
        }

        private IEnumerable<JsonProperty> ToJsonProperties()
        {
            if (!string.IsNullOrEmpty(this.Version))
            {
                yield return new JsonProperty("Version", this.Version);
            }

            if (this.Timestamp != default(DateTime))
            {
                yield return new JsonProperty("Timestamp", Clock.FormatTimestamp(this.Timestamp));
            }

            if (this.Environment.Count > 0)
            {
                yield return new JsonProperty("Environment", this.Environment.Select(e => new JsonProperty(e.Key, e.Value)));
            }

            if (!string.IsNullOrEmpty(this.Context))
            {
                yield return new JsonProperty("Context", this.Context);
            }

            if (this.Gauges.Length > 0)
            {
                yield return new JsonProperty("Gauges", this.Gauges.Select(g => g.ToJsonObject()));
            }

            if (this.Counters.Length > 0)
            {
                yield return new JsonProperty("Counters", this.Counters.Select(c => c.ToJsonObject()));
            }

            if (this.Meters.Length > 0)
            {
                yield return new JsonProperty("Meters", this.Meters.Select(m => m.ToJsonObject()));
            }

            if (this.Histograms.Length > 0)
            {
                yield return new JsonProperty("Histograms", this.Histograms.Select(h => h.ToJsonObject()));
            }

            if (this.Timers.Length > 0)
            {
                yield return new JsonProperty("Timers", this.Timers.Select(t => t.ToJsonTimer()));
            }

            if (this.ChildContexts.Length > 0)
            {
                yield return new JsonProperty("ChildContexts", this.ChildContexts.Select(c => c.ToJsonObject()));
            }
        }
    }
}