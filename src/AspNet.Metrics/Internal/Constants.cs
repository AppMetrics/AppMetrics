namespace AspNet.Metrics.Internal
{
    internal static class Constants
    {
        public static class DefaultRoutePaths
        {
            public const string HealthEndpoint = "/health";
            public const string MetricsEndpoint = "/metrics";
            public const string MetricsTextEndpoint = "/metrics-text";
            public const string PingEndpoint = "/ping";
        }
    }
}