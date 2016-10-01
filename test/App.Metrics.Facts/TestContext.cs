using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.MetricData;
using FluentAssertions;

namespace App.Metrics.Facts
{
    public class TestContext : BaseMetricsContext
    {
        public TestContext(string contextName, TestClock clock, TestScheduler scheduler)
            : base(contextName, new DefaultMetricsRegistry(), new TestMetricsBuilder(clock, scheduler), () => clock.UTCDateTime)
        {
            this.Clock = clock;
            this.Scheduler = scheduler;
        }

        private TestContext(string contextName, TestClock clock)
            : this(contextName, clock, new TestScheduler(clock))
        { }

        public TestContext()
            : this("TestContext", new TestClock())
        { }

        public TestClock Clock { get; private set; }
        public TestScheduler Scheduler { get; private set; }

        protected override MetricsContext CreateChildContextInstance(string contextName)
        {
            return new TestContext(contextName, this.Clock, this.Scheduler);
        }

        public double GaugeValue(params string[] nameWithContext)
        {
            return ValueFor<double>(GetDataFor(nameWithContext).Gauges, nameWithContext);
        }

        public CounterValue CounterValue(params string[] nameWithContext)
        {
            return ValueFor<CounterValue>(GetDataFor(nameWithContext).Counters, nameWithContext);
        }

        public MeterValue MeterValue(params string[] nameWithContext)
        {
            return ValueFor<MeterValue>(GetDataFor(nameWithContext).Meters, nameWithContext);
        }

        public HistogramValue HistogramValue(params string[] nameWithContext)
        {
            return ValueFor<HistogramValue>(GetDataFor(nameWithContext).Histograms, nameWithContext);
        }

        public TimerValue TimerValue(params string[] nameWithContext)
        {
            return ValueFor<TimerValue>(GetDataFor(nameWithContext).Timers, nameWithContext);
        }

        private T ValueFor<T>(IEnumerable<MetricValueSource<T>> values, string[] nameWithContext)
        {
            var value = values.Where(t => t.Name == nameWithContext.Last())
                .Select(t => t.Value);

            value.Should().HaveCount(1, "No metric found with name {0} in context {1}. Available names: {2}", nameWithContext.Last(),
                string.Join(".", nameWithContext.Take(nameWithContext.Length - 1)), string.Join(",", values.Select(v => v.Name)));

            return value.Single();
        }

        public MetricsData GetDataFor(params string[] nameWithContext)
        {
            return GetContextFor(nameWithContext).DataProvider.CurrentMetricsData;
        }

        public TestContext GetContextFor(params string[] nameWithContext)
        {
            if (nameWithContext.Length == 1)
            {
                return this;
            }

            return (this.Context(nameWithContext.First()) as TestContext).GetContextFor(nameWithContext.Skip(1).ToArray());
        }
    }
}
