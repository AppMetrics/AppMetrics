// <copyright file="RecordAndReportHostedService.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.Extensions.Hosting;
using static System.Console;

namespace MetricsSandbox
{
    public class RecordAndReportHostedService : IHostedService, IDisposable
    {
        private static readonly Random Rnd = new Random();
        private Timer _timer;
        private IMetricsRoot _metrics;

        public RecordAndReportHostedService(IMetricsRoot metrics) { _metrics = metrics; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DoWork(object state)
        {
            if (KeyAvailable)
            {
                var key = ReadKey(false).Key;

                if (key == ConsoleKey.Escape)
                {
                    _timer?.Change(Timeout.Infinite, 0);
                    return;
                }
            }

            Clear();

            _metrics.Measure.Counter.Increment(ApplicationsMetricsRegistry.CounterOne);
            _metrics.Measure.Counter.Increment(ApplicationsMetricsRegistry.CounterWithSetItems, "item1");
            _metrics.Measure.Gauge.SetValue(ApplicationsMetricsRegistry.GaugeOne, Rnd.Next(0, 100));
            _metrics.Measure.Histogram.Update(ApplicationsMetricsRegistry.HistogramOne, Rnd.Next(0, 100));
            _metrics.Measure.BucketHistogram.Update(ApplicationsMetricsRegistry.BucketHistogramOne, Rnd.Next(0, 100));
            _metrics.Measure.Meter.Mark(ApplicationsMetricsRegistry.MeterOne, Rnd.Next(0, 100));
            _metrics.Measure.Meter.Mark(ApplicationsMetricsRegistry.MeterWithSetItems, Rnd.Next(0, 100), "item1");

            using (_metrics.Measure.Timer.Time(ApplicationsMetricsRegistry.TimerOne))
            {
                Thread.Sleep(Rnd.Next(0, 100));
            }

            using (_metrics.Measure.BucketTimer.Time(ApplicationsMetricsRegistry.BucketTimerOne))
            {
                Thread.Sleep(Rnd.Next(0, 100));
            }

            using (_metrics.Measure.Apdex.Track(ApplicationsMetricsRegistry.ApdexOne))
            {
                Thread.Sleep(Rnd.Next(0, 100));
            }

            Task.WaitAll(_metrics.ReportRunner.RunAllAsync().ToArray());
        }
    }
}
