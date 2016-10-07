using App.Metrics;

namespace Metrics.Samples
{
    public class MultiContextMetrics
    {
        private readonly ICounter _firstCounter;
        private readonly ICounter _secondCounter;
        private readonly IMeter _secondMeter;

        public MultiContextMetrics(IMetricsContext metricsContext)
        {
            _firstCounter = metricsContext.Context("First Context").Counter("Counter", Unit.Requests);
            _secondCounter = metricsContext.Context("Second Context").Counter("Counter", Unit.Requests);
            _secondMeter = metricsContext.Context("Second Context").Meter("Meter", Unit.Errors, TimeUnit.Seconds);
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
        private static IMetricsContext _metricsContext;

        public MultiContextInstanceMetrics(string instanceName, IMetricsContext metricsContext)
        {
            _metricsContext = metricsContext;

            var context = _metricsContext.Context(instanceName);

            _instanceCounter = context.Counter("Sample Counter", Unit.Errors);
            _instanceTimer = context.Timer("Sample Timer", Unit.Requests);
        }

        public void Run()
        {
            using (var context = this._instanceTimer.NewContext())
            {
                _instanceCounter.Increment();
            }
        }

        public void RunSample()
        {
            for (var i = 0; i < 5; i++)
            {
                new MultiContextInstanceMetrics("Sample Instance " + i.ToString(), _metricsContext).Run();
            }
        }
    }
}