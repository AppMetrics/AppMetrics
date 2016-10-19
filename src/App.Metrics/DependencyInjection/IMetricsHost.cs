// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading;
using App.Metrics;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public interface IMetricsHost
    {
        IServiceCollection Services { get; }

        IMetricsEnvironment Environment { get; }

        void RunReports(CancellationToken token);
    }
}