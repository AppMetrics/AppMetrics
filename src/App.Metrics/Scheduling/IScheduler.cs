// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Scheduling
{
    public interface IScheduler : IDisposable
    {
        Task Interval(TimeSpan pollInterval, Action action, CancellationToken token);

        Task Interval(TimeSpan pollInterval, Action action);

        void Stop();
    }
}