using System;
using System.Collections.Generic;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;

namespace App.Metrics.Formatters.Json.Facts.TestFixtures
{
    public class MetricProviderTestFixture : IDisposable
    {
        public MetricProviderTestFixture()
        {
            Counters = SetupCounters();
            Meters = SetupMeters();
            Gauges = SetupGauges();
            Timers = SetupTimers();
            Histograms = SetupHistograms();
            GroupOne = SetupGroupOne();
            DataWithOneGroup = SetupMetricsData(new[] { GroupOne });
        }

        public IEnumerable<CounterValueSource> Counters { get; }

        public MetricsData DataWithOneGroup { get; }

        public EnvironmentInfo Env => new EnvironmentInfo("assembly_name", "assembly_version", "host_name", "ip_address", "localtime", "machine_name",
            "os", "os_version", "process_name", "8");

        public IEnumerable<GaugeValueSource> Gauges { get; }

        public MetricsDataGroup GroupOne { get; }

        public IEnumerable<HistogramValueSource> Histograms { get; }

        public IEnumerable<MeterValueSource> Meters { get; }

        public MetricTags Tags => new MetricTags().With("host", "server1").With("env", "staging");

        public IEnumerable<TimerValueSource> Timers { get; }

        public void Dispose()
        {
        }

        public IEnumerable<GaugeValueSource> SetupGauges()
        {
            var gauge = new GaugeValueSource("test_gauge", ConstantValue.Provider(0.5), Unit.Calls, Tags);
            return new[] { gauge };
        }

        public IEnumerable<HistogramValueSource> SetupHistograms()
        {
            var histogramValue = new HistogramValue(1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var histogram = new HistogramValueSource("test_histgram", ConstantValue.Provider(histogramValue), Unit.Items, Tags);
            return new[] { histogram };
        }

        public IEnumerable<MeterValueSource> SetupMeters()
        {
            var meterValue = new MeterValue(5, 1, 2, 3, 4, TimeUnit.Seconds, new[]
            {
                new MeterValue.SetItem("item", 0.5, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]))
            });
            var meter = new MeterValueSource("test2", ConstantValue.Provider(meterValue), Unit.Calls, TimeUnit.Seconds, Tags);
            return new[] { meter };
        }

        public IEnumerable<TimerValueSource> SetupTimers()
        {
            const int count = 5;

            var meterValue = new MeterValue(count, 1, 2, 3, 4, TimeUnit.Seconds, new[]
            {
                new MeterValue.SetItem("item", 0.5, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]))
            });
            var histogramValue = new HistogramValue(count, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);

            var timerValue = new TimerValue(meterValue, histogramValue, 0, 1, TimeUnit.Nanoseconds);
            var timer = new TimerValueSource("test_timer", ConstantValue.Provider(timerValue), Unit.Requests, TimeUnit.Seconds, TimeUnit.Milliseconds,
                Tags);

            return new[] { timer };
        }

        private IEnumerable<CounterValueSource> SetupCounters()
        {
            var items = new[]
            {
                new CounterValue.SetItem("item1", 20, 10),
                new CounterValue.SetItem("item2", 40, 20),
                new CounterValue.SetItem("item3", 140, 70)
            };

            var counterValue = new CounterValue(200, items);
            var counter = new CounterValueSource("test_counter", ConstantValue.Provider(counterValue), Unit.Items, Tags);

            return new[] { counter };
        }

        private MetricsDataGroup SetupGroupOne()
        {
            return new MetricsDataGroup("group_one", Gauges, Counters, Meters, Histograms, Timers);
        }

        private MetricsData SetupMetricsData(IEnumerable<MetricsDataGroup> groups)
        {
            return new MetricsData("test_context", new TestClock().UtcDateTime, Env, groups);
        }
    }
}