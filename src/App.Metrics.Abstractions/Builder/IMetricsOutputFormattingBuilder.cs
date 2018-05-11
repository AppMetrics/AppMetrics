// <copyright file="IMetricsOutputFormattingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Formatters;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public interface IMetricsOutputFormattingBuilder
    {
        /// <summary>
        ///     Gets the <see cref="IMetricsBuilder" /> where App Metrics is configured.
        /// </summary>
        IMetricsBuilder Builder { get; }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IMetricsOutputFormatter" /> as one of the available formatters when reporting
        ///         metric values.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputMetricsFormatter" />
        ///         will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <param name="formatter">An <see cref="IMetricsOutputFormatter" /> instance used to format metric values when reporting.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using(IMetricsOutputFormatter formatter);

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IMetricsOutputFormatter" /> as one of the available formatters when reporting
        ///         metric values.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputMetricsFormatter" />
        ///         will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <param name="fields">The metric fields to write as well as thier names.</param>
        /// <typeparam name="TMetricsOutputFormatter">
        ///     An <see cref="IMetricsOutputFormatter" /> type used to format metric values
        ///     when reporting.
        /// </typeparam>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using<TMetricsOutputFormatter>(MetricFields fields = null)
            where TMetricsOutputFormatter : IMetricsOutputFormatter, new();

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IMetricsOutputFormatter" /> as one of the available formatters when reporting
        ///         metric values.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputMetricsFormatter" />
        ///         will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <param name="formatter">An <see cref="IMetricsOutputFormatter" /> instance used to format metric values when reporting.</param>
        /// <param name="replaceExisting">
        ///     If [true] replaces matching formatter type with the formatter instance, otherwise the
        ///     existing formatter instance of matching type.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using(IMetricsOutputFormatter formatter, bool replaceExisting);

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IMetricsOutputFormatter" /> as one of the available formatters when reporting
        ///         metric values.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputMetricsFormatter" />
        ///         will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <typeparam name="TMetricsOutputFormatter">
        ///     An <see cref="IMetricsOutputFormatter" /> type used to format metric values
        ///     when reporting.
        /// </typeparam>
        /// <param name="replaceExisting">
        ///     If [true] replaces matching formatter type with the formatter instance, otherwise the
        ///     existing formatter instance of matching type.
        /// </param>
        /// <param name="fields">The metric fields to write as well as thier names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Using<TMetricsOutputFormatter>(bool replaceExisting, MetricFields fields = null)
            where TMetricsOutputFormatter : IMetricsOutputFormatter, new();
    }
}