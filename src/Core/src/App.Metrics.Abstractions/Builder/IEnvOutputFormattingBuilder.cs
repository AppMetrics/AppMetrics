// <copyright file="IEnvOutputFormattingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Formatters;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public interface IEnvOutputFormattingBuilder
    {
        /// <summary>
        /// Gets the <see cref="IMetricsBuilder"/> where App Metrics is configured.
        /// </summary>
        IMetricsBuilder Builder { get; }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IEnvOutputFormatter"/> as one of the available formatters when reporting environment information.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputEnvFormatter"/> will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <param name="formatter">An <see cref="IEnvOutputFormatter"/> instance used to format environment information when reporting.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using(IEnvOutputFormatter formatter);

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IEnvOutputFormatter"/> as one of the available formatters when reporting environment information.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputEnvFormatter"/> will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <typeparam name="TEvnOutputFormatter">An <see cref="IEnvOutputFormatter"/> type used to format environment information when reporting.</typeparam>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using<TEvnOutputFormatter>()
            where TEvnOutputFormatter : IEnvOutputFormatter, new();
    }
}