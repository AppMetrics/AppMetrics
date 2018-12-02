// <copyright file="IMeasureApdexMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Apdex
{
    /// <summary>
    ///     Provides access to the API allowing Apdex Metrics to be measured/recorded.
    /// </summary>
    public interface IMeasureApdexMetrics
    {
        /// <summary>
        ///     Records a <see cref="IApdexMetric" /> which measures the time taken to process an action, samples data and procuded
        ///     an apdex score.
        /// </summary>
        /// <param name="options">The settings of the apdex metric that is being measured</param>
        /// <param name="action">The action to measure.</param>
        void Track(ApdexOptions options, Action action);

        /// <summary>
        ///     Records a <see cref="IApdexMetric" /> which measures the time taken to process an action, samples data and procuded
        ///     an apdex score.
        /// </summary>
        /// <param name="options">The settings of the apdex metric that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="action">The action to measure.</param>
        void Track(ApdexOptions options, MetricTags tags, Action action);

        /// <summary>
        ///     Records a <see cref="IApdexMetric" /> which measures the time taken to process an action, samples data and procuded
        ///     an apdex score.
        /// </summary>
        /// <param name="options">The settings of the apdex metric that is being measured</param>
        /// <returns>
        ///     A disposable context, when disposed records the time token to process the using block
        /// </returns>
        ApdexContext Track(ApdexOptions options);

        /// <summary>
        ///     Records a <see cref="IApdexMetric" /> which measures the time taken to process an action, samples data and procuded
        ///     an apdex score.
        /// </summary>
        /// <param name="options">The settings of the apdex metric that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <returns>
        ///     A disposable context, when disposed records the time token to process the using block
        /// </returns>
        ApdexContext Track(ApdexOptions options, MetricTags tags);
    }
}