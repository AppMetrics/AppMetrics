using App.Metrics;
using App.Metrics.Core.Options;

namespace Api.Sample
{
    public static class MetricsRegistry
    {
        public static class Contexts
        {
            public static class TestContext
            {
                public static readonly string TestContextName = "Test Context";               

                public static class Counters
                {
                    public static CounterOptions TestCounter { get; } = new CounterOptions
                    {
                        Name = "Test Counter",
                        MeasurementUnit = Unit.Calls,
                        Tags = MetricTags.None
                    };

                    public static CounterOptions TestCounterWithItem { get; } = new CounterOptions
                    {
                        Name = "Test Counter With Item",
                        MeasurementUnit = Unit.Calls,
                        Tags = MetricTags.None
                    };
                }

                public static class Gauges
                {
                    public static GaugeOptions TestGauge { get; } = new GaugeOptions
                    {
                        Name = "Test Gauge",
                        MeasurementUnit = Unit.Items,
                        Tags = MetricTags.None
                    };
                }

                public static class Histograms
                {
                    public static HistogramOptions TestHistogram { get; } = new HistogramOptions
                    {
                        Name = "Test Histogram",
                        SamplingType = SamplingType.HighDynamicRange,                        
                        MeasurementUnit = Unit.MegaBytes,
                        Tags = MetricTags.None
                    };

                    public static HistogramOptions TestHistogramWithUserValue { get; } = new HistogramOptions
                    {
                        Name = "Test Histogram With User Value",
                        SamplingType = SamplingType.HighDynamicRange,
                        MeasurementUnit = Unit.Bytes,
                        Tags = MetricTags.None
                    };
                }

                public static class Meters
                {
                    public static MeterOptions TestMeter { get; } = new MeterOptions
                    {
                        Name = "Test Meter",
                        MeasurementUnit = Unit.Calls,
                        Tags = MetricTags.None
                    };
                }

                public static class Timers
                {
                    public static TimerOptions TestTimer { get; } = new TimerOptions
                    {
                        Name = "Test Timer",
                        MeasurementUnit = Unit.Items,
                        DurationUnit = TimeUnit.Milliseconds,
                        RateUnit = TimeUnit.Milliseconds,
                        Tags = MetricTags.None,
                        SamplingType = SamplingType.ExponentiallyDecaying,
                    };

                    public static TimerOptions TestTimerWithUserValue { get; } = new TimerOptions
                    {
                        MeasurementUnit = Unit.Items,
                        DurationUnit = TimeUnit.Milliseconds,
                        RateUnit = TimeUnit.Milliseconds,
                        Tags = MetricTags.None,
                        SamplingType = SamplingType.ExponentiallyDecaying,
                    };
                }
            }

            public static class TestContextTwo
            {
                public static readonly string TestContextName = "Test Context Two";

                public static class Counters
                {
                    public static CounterOptions TestCounter { get; } = new CounterOptions
                    {
                        Name = "Test Counter",
                        MeasurementUnit = Unit.Calls,
                        Tags = MetricTags.None
                    };

                    public static CounterOptions TestCounterWithItem { get; } = new CounterOptions
                    {
                        Name = "Test Counter With Item",
                        MeasurementUnit = Unit.Calls,
                        Tags = MetricTags.None
                    };
                }

                public static class Gauges
                {
                    public static GaugeOptions TestGauge { get; } = new GaugeOptions
                    {
                        Name = "Test Gauge",
                        MeasurementUnit = Unit.Items,
                        Tags = MetricTags.None
                    };
                }

                public static class Histograms
                {
                    public static HistogramOptions TestHistogram { get; } = new HistogramOptions
                    {
                        Name = "Test Histogram",
                        SamplingType = SamplingType.HighDynamicRange,
                        MeasurementUnit = Unit.MegaBytes,
                        Tags = MetricTags.None
                    };

                    public static HistogramOptions TestHistogramWithUserValue { get; } = new HistogramOptions
                    {
                        Name = "Test Histogram With User Value",
                        SamplingType = SamplingType.HighDynamicRange,
                        MeasurementUnit = Unit.Bytes,
                        Tags = MetricTags.None
                    };
                }

                public static class Meters
                {
                    public static MeterOptions TestMeter { get; } = new MeterOptions
                    {
                        Name = "Test Meter",
                        MeasurementUnit = Unit.Calls,
                        Tags = MetricTags.None
                    };
                }

