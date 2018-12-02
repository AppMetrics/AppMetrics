// <copyright file="IMetricsBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Builder;
using App.Metrics.Filters;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     An interface for configuring App Metrics services and options.
    /// </summary>
    public interface IMetricsBuilder
    {
        /// <summary>
        ///     Builder for configuring the <see cref="IFilterMetrics" /> used to globally filter specific metrics when reporting.
        /// </summary>
        IMetricsFilterBuilder Filter { get; }

        /// <summary>
        ///     Builder for configuring core App Metrics options.
        /// </summary>
        IMetricsConfigurationBuilder Configuration { get; }

        bool CanReport();

        IMetricFieldsBuilder MetricFields { get; }

        /// <summary>
        ///     <para>
        ///         Builder for configuring environment information output formatting for reporting.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputEnvFormatter" /> will
        ///         be set to the first configured formatter.
        ///     </para>
        /// </summary>
        IEnvOutputFormattingBuilder OutputEnvInfo { get; }

        IMetricsReportingBuilder Report { get; }

        /// <summary>
        ///     <para>
        ///         Builder for configuring metrics output formatting for reporting.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputMetricsFormatter" />
        ///         will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        IMetricsOutputFormattingBuilder OutputMetrics { get; }

        /// <summary>
        ///     Builder for configuring the default reservoir sampling using an <see cref="IMetricsBuilder" />. Reservoir sampling
        ///     is used on specific metrics types. By default is set to "DefaultForwardDecayingReservoir".
        /// </summary>
        IMetricsReservoirSamplingBuilder SampleWith { get; }

        /// <summary>
        ///     Builder for configuring the <see cref="IClock" /> used for specific metrics types which requiring timing.
        /// </summary>
        IMetricsClockBuilder TimeWith { get; }

        /// <summary>
        ///     Builds an <see cref="IMetrics" /> with the services configured via an <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <returns>An <see cref="IMetricsRoot" /> with services configured via an <see cref="IMetricsBuilder" />.</returns>
        IMetricsRoot Build();
    }
}