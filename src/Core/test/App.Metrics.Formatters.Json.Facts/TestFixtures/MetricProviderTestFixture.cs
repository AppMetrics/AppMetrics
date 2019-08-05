// <copyright file="MetricProviderTestFixture.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.FactsCommon;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Formatters.Json.Facts.TestFixtures
{
    public class MetricProviderTestFixture : IDisposable
    {
        private readonly IClock _clock = new TestClock();

        public MetricProviderTestFixture()
        {
            Counters = SetupCounters(false);
            ResetCounters = SetupCounters(true);
            Meters = SetupMeters();
            Gauges = SetupGauges();
            Timers = SetupTimers();
            BucketTimers = SetupBucketTimers();
            ApdexScores = SetupApdexScores();
            Histograms = SetupHistograms();
            BucketHistograms = SetupBucketHistograms();
            ContextOne = SetupContextOne();
            DataWithOneContext = SetupMetricsData(new[] { ContextOne });
            ApdexContext = SetupMetricsData(new[] { new MetricsContextValueSource("context_one", Enumerable.Empty<GaugeValueSource>(), Enumerable.Empty<CounterValueSource>(), Enumerable.Empty<MeterValueSource>(), Enumerable.Empty<HistogramValueSource>(), Enumerable.Empty<BucketHistogramValueSource>(), Enumerable.Empty<TimerValueSource>(), Enumerable.Empty<BucketTimerValueSource>(), ApdexScores) });
            CounterContext = SetupMetricsData(new[] { new MetricsContextValueSource("context_one", Enumerable.Empty<GaugeValueSource>(), Counters, Enumerable.Empty<MeterValueSource>(), Enumerable.Empty<HistogramValueSource>(), Enumerable.Empty<BucketHistogramValueSource>(), Enumerable.Empty<TimerValueSource>(), Enumerable.Empty<BucketTimerValueSource>(), Enumerable.Empty<ApdexValueSource>()) });
            ResetCounterContext = SetupMetricsData(new[] { new MetricsContextValueSource("context_one", Enumerable.Empty<GaugeValueSource>(), ResetCounters, Enumerable.Empty<MeterValueSource>(), Enumerable.Empty<HistogramValueSource>(), Enumerable.Empty<BucketHistogramValueSource>(), Enumerable.Empty<TimerValueSource>(), Enumerable.Empty<BucketTimerValueSource>(), Enumerable.Empty<ApdexValueSource>()) });
            GaugeContext = SetupMetricsData(new[] { new MetricsContextValueSource("context_one", Gauges, Enumerable.Empty<CounterValueSource>(), Enumerable.Empty<MeterValueSource>(), Enumerable.Empty<HistogramValueSource>(), Enumerable.Empty<BucketHistogramValueSource>(), Enumerable.Empty<TimerValueSource>(), Enumerable.Empty<BucketTimerValueSource>(), Enumerable.Empty<ApdexValueSource>()) });
            MeterContext = SetupMetricsData(new[] { new MetricsContextValueSource("context_one", Enumerable.Empty<GaugeValueSource>(), Enumerable.Empty<CounterValueSource>(), Meters, Enumerable.Empty<HistogramValueSource>(), Enumerable.Empty<BucketHistogramValueSource>(), Enumerable.Empty<TimerValueSource>(), Enumerable.Empty<BucketTimerValueSource>(), Enumerable.Empty<ApdexValueSource>()) });
            TimerContext = SetupMetricsData(new[] { new MetricsContextValueSource("context_one", Enumerable.Empty<GaugeValueSource>(), Enumerable.Empty<CounterValueSource>(), Enumerable.Empty<MeterValueSource>(), Enumerable.Empty<HistogramValueSource>(), Enumerable.Empty<BucketHistogramValueSource>(), Timers, Enumerable.Empty<BucketTimerValueSource>(), Enumerable.Empty<ApdexValueSource>()) });
            BucketTimerContext = SetupMetricsData(new[] { new MetricsContextValueSource("context_one", Enumerable.Empty<GaugeValueSource>(), Enumerable.Empty<CounterValueSource>(), Enumerable.Empty<MeterValueSource>(), Enumerable.Empty<HistogramValueSource>(), Enumerable.Empty<BucketHistogramValueSource>(), Enumerable.Empty<TimerValueSource>(), BucketTimers, Enumerable.Empty<ApdexValueSource>()) });
            HistogramContext = SetupMetricsData(new[] { new MetricsContextValueSource("context_one", Enumerable.Empty<GaugeValueSource>(), Enumerable.Empty<CounterValueSource>(), Enumerable.Empty<MeterValueSource>(), Histograms, Enumerable.Empty<BucketHistogramValueSource>(), Enumerable.Empty<TimerValueSource>(), Enumerable.Empty<BucketTimerValueSource>(), Enumerable.Empty<ApdexValueSource>()) });
            BucketHistogramContext = SetupMetricsData(new[] { new MetricsContextValueSource("context_one", Enumerable.Empty<GaugeValueSource>(), Enumerable.Empty<CounterValueSource>(), Enumerable.Empty<MeterValueSource>(), Enumerable.Empty<HistogramValueSource>(), BucketHistograms, Enumerable.Empty<TimerValueSource>(), Enumerable.Empty<BucketTimerValueSource>(), Enumerable.Empty<ApdexValueSource>()) });
        }

        public string ApdexNameDefault { get; } = "test_apdex";

        public IEnumerable<ApdexValueSource> ApdexScores { get; }

        public string CounterNameDefault { get; } = "test_counter";

        public IEnumerable<CounterValueSource> Counters { get; }

        public IEnumerable<CounterValueSource> ResetCounters { get; }

        public MetricsDataValueSource DataWithOneContext { get; }

        public MetricsDataValueSource CounterContext { get; }

        public MetricsDataValueSource ResetCounterContext { get; }

        public MetricsDataValueSource GaugeContext { get; }

        public MetricsDataValueSource MeterContext { get; }

        public MetricsDataValueSource TimerContext { get; }

        public MetricsDataValueSource BucketTimerContext { get; }

        public MetricsDataValueSource HistogramContext { get; }

        public MetricsDataValueSource BucketHistogramContext { get; }

        public MetricsDataValueSource ApdexContext { get; }

        public EnvironmentInfo Env => new EnvironmentInfo(
            "development",
            "framework",
            "assembly_name",
            "assembly_version",
            "localtime",
            "machine_name",
            "os",
            "os_version",
            "os_arch",
            "process_arch",
            "8");

        public string GaugeNameDefault { get; } = "test_gauge";

        public IEnumerable<GaugeValueSource> Gauges { get; }

        public string HistogramNameDefault { get; } = "test_histogram";

        public string BucketHistogramNameDefault { get; } = "test_bucket_histogram";

        public IEnumerable<HistogramValueSource> Histograms { get; }

        public IEnumerable<BucketHistogramValueSource> BucketHistograms { get; }

        public string MeterNameDefault { get; } = "test_meter";

        public IEnumerable<MeterValueSource> Meters { get; }

        public string TimerNameDefault { get; } = "test_timer";

        public IEnumerable<TimerValueSource> Timers { get; }

        public string BucketTimerNameDefault { get; } = "test_bucket_timer";

        public IEnumerable<BucketTimerValueSource> BucketTimers { get; }

        private MetricsContextValueSource ContextOne { get; }

        private MetricTags Tags => new MetricTags(new[] { "host", "env" }, new[] { "server1", "staging" });

        public void Dispose() { }

        private IEnumerable<ApdexValueSource> SetupApdexScores()
        {
            var apdexValue = new ApdexValue(0.9, 170, 20, 10, 200);
            var apdex = new ApdexValueSource(ApdexNameDefault, ConstantValue.Provider(apdexValue), Tags);

            return new[] { apdex };
        }

        private MetricsContextValueSource SetupContextOne()
        {
            return new MetricsContextValueSource("context_one", Gauges, Counters, Meters, Histograms, BucketHistograms, Timers, BucketTimers, ApdexScores);
        }

        private IEnumerable<CounterValueSource> SetupCounters(bool resetOnReporting)
        {
            var counterValue = new DefaultCounterMetric();
            counterValue.Increment("item1", 20);
            counterValue.Increment("item2", 40);
            counterValue.Increment("item3", 140);
            var counter = new CounterValueSource(CounterNameDefault, counterValue, Unit.Items, Tags, resetOnReporting);

            return new[] { counter };
        }

        private IEnumerable<GaugeValueSource> SetupGauges()
        {
            var gauge = new GaugeValueSource(GaugeNameDefault, ConstantValue.Provider(0.5), Unit.Calls, Tags);

            return new[] { gauge };
        }

        private IEnumerable<HistogramValueSource> SetupHistograms()
        {
            var histogramValue = new HistogramValue(1, 1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var histogram = new HistogramValueSource(HistogramNameDefault, ConstantValue.Provider(histogramValue), Unit.Items, Tags);

            return new[] { histogram };
        }

        private IEnumerable<BucketHistogramValueSource> SetupBucketHistograms()
        {
            var histogramValue = new BucketHistogramValue(1, 1, new Dictionary<double, double> { { 1, 1 } });
            var histogram = new BucketHistogramValueSource(BucketHistogramNameDefault, ConstantValue.Provider(histogramValue), Unit.Items, Tags);

            return new[] { histogram };
        }


#pragma warning disable SA1118 // Parameter must not span multiple lines

        private IEnumerable<MeterValueSource> SetupMeters()
        {
            var meterValue = new MeterValue(
                5,
                1,
                2,
                3,
                4,
                TimeUnit.Seconds,
                new[]
                {
                    new MeterValue.SetItem("item", 0.5, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]))
                });
            var meter = new MeterValueSource(MeterNameDefault, ConstantValue.Provider(meterValue), Unit.Calls, TimeUnit.Seconds, Tags);

            var unused = new MeterValue(
                5,
                1,
                2,
                3,
                4,
                TimeUnit.Seconds,
                new[]
                {
                    new MeterValue.SetItem("item", 0.5, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]))
                });

            return new[] { meter };
        }

        private MetricsDataValueSource SetupMetricsData(IEnumerable<MetricsContextValueSource> contextValueSources)
        {
            return new MetricsDataValueSource(_clock.UtcDateTime, contextValueSources);
        }

        private IEnumerable<TimerValueSource> SetupTimers()
        {
            const int count = 5;

            var meterValue = new MeterValue(
                count,
                1,
                2,
                3,
                4,
                TimeUnit.Seconds,
                new[]
                {
                    new MeterValue.SetItem("item", 0.5, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]))
                });
            var histogramValue = new HistogramValue(count, 1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);

            var timerValue = new TimerValue(meterValue, histogramValue, 0, TimeUnit.Nanoseconds);
            var timer = new TimerValueSource(
                TimerNameDefault,
                ConstantValue.Provider(timerValue),
                Unit.Requests,
                TimeUnit.Seconds,
                TimeUnit.Milliseconds,
                Tags);

            return new[] { timer };
        }

        private IEnumerable<BucketTimerValueSource> SetupBucketTimers()
        {
            const int count = 5;

            var meterValue = new MeterValue(
                count,
                1,
                2,
                3,
                4,
                TimeUnit.Seconds,
                new[]
                {
                    new MeterValue.SetItem("item", 0.5, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]))
                });
            var histogramValue = new BucketHistogramValue(count, 1, new Dictionary<double, double> { { 1, 1 } });

            var timerValue = new BucketTimerValue(meterValue, histogramValue, 0, TimeUnit.Nanoseconds);
            var timer = new BucketTimerValueSource(
                BucketTimerNameDefault,
                ConstantValue.Provider(timerValue),
                Unit.Requests,
                TimeUnit.Seconds,
                TimeUnit.Milliseconds,
                Tags);

            return new[] { timer };
        }
    }
#pragma warning restore SA1118 // Parameter must not span multiple lines
}