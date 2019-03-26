// <copyright file="ProcessSample.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics;
using App.Metrics;

namespace ReportingSandbox.JustForTesting
{
    public class ProcessSample
    {
        private readonly IMetrics _metrics;
        private readonly CpuUsage _cpuUsage;
        private readonly Process _process;

        public ProcessSample(IMetrics metrics)
        {
            _metrics = metrics;
            _cpuUsage = new CpuUsage();
            _cpuUsage.Start();
            _process = Process.GetCurrentProcess();
        }

        public void Run()
        {
            _metrics.Measure.Gauge.SetValue(
                AppMetricsRegistry.ProcessMetrics.CpuUsageTotal,
                () =>
                {
                    _cpuUsage.CallCpu();
                    return _cpuUsage.CpuUsageTotal;
                });
            _metrics.Measure.Gauge.SetValue(
                AppMetricsRegistry.ProcessMetrics.ProcessPagedMemorySizeGauge,
                () => _process.PagedMemorySize64);
            _metrics.Measure.Gauge.SetValue(
                AppMetricsRegistry.ProcessMetrics.ProcessPeekPagedMemorySizeGauge,
                () => _process.PeakPagedMemorySize64);
            _metrics.Measure.Gauge.SetValue(
                AppMetricsRegistry.ProcessMetrics.ProcessPeekVirtualMemorySizeGauge,
                () => _process.PeakVirtualMemorySize64);
            _metrics.Measure.Gauge.SetValue(
                AppMetricsRegistry.ProcessMetrics.ProcessPeekWorkingSetSizeGauge,
                () => _process.WorkingSet64);
            _metrics.Measure.Gauge.SetValue(
                AppMetricsRegistry.ProcessMetrics.ProcessPrivateMemorySizeGauge,
                () => _process.PrivateMemorySize64);
            _metrics.Measure.Gauge.SetValue(
                AppMetricsRegistry.ProcessMetrics.ProcessVirtualMemorySizeGauge,
                () => _process.VirtualMemorySize64);
            _metrics.Measure.Gauge.SetValue(
                AppMetricsRegistry.ProcessMetrics.SystemNonPagedMemoryGauge,
                () => _process.NonpagedSystemMemorySize64);
            _metrics.Measure.Gauge.SetValue(
                AppMetricsRegistry.ProcessMetrics.SystemPagedMemorySizeGauge,
                () => _process.PagedSystemMemorySize64);
        }
    }
}