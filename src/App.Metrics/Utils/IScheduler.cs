// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Utils
{
    /// <summary>
    ///     Indicates the ability to schedule the execution of an Action at a specified interval
    /// </summary>
    public interface IScheduler : IDisposable
    {
        /// <summary>
        ///     Schedule the <paramref name="action" /> to be executed at <paramref name="interval" />.
        /// </summary>
        /// <param name="interval">Interval at which to execute action</param>
        /// <param name="action">Action to execute</param>
        void Start(TimeSpan interval, Action action);

        /// <summary>
        ///     Schedule the <paramref name="action" /> to be executed at <paramref name="interval" />.
        /// </summary>
        /// <param name="interval">Interval at which to execute action</param>
        /// <param name="action">Action to execute</param>
        void Start(TimeSpan interval, Action<CancellationToken> action);

        /// <summary>
        ///     Schedule the <paramref name="action" /> to be executed at <paramref name="interval" />.
        ///     The returned task is await-ed on each time the <paramref name="action" /> is invoked.
        /// </summary>
        /// <param name="interval">Interval at which to execute action</param>
        /// <param name="action">Action to execute</param>
        void Start(TimeSpan interval, Func<Task> action);

        /// <summary>
        ///     Schedule the <paramref name="action" /> to be executed at <paramref name="interval" />.
        ///     The returned task is await-ed on each time the <paramref name="action" /> is invoked.
        /// </summary>
        /// <param name="interval">Interval at which to execute action</param>
        /// <param name="action">Action to execute</param>
        void Start(TimeSpan interval, Func<CancellationToken, Task> action);

        /// <summary>
        ///     Stop the scheduler.
        /// </summary>
        void Stop();
    }
}