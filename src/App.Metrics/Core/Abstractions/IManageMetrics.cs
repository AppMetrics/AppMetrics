// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Core.Abstractions
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