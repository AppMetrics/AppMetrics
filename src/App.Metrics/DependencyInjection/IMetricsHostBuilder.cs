// <copyright file="IMetricsHostBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Infrastructure;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace
    public interface IMetricsHostBuilder
    {
        IMetricsEnvironment Environment { get; }

        IServiceCollection Services { get; }
    }
}