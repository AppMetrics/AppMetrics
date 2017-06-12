// <copyright file="MetricProviderTestFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.Core;
using App.Metrics.Core.Infrastructure;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Formatters.Json.Facts.TestFixtures
{
    public class MetricProviderTestFixture : IDisposable
    {
        public string ApdexNameDefault { get; } = "test_apdex";

        public string CounterNameDefault { get; } = "test_counter";

        public string GaugeNameDefault { get; } = "test_gauge";

        public string HistogramNameDefault { get; } = "test_histogram";

        public string MeterNameDefault { get; } = "test_meter";

        public string TimerNameDefault { get; } = "test_timer";

        private readonly IClock _clock = new TestClock();

        public MetricProviderTestFixture()
        {
            Counters = SetupCounters();
            Meters = SetupMeters();
            Gauges = SetupGauges();
            Timers = SetupTimers();
            ApdexScores = SetupApdexScores();
            Histograms = SetupHistograms();
            ContextOne = SetupContextOne();
            DataWithOneContext = SetupMetricsData(new[] { ContextOne });
        }

        public IEnumerable<ApdexValueSource> ApdexScores { get; }

        public IEnumerable<CounterValueSource> Counters { get; }

        public MetricsDataValueSource DataWithOneContext { get; }

        public EnvironmentInfo Env => new EnvironmentInfo(
            "assembly_name",
            "assembly_version",
            "host_name",
            "localtime",
            "machine_name",
            "os",
            "os_version",
            "process_name",
            "8");

        public IEnumerable<GaugeValueSource> Gauges { get; }

        public IEnumerable<HistogramValueSource> Histograms { get; }

        public IEnumerable<MeterValueSource> Meters { get; }

        public IEnumerable<TimerValueSource> Timers { get; }

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
            return new MetricsContextValueSource("context_one", Gauges, Counters, Meters, Histograms, Timers, ApdexScores);
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
            var counter = new CounterValueSource(CounterNameDefault, ConstantValue.Provider(counterValue), Unit.Items, Tags);

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
    }
#pragma warning restore SA1118 // Parameter must not span multiple lines
}