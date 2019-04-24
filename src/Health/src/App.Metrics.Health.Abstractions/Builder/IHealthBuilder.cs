// <copyright file="IHealthBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public interface IHealthBuilder
    {
        bool CanReport();

        /// <summary>
        ///     Builder for configuring core App Metrics Health options.
        /// </summary>
        IHealthConfigurationBuilder Configuration { get; }

        IHealthCheckBuilder HealthChecks { get; }

        /// <summary>
        ///     <para>
        ///         Builder for configuring health check output formatting for reporting.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IHealthRoot.DefaultOutputHealthFormatter" />
        ///         will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        IHealthOutputFormattingBuilder OutputHealth { get; }

        /// <summary>
        ///  Builder for configuring health status reporters.
        /// </summary>
        IHealthReportingBuilder Report { get; }

        /// <summary>
        ///     Builds an <see cref="IHealth" /> with the services configured via an <see cref="IHealthBuilder" />.
        /// </summary>
        /// <returns>An <see cref="IHealthRoot" /> with services configured via an <see cref="IHealthBuilder" />.</returns>
        IHealthRoot Build();
    }
}