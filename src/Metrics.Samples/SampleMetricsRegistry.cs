using App.Metrics;
using App.Metrics.Core;

namespace Metrics.Samples
{
    public static class SampleMetricsRegistry
    {
        public static class Contexts
        {
            public static class FirstContext
            {
                public static string ContextName = "First Context";

                public static class Counters
                {
                    public static CounterOptions Counter = new CounterOptions
                    {
                        Context = ContextName,
                        Name = "Counter",
                        MeasurementUnit = Unit.Requests
                    };
                }
            }

            public static class SecondContext
            {
                public static string ContextName = "Second Context";

                public static class Counters
                {
                    public static CounterOptions Counter = new CounterOptions
                    {
                        Context = ContextName,
                        Name = "Counter",
                        MeasurementUnit = Unit.Requests
                    };
                }

                public static class Meters
                {
                    public static MeterOptions Requests = new MeterOptions
                    {
                        Context = ContextName,
                        Name = "Meter",
                        MeasurementUnit = Unit.Errors,
                        RateUnit = TimeUnit.Seconds
                    };
                }
            }
        }

        public static class Counters
        {
            private static readonly MetricTags Tags = new MetricTags().With("filter-tag2", "value2");

            public static CounterOptions CommandCounter = new CounterOptions
            {
                Name = "Command Counter",
                MeasurementUnit = Unit.Custom("Commands"),
                Tags = Tags
            };

            public static CounterOptions ConcurrentRequestsCounter = new CounterOptions
            {
                Name = "SampleMetrics.ConcurrentRequests",
                MeasurementUnit = Unit.Requests
            };

            public static CounterOptions Requests = new CounterOptions
            {
                Name = "Requests",
                MeasurementUnit = Unit.Requests
            };

            public static CounterOptions SampleCounter = new CounterOptions
            {
                Name = "Sample Counter",
                MeasurementUnit = Unit.Errors,
                Tags = Tags
            };

            public static CounterOptions SetCounter = new CounterOptions
            {
                Name = "Set Counter",
                MeasurementUnit = Unit.Items
            };
        }

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

            public static GaugeOptions Ratio = new GaugeOptions
            {
                Name = "Ratio",
                MeasurementUnit = Unit.Percent
            };
        }

        public static class Histograms
        {
            public static HistogramOptions Results = new HistogramOptions
            {
                Name = "Results",
                MeasurementUnit = Unit.Items
            };

            public static HistogramOptions ResultsExample = new HistogramOptions
            {
                Name = "ResultExample",
                MeasurementUnit = Unit.Items
            };
        }

        public static class Meters
        {
            public static MeterOptions Errors = new MeterOptions
            {
                Name = "Errors",
                MeasurementUnit = Unit.Items
            };

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
        }

        public static class Timers
        {
            public static TimerOptions Requests = new TimerOptions
            {
                Name = "Requests",
                MeasurementUnit = Unit.Requests
            };

            public static TimerOptions SampleTimer = new TimerOptions
            {
                Name = "Sample Timer",
                MeasurementUnit = Unit.Requests
            };
        }
    }
}