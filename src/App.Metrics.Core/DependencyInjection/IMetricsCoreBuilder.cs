// <copyright file="IMetricsCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     An interface for configuring essential App Metrics services.
    /// </summary>
    public interface IMetricsCoreBuilder
    {
        /// <summary>
        ///     Gets the <see cref="IServiceCollection" /> where Metrics services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}