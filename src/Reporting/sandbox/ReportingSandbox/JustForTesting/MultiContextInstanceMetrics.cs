// <copyright file="MultiContextInstanceMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Timer;

namespace ReportingSandbox.JustForTesting
{
    public class MultiContextInstanceMetrics
    {
        private readonly ICounter _instanceCounter;
        private readonly ITimer _instanceTimer;

        public MultiContextInstanceMetrics(IMetrics metrics)
        {
            _instanceCounter = metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.SampleCounter);
            _instanceTimer = metrics.Provider.Timer.Instance(SampleMetricsRegistry.Timers.SampleTimer);
        }

        public void Run()
        {
            using (_instanceTimer.NewContext())
            {
                _instanceCounter.Increment();
            }
        }
    }
}