// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading;

namespace App.Metrics.Reporting.Abstractions
{
    public interface IReporter : IDisposable
    {
        void RunReports(IMetrics context, CancellationToken token);
    }
}