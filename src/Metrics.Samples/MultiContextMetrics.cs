
using App.Metrics;

namespace Metrics.Samples
{
    public class MultiContextMetrics
    {
        private readonly Counter firstCounter = Metric.Context("First Context").Counter("Counter", Unit.Requests);

        private readonly Counter secondCounter = Metric.Context("Second Context").Counter("Counter", Unit.Requests);
        private readonly Meter secondMeter = Metric.Context("Second Context").Meter("Meter", Unit.Errors, TimeUnit.Seconds);

        public void Run()
        {
            this.firstCounter.Increment();
            this.secondCounter.Increment();
            this.secondMeter.Mark();
        }
    }

    public class MultiContextInstanceMetrics
    {
        private readonly Counter instanceCounter;
        private readonly Timer instanceTimer;

        public MultiContextInstanceMetrics(string instanceName)
        {
            var context = Metric.Context(instanceName);

            this.instanceCounter = context.Counter("Sample Counter", Unit.Errors);
            this.instanceTimer = context.Timer("Sample Timer", Unit.Requests);
        }

        public void Run()
        {
            using (var context = this.instanceTimer.NewContext())
            {
                this.instanceCounter.Increment();
            }
        }

        public static void RunSample()
        {
            for (int i = 0; i < 5; i++)
            {
                new MultiContextInstanceMetrics("Sample Instance " + i.ToString()).Run();
            }
        }
    }
}
