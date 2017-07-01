// <copyright file="IRegisterHealthChecksOptionsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Provides extensiblity on App Metrics middleware e.g. serialization options on supported endpoints.
    /// </summary>
    public interface IRegisterHealthChecksOptionsBuilder
    {
        IAppMetricsHealthChecksBuilder AppMetricsHealthChecksChecksBuilder { get; }

        IHealthCheckRegistry HealthCheckRegistry { get; }
    }
}