// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Core.Options;
using App.Metrics.Data.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Core.Interfaces
{
    /// <summary>
    ///     Provides access to more advanced application metrics operations
    /// </summary>
    /// <seealso cref="App.Metrics.Core.Interfaces.IMetricsDataProvider" />
    /// <seealso cref="App.Metrics.Core.Interfaces.IHealthStatusProvider" />
    /// <seealso cref="App.Metrics.Utils.IHideObjectMembers" />
    public interface IAdvancedMetrics : IMetricsDataProvider, IHealthStatusProvider
    {
        /// <summary>
        ///     Provides access to the <see cref="IClock" /> configured used to measure by some <see cref="MetricType" />s that
        ///     measure processing time
        /// </summary>
        IClock Clock { get; }

        /// <summary>
        ///     Provides access to the <see cref="IMetricsDataProvider" /> configured to retrieve a snapshot of metrics being
        ///     recorded
        /// </summary>
        IMetricsDataProvider Data { get; }

        /// <summary>
        ///     Provides access to the <see cref="IMetricsFilter" /> configured to globally filter metrics
        /// </summary>
        IMetricsFilter GlobalFilter { get; }

        /// <summary>
        ///     Gets the global tags.
        /// </summary>
        /// <value>
        ///     The global tags.
        /// </value>
        GlobalMetricTags GlobalTags { get; }

        /// <summary>
        ///     Provides access to the <see cref="IHealthStatusProvider" /> configured to retrieve the current health status of the
        ///     application by execurting the registered health checks
        /// </summary>
        IHealthStatusProvider Health { get; }

        /// <summary>
        ///     Instantiates an instance of a <see cref="ICounter" />
        /// </summary>
        /// <param name="options">The details of the counter that is being decremented</param>
        ICounter Counter(CounterOptions options);

        /// <summary>
        ///     Instantiates an instance of a <see cref="ICounter" />
        /// </summary>
        /// <remarks>
        ///     This can be used for custom <see cref="ICounter" /> implementations
        /// </remarks>
        /// <typeparam name="T">The type of <see cref="ICounter" /> to instantiate</typeparam>
        /// <param name="options">The details of the counter that is being decremented</param>
        /// <param name="builder">The function used to build the counter metric.</param>
        /// <returns>A new instance of an <see cref="ICounter" /> or the existing registered instance of the counter</returns>
        ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric;

        /// <summary>
        ///     Disables all recording of metrics
        /// </summary>
        void Disable();

        /// <summary>
        ///     Records <see cref="IGaugeMetric" /> which is a point in time instantaneous value
        /// </summary>
        /// <param name="options">The details of the gauge that is being measured.</param>
        /// <param name="valueProvider">A function that returns custom value provider for the gauge.</param>
        void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider);

        /// <summary>
        ///     Instantiates an instance of a <see cref="IHistogram" />
        /// </summary>
        /// <param name="options">The details of the histogram that is being measured</param>
        /// <returns>A new instance of an <see cref="IHistogram" /> or the existing registered instance of the histogram</returns>
        IHistogram Histogram(HistogramOptions options);

        /// <summary>
        ///     Instantiates an instance of a <see cref="IHistogram" />
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IHistogram" /> to instantiate</typeparam>
        /// <param name="options">The details of the <see cref="IHistogram" /> that is being measured</param>
        /// <param name="builder">The function used to build the histogram metric.</param>
        /// <returns>A new instance of an <see cref="IHistogram" /> or the existing registered instance of the histogram</returns>
        IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric;

        /// <summary>
        ///     Instantiates an instance of a <see cref="IMeter" />
        /// </summary>
        /// <param name="options">The details of the <see cref="IMeter" />  that is being marked</param>
        /// <returns>A new instance of an <see cref="IMeter" /> or the existing registered instance of the meter</returns>
        IMeter Meter(MeterOptions options);

        /// <summary>
        ///     Instantiates an instance of a <see cref="IMeter" />
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IMeter" /> to instantiate</typeparam>
        /// <param name="options">The details of the <see cref="IMeter" />  that is being marked</param>
        /// <param name="builder">The function used to build the meter metric.</param>
        /// <returns>A new instance of an <see cref="IMeter" /> or the existing registered instance of the meter</returns>
        IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric;

        /// <summary>
        ///     Instantiates an instance of a <see cref="ITimer" />
        /// </summary>
        /// <param name="options">The details of the <see cref="ITimer" />  that is being measured</param>
        /// <returns>A new instance of an <see cref="ITimer" /> or the existing registered instance of the timer</returns>
        ITimer Timer(TimerOptions options);

        /// <summary>
        ///     Instantiates an instance of a <see cref="ITimer" />
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ITimer" /> to instantiate</typeparam>
        /// <param name="options">The details of the <see cref="ITimer" />  that is being measured</param>
        /// <param name="builder">The function used to build the timer metric.</param>
        /// <returns>A new instance of an <see cref="ITimer" /> or the existing registered instance of the timer</returns>
        ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerMetric;

        /// <summary>
        ///     Instantiates an instance of a <see cref="ITimer" />
        /// </summary>
        /// <param name="options">The details of the <see cref="ITimer" />  that is being measured</param>
        /// <param name="builder">The function used to build a custom <see cref="IHistogramMetric" /> metric.</param>
        /// <returns>A new instance of an <see cref="ITimer" /> or the existing registered instance of the apdex metric</returns>
        ITimer Timer(TimerOptions options, Func<IHistogramMetric> builder);

        /// <summary>
        ///     Instantiates an instance of a <see cref="IApdex" />
        /// </summary>
        /// <param name="options">The details of the <see cref="IApdex" />  that is being measured</param>
        /// <returns>A new instance of an <see cref="IApdex" /> or the existing registered instance of the apdex metric</returns>
        IApdex Track(ApdexOptions options);

        /// <summary>
        ///     Instantiates an instance of a <see cref="IApdex" />
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IApdex" /> to instantiate</typeparam>
        /// <param name="options">The settings of the <see cref="IApdex" /> metric that is being measured</param>
        /// <param name="builder">The function used to build the apdex metric.</param>
        /// <returns>A new instance of an <see cref="IApdex" /> or the existing registered instance of the apdex metric</returns>
        IApdex Track<T>(ApdexOptions options, Func<T> builder) where T : IApdexMetric;
    }
}