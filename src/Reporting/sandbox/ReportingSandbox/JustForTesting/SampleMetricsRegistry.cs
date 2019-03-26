// <copyright file="SampleMetricsRegistry.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace ReportingSandbox.JustForTesting
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberHidesStaticFromOuterClass
#pragma warning disable SA1401 // Fields must be private
#pragma warning disable SA1202 // Elements must be ordered by access
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
            private static readonly MetricTags Tags = new MetricTags("reporter", "influxdb");

            public static CounterOptions CommandCounter = new CounterOptions
                                                          {
                                                              Name = "Command Counter (Reset)",
                                                              MeasurementUnit = Unit.Custom("Commands"),
                                                              Tags = Tags,
                                                              ResetOnReporting = true
                                                          };

            public static CounterOptions CommandCounterDontReportSetItems = new CounterOptions
                                                                            {
                                                                                Name = "Command Counter (Not Reset, No Set Items)",
                                                                                MeasurementUnit = Unit.Custom("Commands"),
                                                                                Tags = Tags,
                                                                                ReportSetItems = false
                                                                            };

            public static CounterOptions CommandCounterNoPercentages = new CounterOptions
                                                                       {
                                                                           Name = "Command Counter (Not Reset, No Percentages)",
                                                                           MeasurementUnit = Unit.Custom("Commands"),
                                                                           Tags = Tags,
                                                                           ReportItemPercentages = false
                                                                       };

            public static CounterOptions CommandCounterNotReset = new CounterOptions
                                                                  {
                                                                      Name = "Command Counter (Not Reset)",
                                                                      MeasurementUnit = Unit.Custom("Commands"),
                                                                      Tags = Tags,
                                                                      ResetOnReporting = false
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
                                                                Name = "Results Example",
                                                                MeasurementUnit = Unit.Items,
                                                                Tags = new MetricTags("reporter", "influxdb")
                                                            };
        }

        public static class Meters
        {
            public static MeterOptions CommandMeter = new MeterOptions
                                                      {
                                                          Name = "Command Meter",
                                                          MeasurementUnit = Unit.Items,
                                                          Tags = new MetricTags("reporter", "influxdb")
                                                      };

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
                                                      MeasurementUnit = Unit.Items,
                                                  };
        }

        public static class Timers
        {
            public static TimerOptions Requests = new TimerOptions
                                                  {
                                                      Name = "Requests",
                                                      MeasurementUnit = Unit.Requests,
                                                      DurationUnit = TimeUnit.Milliseconds,
                                                      RateUnit = TimeUnit.Milliseconds,
                                                      Tags = new MetricTags("reporter", "influxdb")
                                                  };

            public static TimerOptions SampleTimer = new TimerOptions
                                                     {
                                                         Name = "Sample Timer",
                                                         MeasurementUnit = Unit.Requests
                                                     };
        }
    }
#pragma warning restore SA1202 // Elements must be ordered by access
#pragma warning restore SA1401 // Fields must be private
    // ReSharper restore MemberCanBePrivate.Global
    // ReSharper restore MemberHidesStaticFromOuterClass
}