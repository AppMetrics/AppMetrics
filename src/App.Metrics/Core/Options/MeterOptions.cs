// <copyright file="MeterOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.MetricTypes;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace App.Metrics.Core.Options
{
    /// <summary>
    ///     Configuration of an <see cref="IMeterMetric" /> that will be measured
    /// </summary>
    /// <seealso cref="MetricValueOptionsBase" />
    public class MeterOptions : MetricValueOptionsBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MeterOptions" /> class.
        /// </summary>
        public MeterOptions()
        {
            RateUnit = TimeUnit.Minutes;
        }

        /// <summary>
        ///     Gets or sets the rate unit used for visualization which defaults to Minutes
        /// </summary>
        /// <value>
        ///     The rate unit.
        /// </value>
        public TimeUnit RateUnit { get; set; }
    }

    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper restore MemberCanBePrivate.Global
}