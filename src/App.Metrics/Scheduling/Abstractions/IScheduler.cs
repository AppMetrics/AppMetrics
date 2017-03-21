// <copyright file="IScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Scheduling.Abstractions
{
    public interface IScheduler : IDisposable
    {
        /// <summary>
        ///     Schedules at Task to run at the specified interval.
        /// </summary>
        /// <param name="pollInterval">The poll interval.</param>
        /// <param name="taskCreationOptions">The task creation options.</param>
        /// <param name="action">The action to run at the specified interval.</param>
        /// <param name="token">The Tasks cancellation token, each Tasks tokens are linked to this instance</param>
        /// <returns>A Task which executes the specified action at the give poll interval</returns>
        Task Interval(
            TimeSpan pollInterval,
            TaskCreationOptions taskCreationOptions,
            Action action,
            CancellationToken token);

        /// <summary>
        ///     Schedules at Task to run at the specified interval. All Tasks created with this instance share the same
        ///     cancellation token
        /// </summary>
        /// <param name="pollInterval">The poll interval.</param>
        /// <param name="taskCreationOptions">The task creation options.</param>
        /// <param name="action">The action to run at the specified interval.</param>
        /// <returns>A Task which executes the specified action at the give poll interval</returns>
        Task Interval(TimeSpan pollInterval, TaskCreationOptions taskCreationOptions, Action action);

        /// <summary>
        ///     Stops the Tasks schduled with this instance by cancelling the cancellation token
        /// </summary>
        void Stop();
    }
}