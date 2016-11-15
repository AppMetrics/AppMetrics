// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting.Interfaces
{
    public interface IReporter : IDisposable
    {
        Task RunReportsAsync(IMetrics context, CancellationToken token);
    }
}