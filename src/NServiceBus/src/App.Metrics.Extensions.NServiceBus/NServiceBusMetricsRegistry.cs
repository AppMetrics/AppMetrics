using App.Metrics.Counter;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Extensions.NServiceBus
{
    internal static class NServiceBusMetricsRegistry
    {
        public static string ContenxtName = "NServiceBus";

        public static class Meters
        {
            /// <summary>
            ///     NSB Metric: # of msgs failures / sec
            /// </summary>
            public static readonly MeterOptions FailureRate = new MeterOptions
            {
                Context = ContenxtName,
                Name = "Message Failures",
                MeasurementUnit = Unit.Errors,
                RateUnit = TimeUnit.Seconds
            };

            /// <summary>
            ///     NSB Metric: # of msgs successfully processed / sec
            /// </summary>
            public static readonly MeterOptions SuccessRate = new MeterOptions
            {
                Context = ContenxtName,
                Name = "Messages Successfully Processed",
                MeasurementUnit = Unit.Events,
                RateUnit = TimeUnit.Seconds
            };

            /// <summary>
            ///     NSB Metric: # of msgs pulled from the input queue /sec
            /// </summary>
            public static readonly MeterOptions FetchRate = new MeterOptions
            {
                Context = ContenxtName,
                Name = "Messages Pulled From Input Queue",
                MeasurementUnit = Unit.Events,
                RateUnit = TimeUnit.Seconds
            };

            /// <summary>
            ///     NSB Metric: Retries
            /// </summary>
            public static readonly MeterOptions Retries = new MeterOptions
            {
                Context = ContenxtName,
                Name = "Message Retry Total",
                MeasurementUnit = Unit.Events,
                RateUnit = TimeUnit.Seconds
            };
        }

        public static class Timers
        {
            /// <summary>
            ///     NSB Metric: Critical Time
            /// </summary>
            public static readonly TimerOptions CriticalTime = new TimerOptions
            {
                Context = ContenxtName,
                Name = "Critical Time",
                DurationUnit = TimeUnit.Milliseconds
            };

            /// <summary>
            ///     NSB Metric: Processing Time
            /// </summary>
            public static readonly TimerOptions ProcessingTime = new TimerOptions
            {
                Context = ContenxtName,
                Name = "Processing Time",
                DurationUnit = TimeUnit.Milliseconds
            };
        }
    }
}