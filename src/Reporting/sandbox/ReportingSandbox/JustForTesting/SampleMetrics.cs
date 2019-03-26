// <copyright file="SampleMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace ReportingSandbox.JustForTesting
{
    public class SampleMetrics
    {
        private static IMetrics _metrics;

        private readonly ICounter _concurrentRequestsCounter;
        private readonly IHistogram _histogramOfData;
        private readonly IMeter _meter;
        private readonly ICounter _setCounter;
        private readonly IMeter _setMeter;
        private readonly ITimer _timer;
        private readonly ICounter _totalRequestsCounter;

        private double _someValue = 1;

        public SampleMetrics(IMetrics metrics)
        {
            _metrics = metrics;
            _concurrentRequestsCounter = _metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.ConcurrentRequestsCounter);
            _histogramOfData = _metrics.Provider.Histogram.Instance(SampleMetricsRegistry.Histograms.ResultsExample);
            _meter = _metrics.Provider.Meter.Instance(SampleMetricsRegistry.Meters.Requests);
            _setCounter = _metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.SetCounter);
            _setMeter = _metrics.Provider.Meter.Instance(SampleMetricsRegistry.Meters.SetMeter);
            _timer = _metrics.Provider.Timer.Instance(SampleMetricsRegistry.Timers.Requests);
            _totalRequestsCounter = _metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.Requests);

            // define a simple gauge that will provide the instant value of someValue when requested
            _metrics.Measure.Gauge.SetValue(SampleMetricsRegistry.Gauges.DataValue, () => _someValue);

            _metrics.Measure.Gauge.SetValue(
                SampleMetricsRegistry.Gauges.CustomRatioGauge,
                () => _totalRequestsCounter.GetValueOrDefault().Count / _meter.GetValueOrDefault().FiveMinuteRate);

            _metrics.Measure.Gauge.SetValue(SampleMetricsRegistry.Gauges.Ratio, () => new HitRatioGauge(_meter, _timer, m => m.OneMinuteRate));
        }

        public void Request(int i)
        {
            var multiContextMetrics = new MultiContextMetrics(_metrics);
            multiContextMetrics.Run();

            for (var j = 0; j < 5; j++)
            {
                var multiContextInstanceMetrics = new MultiContextInstanceMetrics(_metrics);
                multiContextInstanceMetrics.Run();
            }

            using (_timer.NewContext(i.ToString())) // measure until disposed
            {
                _someValue *= i + 1; // will be reflected in the gauge

                _concurrentRequestsCounter.Increment(); // increment concurrent requests counter

                _totalRequestsCounter.Increment(); // increment total requests counter

                _meter.Mark(); // signal a new request to the meter

                _histogramOfData.Update(new Random().Next(5000), "user-value-" + i); // update the histogram with the input data

                var item = "Item " + new Random().Next(5);
                _setCounter.Increment(item);

                _setMeter.Mark(item);

                // simulate doing some work
                var ms = Math.Abs(new Random().Next(3000));
                Thread.Sleep(ms);

                _concurrentRequestsCounter.Decrement(); // decrement number of concurrent requests
            }
        }

        public void Run()
        {
            var tasks = new List<Thread>();
            for (var i = 0; i < 10; i++)
            {
                var j = i;
                tasks.Add(new Thread(() => Request(j)));
            }

            tasks.ForEach(t => t.Start());
            tasks.ForEach(t => t.Join());
        }
    }
}