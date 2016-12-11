using App.Metrics;

namespace Metrics.Samples
{
    public class MultiContextMetrics
    {
        private readonly ICounter _firstCounter;
        private readonly ICounter _secondCounter;
        private readonly IMeter _secondMeter;

        public MultiContextMetrics(IMetrics metrics)
        {
            _firstCounter = metrics.Advanced.Counter(SampleMetricsRegistry.Contexts.FirstContext.Counters.Counter);
            _secondCounter = metrics.Advanced.Counter(SampleMetricsRegistry.Contexts.SecondContext.Counters.Counter);
            _secondMeter = metrics.Advanced.Meter(SampleMetricsRegistry.Contexts.SecondContext.Meters.Requests);
        }

        public void Run()
        {
            _firstCounter.Increment();
            _secondCounter.Increment();
            _secondMeter.Mark();
        }
    }

    public class MultiContextInstanceMetrics
    {
        private readonly ICounter _instanceCounter;
        private readonly ITimer _instanceTimer;
        private static IMetrics _metrics;

        public MultiContextInstanceMetrics(string instanceName, IMetrics metrics)
        {
            _metrics = metrics;
            _instanceCounter = _metrics.Advanced.Counter(SampleMetricsRegistry.Counters.SampleCounter);
            _instanceTimer = _metrics.Advanced.Timer(SampleMetricsRegistry.Timers.SampleTimer);
        }

        public void Run()
        {
            using (_instanceTimer.NewContext())
            {
                _instanceCounter.Increment();
            }
        }

        public void RunSample()
        {
            for (var i = 0; i < 5; i++)
            {
                new MultiContextInstanceMetrics("Sample Instance " + i, _metrics).Run();
            }
        }
    }
}