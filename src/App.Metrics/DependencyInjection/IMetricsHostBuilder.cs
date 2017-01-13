// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics;

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