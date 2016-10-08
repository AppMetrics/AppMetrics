using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    public class JsonMetricsContext
    {
        private JsonMetricsContext[] _childContexts = new JsonMetricsContext[0];
        private JsonCounter[] _counters = new JsonCounter[0];
        private IDictionary<string, string>_environment = new Dictionary<string, string>();
        private JsonGauge[] _gauges = new JsonGauge[0];
        private JsonHistogram[] _histograms = new JsonHistogram[0];
        private JsonMeter[] _meters = new JsonMeter[0];
        private JsonTimer[] _timers = new JsonTimer[0];

        public JsonMetricsContext[] ChildContexts
        {
            get { return _childContexts; }
            set { _childContexts = value ?? new JsonMetricsContext[0]; }
        }

        public string Context { get; set; }

        public JsonCounter[] Counters
        {
            get { return _counters; }
            set { _counters = value ?? new JsonCounter[0]; }
        }

        public IDictionary<string, string> Environment
        {
            get { return _environment; }
            set { _environment = value ?? new Dictionary<string, string>(); ; }
        }

        public JsonGauge[] Gauges
        {
            get { return _gauges; }
            set { _gauges = value ?? new JsonGauge[0]; }
        }

        public JsonHistogram[] Histograms
        {
            get { return _histograms; }
            set { _histograms = value ?? new JsonHistogram[0]; }
        }

        public JsonMeter[] Meters
        {
            get { return _meters; }
            set { _meters = value ?? new JsonMeter[0]; }
        }

        public JsonTimer[] Timers
        {
            get { return _timers; }
            set { _timers = value ?? new JsonTimer[0]; }
        }

        public DateTime Timestamp { get; set; }

        public string Version { get; set; }

        public static JsonMetricsContext FromContext(MetricsData contextData)
        {
            return FromContext(contextData, null);
        }

        public static JsonMetricsContext FromContext(MetricsData contextData, string version)
        {
            return FromContext(contextData, EnvironmentInfo.Empty, version);
        }

        public static JsonMetricsContext FromContext(MetricsData contextData, EnvironmentInfo environment, string version)
        {
            return new JsonMetricsContext
            {
                Version = version,
                Timestamp = contextData.Timestamp,
                Environment = environment.Entries.Any() ? contextData.Environment.Union(environment.Entries).ToDictionary(d => d.Name, d => d.Value) : contextData.Environment.ToDictionary(d => d.Name, d => d.Value),
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
            return new MetricsData(Context, Timestamp,
                Environment.Select(e => new EnvironmentInfoEntry(e.Key, e.Value)),
                Gauges.Select(g => g.ToValueSource()),
                Counters.Select(c => c.ToValueSource()),
                Meters.Select(m => m.ToValueSource()),
                Histograms.Select(h => h.ToValueSource()),
                Timers.Select(t => t.ToValueSource()),
                ChildContexts.Select(c => c.ToMetricsData()));
        }

        private IEnumerable<JsonProperty> ToJsonProperties()
        {
            if (!string.IsNullOrEmpty(Version))
            {
                yield return new JsonProperty("Version", Version);
            }

            if (Timestamp != default(DateTime))
            {
                yield return new JsonProperty("Timestamp", Clock.FormatTimestamp(Timestamp));
            }

            if (Environment.Any())
            {
                yield return new JsonProperty("Environment", Environment.Select(e => new JsonProperty(e.Key, e.Value)));
            }

            if (!string.IsNullOrEmpty(Context))
            {
                yield return new JsonProperty("Context", Context);
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

            if (ChildContexts.Length > 0)
            {
                yield return new JsonProperty("ChildContexts", ChildContexts.Select(c => c.ToJsonObject()));
            }
        }
    }
}