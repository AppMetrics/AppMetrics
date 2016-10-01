namespace App.Metrics.MetricData
{
    /// <summary>
    ///     The value reported by a Timer Metric
    /// </summary>
    public sealed class TimerValue
    {
        public readonly long ActiveSessions;
        public readonly HistogramValue Histogram;
        public readonly MeterValue Rate;
        public readonly long TotalTime;
        private readonly TimeUnit _durationUnit;

        public TimerValue(MeterValue rate, HistogramValue histogram, long activeSessions, long totalTime, TimeUnit durationUnit)
        {
            Rate = rate;
            Histogram = histogram;
            ActiveSessions = activeSessions;
            TotalTime = totalTime;
            _durationUnit = durationUnit;
        }

        public TimerValue Scale(TimeUnit rate, TimeUnit duration)
        {
            var durationFactor = _durationUnit.ScalingFactorFor(duration);
            var total = _durationUnit.Convert(duration, TotalTime);
            return new TimerValue(Rate.Scale(rate), Histogram.Scale(durationFactor), ActiveSessions, total, duration);
        }
    }

    /// <summary>
    ///     Combines the value of the timer with the defined unit and the time units for rate and duration.
    /// </summary>
    public class TimerValueSource : MetricValueSource<TimerValue>
    {
        public TimerValueSource(string name, MetricValueProvider<TimerValue> value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit,
            MetricTags tags)
            : base(name, new ScaledValueProvider<TimerValue>(value, v => v.Scale(rateUnit, durationUnit)), unit, tags)
        {
            RateUnit = rateUnit;
            DurationUnit = durationUnit;
        }

        public TimeUnit DurationUnit { get; private set; }

        public TimeUnit RateUnit { get; private set; }
    }
}