                public static class Timers
                {
                    public static TimerOptions TestTimer { get; } = new TimerOptions
                    {
                        Name = "Test Timer",
                        MeasurementUnit = Unit.Items,
                        DurationUnit = TimeUnit.Milliseconds,
                        RateUnit = TimeUnit.Milliseconds,
                        Tags = MetricTags.None,
                        SamplingType = SamplingType.ExponentiallyDecaying,
                    };

                    public static TimerOptions TestTimerWithUserValue { get; } = new TimerOptions
                    {
                        Name = "Test Timer With User Value",
                        MeasurementUnit = Unit.Items,
                        DurationUnit = TimeUnit.Milliseconds,
                        RateUnit = TimeUnit.Milliseconds,
                        Tags = MetricTags.None,
                        SamplingType = SamplingType.ExponentiallyDecaying,
                    };
                }
            }
        }

        public static class Counters
        {
            public static CounterOptions TestCounter { get; } = new CounterOptions
            {
                Name = "Test Counter",
                MeasurementUnit = Unit.Calls,
                Tags = MetricTags.None
            };

            public static CounterOptions TestCounterWithItem { get; } = new CounterOptions
            {
                Name = "Test Counter With Item",
                MeasurementUnit = Unit.Calls,
                Tags = MetricTags.None
            };
        }

        public static class ApdexScores
        {
            public static ApdexOptions TestApdex { get; } = new ApdexOptions
            {
                Name = "Test Apdex",
                Tags = MetricTags.None
            };
        }

        public static class Gauges
        {
            public static GaugeOptions TestGauge { get; } = new GaugeOptions
            {
                Name = "Test Gauge",
                MeasurementUnit = Unit.Bytes,
                Tags = MetricTags.None
            };

            public static GaugeOptions DerivedGauge { get; } = new GaugeOptions
            {
                Name = "Derived Gauge",
                MeasurementUnit = Unit.MegaBytes,
                Tags = MetricTags.None
            };

            public static GaugeOptions CacheHitRatioGauge { get; } = new GaugeOptions
            {
                Name = "Cache Gauge",
                MeasurementUnit = Unit.Calls,
                Tags = MetricTags.None
            };
        }

        public static class Histograms
        {
            public static HistogramOptions TestHAdvancedistogram { get; } = new HistogramOptions
            {
                Name = "Test Advanced Histogram",
                SamplingType = SamplingType.ExponentiallyDecaying,
                MeasurementUnit = Unit.MegaBytes,
                Tags = MetricTags.None
            };

            public static HistogramOptions TestHistogram { get; } = new HistogramOptions
            {
                Name = "Test Histogram",
                SamplingType = SamplingType.ExponentiallyDecaying,
                MeasurementUnit = Unit.MegaBytes,
                Tags = MetricTags.None
            };

            public static HistogramOptions TestHistogramWithUserValue { get; } = new HistogramOptions
            {
                Name = "Test Histogram With User Value",
                SamplingType = SamplingType.ExponentiallyDecaying,
                MeasurementUnit = Unit.Bytes,
                Tags = MetricTags.None
            };
        }

        public static class Meters
        {
            public static MeterOptions CacheHits { get; } = new MeterOptions
            {
                Name = "Cache Hits Meter",
                MeasurementUnit = Unit.Calls,
                Tags = MetricTags.None
            };
        }

        public static class Timers
        {
            public static TimerOptions TestTimer { get; } = new TimerOptions
            {
                Name = "Test Timer",
                MeasurementUnit = Unit.Items,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds,
                Tags = MetricTags.None,
                SamplingType = SamplingType.ExponentiallyDecaying,
            };

            public static TimerOptions DatabaseQueryTimer { get; } = new TimerOptions
            {
                Name = "Database Query Timer",
                MeasurementUnit = Unit.Calls,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds,
                Tags = MetricTags.None,
                SamplingType = SamplingType.SlidingWindow
            };

            public static TimerOptions TestTimerTwo { get; } = new TimerOptions
            {
                Name = "Test Timer 2",
                MeasurementUnit = Unit.Items,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds,
                Tags = MetricTags.None,
                SamplingType = SamplingType.ExponentiallyDecaying,
            };

            public static TimerOptions TestTimerTwoWithUserValue { get; } = new TimerOptions
            {
                Name = "Test Timer 2 With User Value",
                MeasurementUnit = Unit.Items,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds,
                Tags = MetricTags.None,
                SamplingType = SamplingType.ExponentiallyDecaying,
            };

            public static TimerOptions TestTimerWithUserValue { get; } = new TimerOptions
            {
                Name = "Test Timer With User Value",
                MeasurementUnit = Unit.Items,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds,
                Tags = MetricTags.None,
                SamplingType = SamplingType.ExponentiallyDecaying,
            };
        }
    }
}