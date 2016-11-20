public static class AppMetricsRegistery
{
    public static class ProcessMetrics
    {
        private static readonly MetricTags Tags = new MetricTags().With("tag-key", "tag-value");
        private static readonly string Context = "Process";

        public static GaugeOptions SystemNonPagedMemoryGauge = new GaugeOptions
        {
            Context = Context,
            Name = "System Non-Paged Memory",
            MeasurementUnit = Unit.Bytes,
            Tag = Tags
        };            

        public static GaugeOptions ProcessVirtualMemorySizeGauge = new GaugeOptions
        {
            Context = Context,
            Name = "Process Virtual Memory Size",
            MeasurementUnit = Unit.Bytes,
            Tag = Tags
        };
    }

    public static class DatabaseMetrics
    {
        private static readonly string Context = "Database";

        public static TimerOptions SearchUsersSqlTimer = new TimerOptions
        {
            Context = ContextName,
            Name = "Search Users",
            MeasurementUnit = Unit.Calls
        };
    }
}