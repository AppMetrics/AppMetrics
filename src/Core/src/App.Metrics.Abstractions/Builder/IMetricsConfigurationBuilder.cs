// <copyright file="IMetricsConfigurationBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public interface IMetricsConfigurationBuilder
    {
        /// <summary>
        /// Gets the <see cref="IMetricsBuilder"/> where App Metrics is configured.
        /// </summary>
        IMetricsBuilder Builder { get; }

        /// <summary>
        ///         Uses the specifed <see cref="MetricsOptions" /> instance for App Metrics core configuration.
        /// </summary>
        /// <param name="options">An <see cref="MetricsOptions" /> instance used to configure core App Metrics options.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Configure(MetricsOptions options);

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsOptions" /> instance for App Metrics core
        ///         configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsOptions" />s property names.
        ///     </para>
        /// </summary>
        /// <param name="optionValues">Key value pairs for configuring App Metrics</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Configure(IEnumerable<KeyValuePair<string, string>> optionValues);

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsOptions" /> instance for App Metrics core
        ///         configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsOptions" />s property names. Any make key will override the
        ///         <see cref="MetricsOptions" /> value configured.
        ///     </para>
        /// </summary>
        /// <param name="options">An <see cref="MetricsOptions" /> instance used to configure core App Metrics options.</param>
        /// <param name="optionValues">Key value pairs for configuring App Metrics</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Configure(MetricsOptions options, IEnumerable<KeyValuePair<string, string>> optionValues);

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsOptions" /> instance for App Metrics core
        ///         configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsOptions" />s property names. Any make key will override the
        ///         <see cref="MetricsOptions" /> value configured.
        ///     </para>
        /// </summary>
        /// <param name="setupAction">An <see cref="MetricsOptions" /> setup action used to configure core App Metrics options.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Configure(Action<MetricsOptions> setupAction);

        /// <summary>
        ///     Merges the specifed <see cref="MetricsOptions" /> instance with any previously configured options.
        /// </summary>
        /// <param name="options">An <see cref="MetricsOptions" /> instance used to configure core App Metrics options.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Extend(MetricsOptions options);

        /// <summary>
        ///     Merges the specifed <see cref="MetricsOptions" /> instance with any previously configured options.
        /// </summary>
        /// <param name="optionValues">An <see cref="KeyValuePair{TKey,TValue}"/> used to configure core App Metrics options.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Extend(IEnumerable<KeyValuePair<string, string>> optionValues);
    }
}