// <copyright file="IProvideMeterMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Options;
using App.Metrics.Tagging;

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

        /// <summary>
        ///     Instantiates an instance of a <see cref="IMeter" />
        /// </summary>
        /// <param name="options">The details of the <see cref="IMeter" />  that is being marked</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <returns>
        ///     A new instance of an <see cref="IMeter" /> or the existing registered instance of the meter
        /// </returns>
        IMeter Instance(MeterOptions options, MetricTags tags);

        /// <summary>
        ///     Instantiates an instance of a <see cref="IMeter" />
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IMeter" /> to instantiate</typeparam>
        /// <param name="options">The details of the <see cref="IMeter" />  that is being marked</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="builder">The function used to build the meter metric.</param>
        /// <returns>
        ///     A new instance of an <see cref="IMeter" /> or the existing registered instance of the meter
        /// </returns>
        IMeter Instance<T>(MeterOptions options, MetricTags tags, Func<T> builder)
            where T : IMeterMetric;
    }
}