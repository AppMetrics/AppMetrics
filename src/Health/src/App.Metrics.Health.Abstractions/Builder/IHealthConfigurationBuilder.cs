// <copyright file="IHealthConfigurationBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public interface IHealthConfigurationBuilder
    {
        /// <summary>
        /// Gets the <see cref="IHealthBuilder"/> where App Metrics Helath is configured.
        /// </summary>
        IHealthBuilder Builder { get; }

        /// <summary>
        ///         Uses the specifed <see cref="HealthOptions" /> instance for App Metrics Health core configuration.
        /// </summary>
        /// <param name="options">An <see cref="HealthOptions" /> instance used to configure core App Metrics Health options.</param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        IHealthBuilder Configure(HealthOptions options);

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="HealthOptions" /> instance for App Metrics Health core
        ///         configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="HealthOptions" />s property names.
        ///     </para>
        /// </summary>
        /// <param name="optionValues">Key value pairs for configuring App Metrics Health</param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        IHealthBuilder Configure(IEnumerable<KeyValuePair<string, string>> optionValues);

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="HealthOptions" /> instance for App Metrics Health core
        ///         configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="HealthOptions" />s property names. Any make key will override the
        ///         <see cref="HealthOptions" /> value configured.
        ///     </para>
        /// </summary>
        /// <param name="options">An <see cref="HealthOptions" /> instance used to configure core App Metrics Health options.</param>
        /// <param name="optionValues">Key value pairs for configuring App Metrics</param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        IHealthBuilder Configure(HealthOptions options, IEnumerable<KeyValuePair<string, string>> optionValues);

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="HealthOptions" /> instance for App Metrics Health core
        ///         configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="HealthOptions" />s property names. Any make key will override the
        ///         <see cref="HealthOptions" /> value configured.
        ///     </para>
        /// </summary>
        /// <param name="setupAction">An <see cref="HealthOptions" /> setup action used to configure core App Metrics Health options.</param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        IHealthBuilder Configure(Action<HealthOptions> setupAction);

        /// <summary>
        ///     Merges the specifed <see cref="HealthOptions" /> instance with any previously configured options.
        /// </summary>
        /// <param name="options">An <see cref="HealthOptions" /> instance used to configure core App Metrics Health options.</param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        IHealthBuilder Extend(HealthOptions options);

        /// <summary>
        ///     Merges the specifed <see cref="HealthOptions" /> instance with any previously configured options.
        /// </summary>
        /// <param name="optionValues">An <see cref="KeyValuePair{TKey,TValue}"/> used to configure core App Metrics Health options.</param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        IHealthBuilder Extend(IEnumerable<KeyValuePair<string, string>> optionValues);
    }
}