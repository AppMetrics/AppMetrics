// <copyright file="IMetricsCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     An interface for configuring essential Metrics services.
    /// </summary>
    public interface IMetricsCoreBuilder
    {
        IAppMetricsEnvironment Environment { get; }

        /// <summary>
        ///     Gets the <see cref="IServiceCollection" /> where Metrics services are configured.
        /// </summary>
        /// <value>
        ///     The <see cref="IServiceCollection" /> where Metrics services are configured.
        /// </value>
        IServiceCollection Services { get; }
    }
}