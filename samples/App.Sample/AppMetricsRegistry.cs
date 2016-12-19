using App.Metrics;
using App.Metrics.Core;
using App.Metrics.Core.Options;

namespace App.Sample
{
    public static class AppMetricsRegistry
    {
        public static class ApdexScores
        {
            public static ApdexOptions AppApdex = new ApdexOptions
            {
                Name = "App Apdex",
                ApdexTSeconds = 0.5,
                Tags = new MetricTags().With("reporter", "influxdb")
            };
        }

        public static class Gauges
        {
            public static GaugeOptions ApmGauge = new GaugeOptions
            {
                Name = "& ApmGauge",
                MeasurementUnit = Unit.None
            };

            public static GaugeOptions Errors = new GaugeOptions
            {
                Name = "Errors",
                MeasurementUnit = Unit.None
            };

            public static GaugeOptions GaugeWithNoValue = new GaugeOptions
            {
                Name = "Gauge With No Value",
                MeasurementUnit = Unit.None
            };

            public static GaugeOptions ParenthesisGauge = new GaugeOptions
            {
                Name = "()[]{} ParantesisGauge",
                MeasurementUnit = Unit.None
            };

            public static GaugeOptions PercentGauge = new GaugeOptions
            {
                Name = "% Percent/Gauge|test",
                MeasurementUnit = Unit.None
            };
        }

        //https://msdn.microsoft.com/en-us/library/system.diagnostics.process(v=vs.110).aspx
        public static class ProcessMetrics
        {
            private static readonly string Context = "Process";

            public static GaugeOptions SystemNonPagedMemoryGauge = new GaugeOptions
            {
                Context = Context,
                Name = "System Non-Paged Memory",
                MeasurementUnit = Unit.Bytes,
                Tags = new MetricTags().With("reporter", "influxdb")
            };

            public static GaugeOptions ProcessPagedMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "PagedProcess Memory Size",
                MeasurementUnit = Unit.Bytes,
                Tags = new MetricTags().With("reporter", "influxdb")
            };

            public static GaugeOptions CpuUsageTotal = new GaugeOptions
            {
                Context = Context,
                Name = "Process Total CPU Usage",
                MeasurementUnit = Unit.Percent,
                Tags = new MetricTags().With("reporter", "influxdb")
            };

            public static GaugeOptions SystemPagedMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "PagedSystem Memory Size",
                MeasurementUnit = Unit.Bytes,
                Tags = new MetricTags().With("reporter", "influxdb")
            };

            public static GaugeOptions ProcessPeekPagedMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "Process Peek Paged Memory Size",
                MeasurementUnit = Unit.Bytes,
                Tags = new MetricTags().With("reporter", "influxdb")
            };

            public static GaugeOptions ProcessPeekVirtualMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "Process Peek Paged Memory Size",
                MeasurementUnit = Unit.Bytes,
                Tags = new MetricTags().With("reporter", "influxdb")
            };

            public static GaugeOptions ProcessPeekWorkingSetSizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "Process Working Set",
                MeasurementUnit = Unit.Bytes,
                Tags = new MetricTags().With("reporter", "influxdb")
            };

            public static GaugeOptions ProcessPrivateMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "Process Private Memory Size",
                MeasurementUnit = Unit.Bytes,
                Tags = new MetricTags().With("reporter", "influxdb")
            };

            public static GaugeOptions ProcessVirtualMemorySizeGauge = new GaugeOptions
            {
                Context = Context,
                Name = "Process Virtual Memory Size",
                MeasurementUnit = Unit.Bytes,
                Tags = new MetricTags().With("reporter", "influxdb")
            };
        }
    }
}