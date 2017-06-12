// <copyright file="IManageMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public interface IManageMetrics
    {
        /// <summary>
        ///     Disables all recording of metrics
        /// </summary>
        void Disable();

        /// <summary>
        ///     Allows resetting of all metric data at runtime.
        /// </summary>
        void Reset();

        /// <summary>
        ///     Allows clearing all metrics belonging to the specified context at runtime.
        /// </summary>
        /// <param name="context">The context.</param>
        void ShutdownContext(string context);
    }
}