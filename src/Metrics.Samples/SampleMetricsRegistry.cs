using App.Metrics;
using App.Metrics.Core;

namespace Metrics.Samples
{
    public static class SampleMetricsRegistry
    {
        public static class Gauges
        {
            public static GaugeOptions CustomRatioGauge = new GaugeOptions
            {
                Name = "Custom Ratio",
                MeasurementUnit = Unit.Percent
            };

            public static GaugeOptions DataValue = new GaugeOptions
            {
                Name = "SampleMetrics.DataValue",
                MeasurementUnit = Unit.Custom("$")
            };
        }

        public static class Meters
        {
            public static MeterOptions Requests = new MeterOptions
            {
                Name = "Requests",
                MeasurementUnit = Unit.Requests
            };

            public static MeterOptions SetMeter = new MeterOptions
            {
                Name = "Set Meter",
                MeasurementUnit = Unit.Items
            };

            public static MeterOptions Errors = new MeterOptions
            {
                Name = "Errors",
                MeasurementUnit = Unit.Items
            };

        }

        public static class Counters
        {
            public static CounterOptions ConcurrentRequestsCounter = new CounterOptions
            {
                Name = "SampleMetrics.ConcurrentRequests",
                MeasurementUnit = Unit.Requests
            };

            public static CounterOptions SetCounter = new CounterOptions
            {
                Name = "Set Counter",
                MeasurementUnit = Unit.Items
            };

            public static CounterOptions Requests = new CounterOptions
            {
                Name = "Requests",
                MeasurementUnit = Unit.Requests
            };

            public static CounterOptions SampleCounter = new CounterOptions
            {
                Name = "Sample Counter",
                MeasurementUnit = Unit.Errors
            };

            public static CounterOptions CommandCounter = new CounterOptions
            {
                Name = "Command Counter",
                MeasurementUnit = Unit.Custom("Commands")
            };
        }

        public static class Groups
        {
            public static class FirstGroup
            {
                public static string GroupName = "First Group";

                public static class Counters
                {
                    public static CounterOptions Counter = new CounterOptions
                    {
                        Name = "Counter",
                        MeasurementUnit = Unit.Requests
                    };
                }
            }

            public static class SecondGroup
            {
                public static string GroupName = "Second Group";

                public static class Counters
                {
                    public static CounterOptions Counter = new CounterOptions
                    {
                        Name = "Counter",
                        MeasurementUnit = Unit.Requests
                    };
                }

                public static class Meters
                {
                    public static MeterOptions Requests = new MeterOptions
                    {
                        Name = "Meter",
                        MeasurementUnit = Unit.Errors,
                        RateUnit = TimeUnit.Seconds
                    };
                  
                }
            }
        }
    }
}