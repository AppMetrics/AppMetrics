// <copyright file="GcEventsCollectorHostedService.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Extensions.Collectors.EventListeners;
using App.Metrics.Extensions.Collectors.MetricsRegistries;
using Microsoft.Extensions.Hosting;

namespace App.Metrics.Extensions.Collectors.HostedServices
{
    public class GcEventsCollectorHostedService : IHostedService, IDisposable
    {
        private readonly IMetrics _metrics;
        private readonly MetricsGcEventsCollectorOptions _options;
        private GcEventListener _gcEventListener;
        private System.Threading.Timer _timer;

        public GcEventsCollectorHostedService(IMetrics metrics, MetricsGcEventsCollectorOptions options)
        {
            _metrics = metrics;
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Threading.Timer(CollectData, null, 0, _options.CollectIntervalMilliseconds);
            _gcEventListener = new GcEventListener(_metrics);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }

        private void CollectData(object state)
        {
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.Gen0Collections, GC.CollectionCount(0));
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.Gen1Collections, GC.CollectionCount(1));
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.Gen2Collections, GC.CollectionCount(2));
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.LiveObjectsSize, GC.GetTotalMemory(false));
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _gcEventListener?.Dispose();
        }
    }
}