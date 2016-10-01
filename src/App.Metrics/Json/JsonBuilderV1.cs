using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    public class JsonBuilderV1
    {
        public const int Version = 1;
        public const string MetricsMimeType = "application/vnd.metrics.net.v1.metrics+json";

#if !DEBUG
        private const bool DefaultIndented = false;
#else
        private const bool DefaultIndented = true;
#endif

        private readonly List<JsonProperty> root = new List<JsonProperty>();
        private readonly List<JsonProperty> units = new List<JsonProperty>();

        public static string BuildJson(MetricsData data)
        {
            return BuildJson(data, Clock.Default, indented: DefaultIndented);
        }

        public static string BuildJson(MetricsData data, Clock clock, bool indented = DefaultIndented)
        {
            var flatData = data.OldFormat();

            return new JsonBuilderV1()
                .AddVersion(Version)
                .AddTimestamp(Clock.Default)
                .AddEnvironment()
                .AddObject(flatData.Gauges)
                .AddObject(flatData.Counters)
                .AddObject(flatData.Meters)
                .AddObject(flatData.Histograms)
                .AddObject(flatData.Timers)
                .GetJson(indented);
        }

        private JsonBuilderV1 AddEnvironment()
        {
            var environment = AppEnvironment.Current
                .Select(e => new JsonProperty(e.Name, e.Value));

            root.Add(new JsonProperty("Environment", new ObjectJsonValue(new JsonObject(environment))));

            return this;
        }

        private string GetJson(bool indented = DefaultIndented)
        {
            if (units.Any() && !root.Any(p => p.Name == "Units"))
            {
                root.Add(new JsonProperty("Units", units));
            }

            return new JsonObject(root).AsJson(indented);
        }

        public JsonBuilderV1 AddVersion(int version)
        {
            root.Add(new JsonProperty("Version", version.ToString(CultureInfo.InvariantCulture)));
            return this;
        }

        public JsonBuilderV1 AddTimestamp(Clock clock)
        {
            root.Add(new JsonProperty("Timestamp", Clock.FormatTimestamp(clock.UTCDateTime)));
            return this;
        }

        public JsonBuilderV1 AddObject(IEnumerable<GaugeValueSource> gauges)
        {
            root.Add(new JsonProperty("Gauges", gauges.Select(g => new JsonProperty(g.Name, g.Value))));
            units.Add(new JsonProperty("Gauges", gauges.Select(g => new JsonProperty(g.Name, g.Unit.Name))));
            return this;
        }

        public JsonBuilderV1 AddObject(IEnumerable<CounterValueSource> counters)
        {
            root.Add(new JsonProperty("Counters", counters.Select(c => new JsonProperty(c.Name, c.Value.Count))));
            units.Add(new JsonProperty("Counters", counters.Select(c => new JsonProperty(c.Name, c.Unit.Name))));
            return this;
        }

        public JsonBuilderV1 AddObject(IEnumerable<MeterValueSource> meters)
        {
            root.Add(new JsonProperty("Meters", meters.Select(m => new JsonProperty(m.Name, Meter(m.Value)))));
            units.Add(new JsonProperty("Meters", meters.Select(m => new JsonProperty(m.Name, $"{m.Unit.Name}/{m.RateUnit.Unit()}"))));
            return this;
        }

        public JsonBuilderV1 AddObject(IEnumerable<HistogramValueSource> histograms)
        {
            root.Add(new JsonProperty("Histograms", histograms.Select(m => new JsonProperty(m.Name, Histogram(m.Value)))));
            units.Add(new JsonProperty("Histograms", histograms.Select(m => new JsonProperty(m.Name, m.Unit.Name))));
            return this;
        }

        public JsonBuilderV1 AddObject(IEnumerable<TimerValueSource> timers)
        {
            var properties = timers.Select(t => new { t.Name, t.Value, t.RateUnit, t.DurationUnit })
                .Select(t => new JsonProperty(t.Name, new[]
                {
                    new JsonProperty("ActiveSessions", t.Value.ActiveSessions),
                    new JsonProperty("TotalTime", t.Value.TotalTime),
                    new JsonProperty("Rate", Meter(t.Value.Rate)),
                    new JsonProperty("Histogram", Histogram(t.Value.Histogram))
                }));

            root.Add(new JsonProperty("Timers", properties));

            var units = timers.Select(t => new JsonProperty(t.Name, new[]
            {
                new JsonProperty("Rate", $"{t.Unit.Name}/{t.RateUnit.Unit()}"),
                new JsonProperty("Duration", t.DurationUnit.Unit())
            }));

            this.units.Add(new JsonProperty("Timers", units));

            return this;
        }

        private static IEnumerable<JsonProperty> Meter(MeterValue value)
        {
            yield return new JsonProperty("Count", value.Count);
            yield return new JsonProperty("MeanRate", value.MeanRate);
            yield return new JsonProperty("OneMinuteRate", value.OneMinuteRate);
            yield return new JsonProperty("FiveMinuteRate", value.FiveMinuteRate);
            yield return new JsonProperty("FifteenMinuteRate", value.FifteenMinuteRate);
        }

        private static IEnumerable<JsonProperty> Histogram(HistogramValue value)
        {
            yield return new JsonProperty("Count", value.Count);
            yield return new JsonProperty("LastValue", value.LastValue);
            yield return new JsonProperty("LastUserValue", value.LastUserValue);
            yield return new JsonProperty("Min", value.Min);
            yield return new JsonProperty("MinUserValue", value.MinUserValue);
            yield return new JsonProperty("Mean", value.Mean);
            yield return new JsonProperty("Max", value.Max);
            yield return new JsonProperty("MaxUserValue", value.MaxUserValue);
            yield return new JsonProperty("StdDev", value.StdDev);
            yield return new JsonProperty("Median", value.Median);
            yield return new JsonProperty("Percentile75", value.Percentile75);
            yield return new JsonProperty("Percentile95", value.Percentile95);
            yield return new JsonProperty("Percentile98", value.Percentile98);
            yield return new JsonProperty("Percentile99", value.Percentile99);
            yield return new JsonProperty("Percentile999", value.Percentile999);
            yield return new JsonProperty("SampleSize", value.SampleSize);
        }
    }
}