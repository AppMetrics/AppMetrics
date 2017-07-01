// <copyright file="IAppMetricsHealthChecksBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public interface IAppMetricsHealthChecksBuilder
    {
        IAppMetricsEnvironment Environment { get; }

        IServiceCollection Services { get; }
    }
}