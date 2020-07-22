using App.Metrics.Gauge;

namespace App.Metrics.Extensions.Collectors.MetricsRegistries
{
    public static class SystemUsageMetricsRegistry
    {
        public static readonly string ContextName = "System";

        public static class Gauges
        {
            public static readonly GaugeOptions TotalCpuUsed = new GaugeOptions
            {
                Context = ContextName,
                Name = "Total CPU Percentage Used",
                MeasurementUnit = Unit.Percent
            };

            public static readonly GaugeOptions PrivilegedCpuUsed = new GaugeOptions
            {
                Context = ContextName,
                Name = "Privileged CPU Percentage Used",
                MeasurementUnit = Unit.Percent
            };

            public static readonly GaugeOptions UserCpuUsed = new GaugeOptions
            {
                Context = ContextName,
                Name = "User CPU Percentage Used",
                MeasurementUnit = Unit.Percent
            };

            public static readonly GaugeOptions MemoryWorkingSet = new GaugeOptions
            {
                Context = ContextName,
                Name = "Memory Working Set",
                MeasurementUnit = Unit.Bytes
            };

            public static readonly GaugeOptions NonPagedSystemMemory = new GaugeOptions
            {
                Context = ContextName,
                Name = "Non Paged System Memory",
                MeasurementUnit = Unit.Bytes
            };

            public static readonly GaugeOptions PagedMemory = new GaugeOptions
            {
                Context = ContextName,
                Name = "Paged Memory",
                MeasurementUnit = Unit.Bytes
            };

            public static readonly GaugeOptions PagedSystemMemory = new GaugeOptions
            {
                Context = ContextName,
                Name = "Paged System Memory",
                MeasurementUnit = Unit.Bytes
            };

            public static readonly GaugeOptions PrivateMemory = new GaugeOptions
            {
                Context = ContextName,
                Name = "Private Memory",
                MeasurementUnit = Unit.Bytes
            };

            public static readonly GaugeOptions VirtualMemory = new GaugeOptions
            {
                Context = ContextName,
                Name = "Virtual Memory",
                MeasurementUnit = Unit.Bytes
            };
        }
    }
}