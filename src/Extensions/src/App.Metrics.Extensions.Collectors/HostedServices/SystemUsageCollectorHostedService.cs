// <copyright file="SystemUsageCollectorHostedService.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Extensions.Collectors.MetricsRegistries;
using Microsoft.Extensions.Hosting;
namespace App.Metrics.Extensions.Collectors.HostedServices
{
    public class SystemUsageCollectorHostedService : IHostedService, IDisposable
    {
        private readonly Process _process = Process.GetCurrentProcess();
        private readonly IMetrics _metrics;
        private readonly MetricsSystemUsageCollectorOptions _options;
        private System.Threading.Timer _timer;
        private DateTime _lastTimeStamp;
        private TimeSpan _lastTotalProcessorTime = TimeSpan.Zero;
        private TimeSpan _lastUserProcessorTime = TimeSpan.Zero;
        private TimeSpan _lastPrivilegedProcessorTime = TimeSpan.Zero;

        public SystemUsageCollectorHostedService(IMetrics metrics, MetricsSystemUsageCollectorOptions options)
        {
            _metrics = metrics;
            _options = options;
            _lastTimeStamp = _process.StartTime;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Threading.Timer(CollectData, null, 1000, _options.CollectIntervalMilliseconds);

            return Task.CompletedTask;
        }

        private void CollectData(object state)
        {
            var totalCpuTimeUsed = _process.TotalProcessorTime.TotalMilliseconds - _lastTotalProcessorTime.TotalMilliseconds;
            var privilegedCpuTimeUsed = _process.PrivilegedProcessorTime.TotalMilliseconds - _lastPrivilegedProcessorTime.TotalMilliseconds;
            var userCpuTimeUsed = _process.UserProcessorTime.TotalMilliseconds - _lastUserProcessorTime.TotalMilliseconds;

            _lastTotalProcessorTime = _process.TotalProcessorTime;
            _lastPrivilegedProcessorTime = _process.PrivilegedProcessorTime;
            _lastUserProcessorTime = _process.UserProcessorTime;

            var cpuTimeElapsed = (DateTime.UtcNow - _lastTimeStamp).TotalMilliseconds * Environment.ProcessorCount;
            _lastTimeStamp = DateTime.UtcNow;

            _metrics.Measure.Gauge.SetValue(SystemUsageMetricsRegistry.Gauges.TotalCpuUsed, totalCpuTimeUsed * 100 / cpuTimeElapsed);
            _metrics.Measure.Gauge.SetValue(SystemUsageMetricsRegistry.Gauges.PrivilegedCpuUsed, privilegedCpuTimeUsed * 100 / cpuTimeElapsed);
            _metrics.Measure.Gauge.SetValue(SystemUsageMetricsRegistry.Gauges.UserCpuUsed, userCpuTimeUsed * 100 / cpuTimeElapsed);

            _metrics.Measure.Gauge.SetValue(SystemUsageMetricsRegistry.Gauges.MemoryWorkingSet, _process.WorkingSet64);
            _metrics.Measure.Gauge.SetValue(SystemUsageMetricsRegistry.Gauges.NonPagedSystemMemory, _process.NonpagedSystemMemorySize64);
            _metrics.Measure.Gauge.SetValue(SystemUsageMetricsRegistry.Gauges.PagedMemory, _process.PagedMemorySize64);
            _metrics.Measure.Gauge.SetValue(SystemUsageMetricsRegistry.Gauges.PagedSystemMemory, _process.PagedSystemMemorySize64);
            _metrics.Measure.Gauge.SetValue(SystemUsageMetricsRegistry.Gauges.PrivateMemory, _process.PrivateMemorySize64);
            _metrics.Measure.Gauge.SetValue(SystemUsageMetricsRegistry.Gauges.VirtualMemory, _process.VirtualMemorySize64);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }
    }
}