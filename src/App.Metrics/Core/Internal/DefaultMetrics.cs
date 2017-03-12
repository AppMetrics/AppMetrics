// <copyright file="DefaultMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.Filtering;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Interfaces;
using App.Metrics.Health.Abstractions;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;

namespace App.Metrics.Core.Internal
{
    /// <summary>
    ///     Provides access to record application metrics.
    /// </summary>
    /// <remarks>
    ///     This is the entry point to the application's metrics registry
    /// </remarks>
    /// <seealso cref="IMetrics" />
    // ReSharper disable ClassNeverInstantiated.Global
    internal sealed class DefaultMetrics : IMetrics
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetrics" /> class.
        /// </summary>
        /// <param name="clock">The clock.</param>
        /// <param name="globalFilter">The global filter.</param>
        /// <param name="measureMetricsProvider">The factory used to provide access to metric managers.</param>
        /// <param name="metricsBuilder">The factory used to provide access to metric builders.</param>
        /// <param name="metricsProvider">The metrics advanced manager factory.</param>
        /// <param name="dataManager">The data manager.</param>
        /// <param name="metricsManager">The metrics manager.</param>
        /// <param name="healthProvider">The health status provider.</param>
        public DefaultMetrics(
            IClock clock,
            IFilterMetrics globalFilter,
            IMeasureMetrics measureMetricsProvider,
            IBuildMetrics metricsBuilder,
            IProvideMetrics metricsProvider,
            IProvideMetricValues dataManager,
            IManageMetrics metricsManager,
            IProvideHealth healthProvider)
        {
            Clock = clock ?? new StopwatchClock();
            GlobalFilter = globalFilter ?? new NoOpMetricsFilter();
            Measure = measureMetricsProvider;
            Build = metricsBuilder;
            Snapshot = dataManager;
            Provider = metricsProvider;
            Manage = metricsManager;
            Health = healthProvider;
        }

        /// <inheritdoc />
        public IBuildMetrics Build { get; }

        /// <inheritdoc />
        public IClock Clock { get; }

        /// <inheritdoc />
        public IFilterMetrics GlobalFilter { get; }

        /// <inheritdoc />
        public IProvideHealth Health { get; }

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