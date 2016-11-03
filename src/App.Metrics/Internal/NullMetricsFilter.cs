using App.Metrics.MetricData;

namespace App.Metrics.Internal
{
    internal sealed class NullMetricsFilter : IMetricsFilter
    {
        public bool IsMatch(string @group)
        {
            return true;
        }

        public bool IsMatch(GaugeValueSource gauge)
        {
            return true;
        }

        public bool IsMatch(CounterValueSource counter)
        {
            return true;
        }

        public bool IsMatch(MeterValueSource meter)
        {
            return true;
        }

        public bool IsMatch(HistogramValueSource histogram)
        {
            return true;
        }

        public bool IsMatch(TimerValueSource timer)
        {
            return true;
        }

        public bool ReportEnvironment => false;

        public bool ReportHealthChecks => false;
    }
}