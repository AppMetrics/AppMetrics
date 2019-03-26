// <copyright file="MultiContextMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Meter;

namespace ReportingSandbox.JustForTesting
{
    public class MultiContextMetrics
    {
        private readonly ICounter _firstCounter;
        private readonly ICounter _secondCounter;
        private readonly IMeter _secondMeter;

        public MultiContextMetrics(IMetrics metrics)
        {
            _firstCounter = metrics.Provider.Counter.Instance(SampleMetricsRegistry.Contexts.FirstContext.Counters.Counter);
            _secondCounter = metrics.Provider.Counter.Instance(
                SampleMetricsRegistry.Contexts.SecondContext.Counters.Counter);
            _secondMeter = metrics.Provider.Meter.Instance(SampleMetricsRegistry.Contexts.SecondContext.Meters.Requests);
        }

        public void Run()
        {
            _firstCounter.Increment();
            _secondCounter.Increment();
            _secondMeter.Mark();
        }
    }
}