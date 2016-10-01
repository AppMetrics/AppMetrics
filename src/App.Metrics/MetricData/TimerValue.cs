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
        private readonly TimeUnit durationUnit;

        public TimerValue(MeterValue rate, HistogramValue histogram, long activeSessions, long totalTime, TimeUnit durationUnit)
        {
            this.Rate = rate;
            this.Histogram = histogram;
            this.ActiveSessions = activeSessions;
            this.TotalTime = totalTime;
            this.durationUnit = durationUnit;
        }

        public TimerValue Scale(TimeUnit rate, TimeUnit duration)
        {
            var durationFactor = this.durationUnit.ScalingFactorFor(duration);
            var total = this.durationUnit.Convert(duration, this.TotalTime);
            return new TimerValue(this.Rate.Scale(rate), this.Histogram.Scale(durationFactor), this.ActiveSessions, total, duration);
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
            this.RateUnit = rateUnit;
            this.DurationUnit = durationUnit;
        }

        public TimeUnit DurationUnit { get; private set; }

        public TimeUnit RateUnit { get; private set; }
    }
}