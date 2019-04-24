// <copyright file="IMetricsTaskSchedular.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Internal
{
    internal interface IMetricsTaskSchedular : IDisposable
    {
        void Start(TimeSpan interval);

        void SetTaskSource(Func<CancellationToken, Task> onTick);
    }
}