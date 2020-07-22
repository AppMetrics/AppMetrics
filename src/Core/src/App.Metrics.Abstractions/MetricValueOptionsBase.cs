// <copyright file="MetricValueOptionsBase.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics
{
    /// <summary>
    ///     Configuration of a Metric that will be measured
    /// </summary>
    public abstract class MetricValueOptionsBase
    {
        protected MetricValueOptionsBase() { MeasurementUnit = Unit.None; }

        /// <summary>
        ///     Gets or sets the context for which the metric belongs e.g. Application.WebRequests
        /// </summary>
        /// <value>
        ///     The context.
        /// </value>
        public string Context { get; set; }

        /// <summary>
        ///     Gets or sets the description of what is being measured, a <see cref="Unit" /> is something that is expressed in MB,
        ///     kB for example
        /// </summary>
        /// <value>
        ///     The measurement unit.
        /// </value>
        public Unit MeasurementUnit { get; set; }

        /// <summary>
        ///     Gets or sets the name of the Metric being measure, this should be unique per <see cref="MetricType" />
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="MetricTags" /> associated with the metric, this is useful for grouping metrics when
        ///     visualizing
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public MetricTags Tags { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the Metric should be reset when it is reported, otherwise values
        ///     are cumulative. Note: If using more than one reporter, the count will be reset for the first reporter which sends
        ///     the value. Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [reset on reporting]; otherwise, <c>false</c>.
        /// </value>
        public bool ResetOnReporting { get; set; }
    }
}
