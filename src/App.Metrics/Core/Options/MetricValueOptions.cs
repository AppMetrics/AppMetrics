// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace App.Metrics.Core.Options
{
    /// <summary>
    ///     Configuration of a Metric that will be measured
    /// </summary>
    public abstract class MetricValueOptions
    {
        protected MetricValueOptions()
        {
            Tags = new MetricTags();
            MeasurementUnit = Unit.None;
        }

        /// <summary>
        ///     The context for which the metric belongs e.g. Application.WebRequests
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        ///     Describes what is being measured, a <see cref="Unit"/> is something that is expressed in MB, kB for example
        /// </summary>        
        public Unit MeasurementUnit { get; set; }

        /// <summary>
        ///     The name of the Metric being measure, this should be unique per <see cref="MetricType" />
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The <see cref="MetricTags"/> associated with the metric, this is useful for grouping metrics when visualizing
        /// </summary>
        public MetricTags Tags { get; set; }
    }
}