// <copyright file="DefaultMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Infrastructure;

namespace App.Metrics.Internal
{
    /// <summary>
    ///     Provides access to record application metrics.
    /// </summary>
    /// <remarks>
    ///     This is the entry point to the application's metrics registry
    /// </remarks>
    /// <seealso cref="IMetrics" />
    public sealed class DefaultMetrics : IMetrics
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetrics" /> class.
        /// </summary>
        /// <param name="clock">The clock.</param>
        /// <param name="filter">The metrics filter.</param>
        /// <param name="measureMetricsProvider">The factory used to provide access to metric managers.</param>
        /// <param name="metricsBuilder">The factory used to provide access to metric builders.</param>
        /// <param name="metricsProvider">The metrics advanced manager factory.</param>
        /// <param name="dataManager">The data manager.</param>
        /// <param name="metricsManager">The metrics manager.</param>
        public DefaultMetrics(
            IClock clock,
            IFilterMetrics filter,
            IMeasureMetrics measureMetricsProvider,
            IBuildMetrics metricsBuilder,
            IProvideMetrics metricsProvider,
            IProvideMetricValues dataManager,
            IManageMetrics metricsManager)
        {
            Clock = clock ?? new StopwatchClock();
            Filter = filter ?? new NoOpMetricsFilter();
            Measure = measureMetricsProvider;
            Build = metricsBuilder;
            Snapshot = dataManager;
            Provider = metricsProvider;
            Manage = metricsManager;
        }

        /// <inheritdoc />
        public IBuildMetrics Build { get; }

        /// <inheritdoc />
        public IClock Clock { get; }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; }

        /// <inheritdoc />
        public IManageMetrics Manage { get; }

        /// <inheritdoc />
        public IMeasureMetrics Measure { get; }

        /// <inheritdoc />
        public IProvideMetrics Provider { get; }

        /// <inheritdoc />
        public IProvideMetricValues Snapshot { get; }
    }
}