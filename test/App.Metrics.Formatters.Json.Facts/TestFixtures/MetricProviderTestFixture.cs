// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.Core;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.Tagging;
using App.Metrics.Timer;

namespace App.Metrics.Formatters.Json.Facts.TestFixtures
{
    public class MetricProviderTestFixture : IDisposable
    {
        public string ApdexNameDefault = "test_apdex";
        public string ApdexNameWithGroup = "test_apdex_with_group";

        public string CounterNameDefault = "test_counter";
        public string CounterNameWithGroup = "test_counter_with_group";

        public string GaugeNameDefault = "test_gauge";
        public string GaugeNameWithGroup = "test_gauge_with_group";

        public string GroupDefault = "test_group";

        public string HistogramNameDefault = "test_histogram";
        public string HistogramNameWithGroup = "test_histogram_with_group";

        public string MeterNameDefault = "test_meter";
        public string MeterNameWithGroup = "test_meter_with_group";

        public string TimerNameDefault = "test_timer";
        public string TimerNameWithGroup = "test_timer_with_group";

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

        public MetricsContextValueSource ContextOne { get; }

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

        public MetricTags Tags => new MetricTags(new[] { "host", "env" }, new[] { "server1", "staging" });

        public IEnumerable<TimerValueSource> Timers { get; }

        public void Dispose() { }

        public IEnumerable<ApdexValueSource> SetupApdexScores()
        {
            var apdexValue = new ApdexValue(0.9, 170, 20, 10, 200);
            var apdex = new ApdexValueSource(ApdexNameDefault, ConstantValue.Provider(apdexValue), Tags);

            var apdexValueWithGroup = new ApdexValue(0.9, 170, 20, 10, 200);
            var apdexWithGroup = new ApdexValueSource(ApdexNameWithGroup, GroupDefault, ConstantValue.Provider(apdexValueWithGroup), Tags);

            return new[] { apdex, apdexWithGroup };
        }

        public IEnumerable<GaugeValueSource> SetupGauges()
        {
            var gauge = new GaugeValueSource(GaugeNameDefault, ConstantValue.Provider(0.5), Unit.Calls, Tags);
            var gaugeWithGroup = new GaugeValueSource(GaugeNameWithGroup, GroupDefault, ConstantValue.Provider(0.5), Unit.Calls, Tags);

            return new[] { gauge, gaugeWithGroup };
        }

        public IEnumerable<HistogramValueSource> SetupHistograms()
        {
            var histogramValue = new HistogramValue(1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var histogram = new HistogramValueSource(HistogramNameDefault, ConstantValue.Provider(histogramValue), Unit.Items, Tags);

            var histogramValueWithGroup = new HistogramValue(1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var histogramWithGroup = new HistogramValueSource(
                HistogramNameWithGroup,
                GroupDefault,
                ConstantValue.Provider(histogramValueWithGroup),
                Unit.Items,
                Tags);

            return new[] { histogram, histogramWithGroup };
        }

        public IEnumerable<MeterValueSource> SetupMeters()
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

            var meterValueWithGroup = new MeterValue(
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
            var meterWithGroup = new MeterValueSource(
                MeterNameWithGroup,
                GroupDefault,
                ConstantValue.Provider(meterValueWithGroup),
                Unit.Calls,
                TimeUnit.Seconds,
                Tags);

            return new[] { meter, meterWithGroup };
        }

        public IEnumerable<TimerValueSource> SetupTimers()
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
            var histogramValue = new HistogramValue(count, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);

            var timerValue = new TimerValue(meterValue, histogramValue, 0, 1, TimeUnit.Nanoseconds);
            var timer = new TimerValueSource(
                TimerNameDefault,
                ConstantValue.Provider(timerValue),
                Unit.Requests,
                TimeUnit.Seconds,
                TimeUnit.Milliseconds,
                Tags);

            var timerValueWithGroup = new TimerValue(meterValue, histogramValue, 0, 1, TimeUnit.Nanoseconds);

            var timerWithGroup = new TimerValueSource(
                TimerNameWithGroup,
                GroupDefault,
                ConstantValue.Provider(timerValueWithGroup),
                Unit.Requests,
                TimeUnit.Seconds,
                TimeUnit.Milliseconds,
                Tags);

            return new[] { timer, timerWithGroup };
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

            var counterWithGroup = new CounterValueSource(
                CounterNameWithGroup,
                GroupDefault,
                ConstantValue.Provider(counterValue),
                Unit.Items,
                Tags);

            return new[] { counter, counterWithGroup };
        }

        private MetricsDataValueSource SetupMetricsData(IEnumerable<MetricsContextValueSource> contextValueSources)
        {
            return new MetricsDataValueSource(_clock.UtcDateTime, Env, contextValueSources);
        }
    }
}