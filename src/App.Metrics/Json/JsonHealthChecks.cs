using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    public class JsonHealthChecks
    {
        public const string HealthChecksMimeType = "application/vnd.metrics.net.v1.health+json";
        public const int Version = 1;

        private readonly List<JsonProperty> root = new List<JsonProperty>();

        public static string BuildJson(HealthStatus status)
        {
            return BuildJson(status, Clock.Default, indented: false);
        }

        public static string BuildJson(HealthStatus status, Clock clock, bool indented = true)
        {
            return new JsonHealthChecks()
                .AddVersion(Version)
                .AddTimestamp(Clock.Default)
                .AddObject(status)
                .GetJson(indented);
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

        public JsonHealthChecks AddTimestamp(Clock clock)
        {
            root.Add(new JsonProperty("Timestamp", Clock.FormatTimestamp(clock.UTCDateTime)));
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