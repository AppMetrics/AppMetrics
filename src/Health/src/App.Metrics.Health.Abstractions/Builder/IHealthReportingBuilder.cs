// <copyright file="IHealthReportingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public interface IHealthReportingBuilder
    {
        /// <summary>
        ///     Gets the <see cref="IHealthBuilder" /> where App Metrics Health is configured.
        /// </summary>
        IHealthBuilder Builder { get; }

        /// <summary>
        ///     Reports health status using the specified <see cref="IReportHealthStatus" />.
        /// </summary>
        /// <param name="reporter">An <see cref="IReportHealthStatus" /> instance used to report health status.</param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        IHealthBuilder Using(IReportHealthStatus reporter);

        /// <summary>
        ///     Reports metrics using the specified <see cref="IReportHealthStatus" />.
        /// </summary>
        /// <typeparam name="TReportHealth">
        ///     An <see cref="IReportHealthStatus" /> type used to report health status.
        /// </typeparam>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        IHealthBuilder Using<TReportHealth>()
            where TReportHealth : IReportHealthStatus, new();

        /// <summary>
        ///     Reports metrics using the specified <see cref="IReportHealthStatus" />.
        /// </summary>
        /// <param name="reportInterval">The <see cref="TimeSpan" /> interval used to schedule health status reporting.</param>
        /// <typeparam name="TReportHealth">
        ///     An <see cref="IReportHealthStatus" /> type used to report health status.
        /// </typeparam>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        IHealthBuilder Using<TReportHealth>(TimeSpan reportInterval)
            where TReportHealth : IReportHealthStatus, new();
    }
}