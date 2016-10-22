// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{

    #region data manager

    //public interface IAppMetricsDataManager
    //{
    //    Task<MetricsData> GetMetricsAsync();

    //    Task<HealthCheckResult> GetHealthAsync();
    //}

    //public sealed class DefaultAppMetricsDataManager : IAppMetricsDataManager
    //{
    //    private readonly EnvironmentInfo _environmentInfo;
    //    private readonly IMetricRegistryManager _registry;
    //    private readonly ILogger _logger;

    //    public DefaultAppMetricsDataManager(ILoggerFactory loggerFactory,
    //        EnvironmentInfo environmentInfo,
    //        IMetricRegistryManager registry)
    //    {
    //        if (loggerFactory == null)
    //        {
    //            throw new ArgumentNullException(nameof(loggerFactory));
    //        }

    //        if (registry == null)
    //        {
    //            throw new ArgumentNullException(nameof(registry));
    //        }

    //        _logger = loggerFactory.CreateLogger<DefaultAppMetricsDataManager>();
    //        _environmentInfo = environmentInfo;
    //        _registry = registry;
    //    }


    //    public Task<MetricsData> GetMetricsAsync()
    //    {
    //        _logger.MetricsDataGetExecuting();

    //        var metricsData = new MetricsData(metricsContext.GroupName, _clock.UtcDateTime,
    //            _environmentInfo.Entries,
    //            _registry.Gauges.ToArray(),
    //            _registry.Counters.ToArray(),
    //            _registry.Meters.ToArray(),
    //            _registry.Histograms.ToArray(),
    //            _registry.Timers.ToArray(),
    //            metricsContext.Groups.Values.Select(p => p.Advanced.MetricsDataManager.GetMetricsData(p)));

    //        _logger.MetricsDataGetExecuted();

    //        return metricsData;
    //    }

    //    public Task<HealthCheckResult> GetHealthAsync()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    #endregion

    public interface IAdvancedMetricsContext : IHideObjectMembers
    {
        event EventHandler ContextDisabled;

        event EventHandler ContextShuttingDown;

        IClock Clock { get; }

        IHealthCheckManager HealthCheckManager { get; }

        IMetricsDataManager MetricsDataManager { get; }

        void CompletelyDisableMetrics();

        ICounter Counter(CounterOptions options);

        ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterImplementation;

        void Gauge(GaugeOptions options, Func<double> valueProvider);

        void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider);

        IHistogram Histogram(HistogramOptions options);

        IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramImplementation;

        IHistogram Histogram(HistogramOptions options, Func<IReservoir> builder);

        IMeter Meter(MeterOptions options);

        IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterImplementation;

        void ResetMetricsValues();

        void ShutdownGroup(string groupName);

        ITimer Timer(TimerOptions options);

        ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerImplementation;

        ITimer Timer(TimerOptions options, Func<IHistogramImplementation> builder);

        ITimer Timer(TimerOptions options, Func<IReservoir> builder);
    }
}