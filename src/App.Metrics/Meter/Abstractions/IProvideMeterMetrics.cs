// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Options;

namespace App.Metrics.Meter.Abstractions
{
    public interface IProvideMeterMetrics
    {
        /// <summary>
        ///     Instantiates an instance of a <see cref="IMeter" />
        /// </summary>
        /// <param name="options">The details of the <see cref="IMeter" />  that is being marked</param>
        /// <returns>A new instance of an <see cref="IMeter" /> or the existing registered instance of the meter</returns>
        IMeter Instance(MeterOptions options);

        /// <summary>
        ///     Instantiates an instance of a <see cref="IMeter" />
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IMeter" /> to instantiate</typeparam>
        /// <param name="options">The details of the <see cref="IMeter" />  that is being marked</param>
        /// <param name="builder">The function used to build the meter metric.</param>
        /// <returns>A new instance of an <see cref="IMeter" /> or the existing registered instance of the meter</returns>
        IMeter Instance<T>(MeterOptions options, Func<T> builder)
            where T : IMeterMetric;
    }
}