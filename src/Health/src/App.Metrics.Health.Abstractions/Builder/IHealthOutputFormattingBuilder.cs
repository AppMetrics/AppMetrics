// <copyright file="IHealthOutputFormattingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Health.Formatters;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public interface IHealthOutputFormattingBuilder
    {
        /// <summary>
        ///     Gets the <see cref="IHealthBuilder" /> where App Metrics Health is configured.
        /// </summary>
        IHealthBuilder Builder { get; }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IHealthOutputFormatter" /> as one of the available formatters when reporting
        ///         metric values.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IHealthRoot.DefaultOutputHealthFormatter" />
        ///         will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <param name="formatter">An <see cref="IHealthOutputFormatter" /> instance used to format metric values when reporting.</param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IHealthBuilder Using(IHealthOutputFormatter formatter);

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IHealthOutputFormatter" /> as one of the available formatters when reporting
        ///         metric values.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IHealthRoot.DefaultOutputHealthFormatter" />
        ///         will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <typeparam name="THealthOutputFormatter">
        ///     An <see cref="IHealthOutputFormatter" /> type used to format metric values
        ///     when reporting.
        /// </typeparam>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IHealthBuilder Using<THealthOutputFormatter>()
            where THealthOutputFormatter : IHealthOutputFormatter, new();

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IHealthOutputFormatter" /> as one of the available formatters when reporting
        ///         metric values.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IHealthRoot.DefaultOutputHealthFormatter" />
        ///         will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <param name="formatter">An <see cref="IHealthOutputFormatter" /> instance used to format metric values when reporting.</param>
        /// <param name="replaceExisting">
        ///     If [true] replaces matching formatter type with the formatter instance, otherwise the
        ///     existing formatter instance of matching type.
        /// </param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IHealthBuilder Using(IHealthOutputFormatter formatter, bool replaceExisting);

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IHealthOutputFormatter" /> as one of the available formatters when reporting
        ///         metric values.
        ///     </para>
        ///     <para>
        ///         Multiple formatters can be used, in which case the <see cref="IHealthRoot.DefaultOutputHealthFormatter" />
        ///         will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <typeparam name="THealthOutputFormatter">
        ///     An <see cref="IHealthOutputFormatter" /> type used to format metric values
        ///     when reporting.
        /// </typeparam>
        /// <param name="replaceExisting">
        ///     If [true] replaces matching formatter type with the formatter instance, otherwise the
        ///     existing formatter instance of matching type.
        /// </param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IHealthBuilder Using<THealthOutputFormatter>(bool replaceExisting)
            where THealthOutputFormatter : IHealthOutputFormatter, new();
    }
}