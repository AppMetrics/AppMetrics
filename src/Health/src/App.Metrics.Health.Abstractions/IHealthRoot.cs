// <copyright file="IHealthRoot.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Health.Formatters;

namespace App.Metrics.Health
{
    public interface IHealthRoot : IHealth
    {
        /// <summary>
        ///     Gets the default <see cref="IHealthOutputFormatter" /> to use when health checks are attempted to be formatted.
        /// </summary>
        /// <value>
        ///     The default <see cref="IHealthOutputFormatter" />s that is used by this application.
        /// </value>
        IHealthOutputFormatter DefaultOutputHealthFormatter { get; }

        IRunHealthChecks HealthCheckRunner { get; }

        HealthOptions Options { get; }

        /// <summary>
        ///     Gets a list of <see cref="IHealthOutputFormatter" />s that are used by this application to format health
        ///     results.
        /// </summary>
        /// <value>
        ///     A list of <see cref="IHealthOutputFormatter" />s that are used by this application.
        /// </value>
        IReadOnlyCollection<IHealthOutputFormatter> OutputHealthFormatters { get; }

        /// <summary>
        ///     Gets a readonly collection of <see cref="IReportHealthStatus" />s which provide the ability to report on health
        ///     check results.
        /// </summary>
        IReadOnlyCollection<IReportHealthStatus> Reporters { get; }
    }
}