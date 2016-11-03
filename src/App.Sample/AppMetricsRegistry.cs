using App.Metrics;
using App.Metrics.Core;

namespace App.Sample
{
    public static class AppMetricsRegistry
    {
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
            private static readonly string GroupName = "Process Metrics";

            public static GaugeOptions SystemNonPagedMemoryGauge = new GaugeOptions
            {
                GroupName = GroupName,
                Name = "System Non-Paged Memory",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions ProcessPagedMemorySizeGauge = new GaugeOptions
            {
                GroupName = GroupName,
                Name = "PagedProcess Memory Size",
                MeasurementUnit = Unit.Bytes,
            };

            public static GaugeOptions SystemPagedMemorySizeGauge = new GaugeOptions
            {
                GroupName = GroupName,
                Name = "PagedSystem Memory Size",
                MeasurementUnit = Unit.Bytes,
            };

            public static GaugeOptions ProcessPeekPagedMemorySizeGauge = new GaugeOptions
            {
                GroupName = GroupName,
                Name = "Process Peek Paged Memory Size",
                MeasurementUnit = Unit.Bytes,
            };

            public static GaugeOptions ProcessPeekVirtualMemorySizeGauge = new GaugeOptions
            {
                GroupName = GroupName,
                Name = "Process Peek Paged Memory Size",
                MeasurementUnit = Unit.Bytes,
            };

            public static GaugeOptions ProcessPeekWorkingSetSizeGauge = new GaugeOptions
            {
                GroupName = GroupName,
                Name = "Process Working Set",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions ProcessPrivateMemorySizeGauge = new GaugeOptions
            {
                GroupName = GroupName,
                Name = "Process Private Memory Size",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions ProcessVirtualMemorySizeGauge = new GaugeOptions
            {
                GroupName = GroupName,
                Name = "Process Virtual Memory Size",
                MeasurementUnit = Unit.Bytes
            };
        }
    }
}