using App.Metrics.Core;

namespace App.Metrics.MetricData
{
    public static class ValueReader
    {
        private static readonly CounterValue EmptyCounter = new CounterValue(0);

        private static readonly HistogramValue EmptyHistogram = new HistogramValue(0, 0.0, null, 0.0, null, 0.0, 0.0, null, 0.0, 0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0);

        private static readonly MeterValue EmptyMeter = new MeterValue(0, 0.0, 0.0, 0.0, 0.0, TimeUnit.Seconds);
        private static readonly TimerValue EmptyTimer = new TimerValue(EmptyMeter, EmptyHistogram, 0, 0, TimeUnit.Milliseconds);

        public static CounterValue GetCurrentValue(Counter metric)
        {
            var implementation = metric as CounterImplementation;
            if (implementation != null)
            {
                return implementation.Value;
            }
            return EmptyCounter;
        }

        public static MeterValue GetCurrentValue(Meter metric)
        {
            var implementation = metric as MeterImplementation;
            if (implementation != null)
            {
                return implementation.Value;
            }
            return EmptyMeter;
        }

        public static HistogramValue GetCurrentValue(Histogram metric)
        {
            var implementation = metric as HistogramImplementation;
            if (implementation != null)
            {
                return implementation.Value;
            }
            return EmptyHistogram;
        }

        public static TimerValue GetCurrentValue(Timer metric)
        {
            var implementation = metric as TimerImplementation;
            if (implementation != null)
            {
                return implementation.Value;
            }
            return EmptyTimer;
        }
    }
}