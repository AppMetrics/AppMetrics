// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Counter.Abstractions;

namespace App.Metrics.Core.Options
{
    /// <summary>
    ///     Configuration of a <see cref="ICounter" /> that will be measured
    /// </summary>
    /// <seealso cref="MetricValueOptionsBase" />
    public class CounterOptions : MetricValueOptionsBase
    {
        public CounterOptions()
        {
            ReportItemPercentages = true;
            ReportSetItems = true;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the counter's set items should be reported. Defaults to <c>true</c>.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [report item percentages]; otherwise, <c>false</c>.
        /// </value>
        public bool ReportItemPercentages { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to report the counter's set items. Defaults to <c>true</c>.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [report set items]; otherwise, <c>false</c>.
        /// </value>
        public bool ReportSetItems { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the counter should be reset when it is reported, otherwise counts are
        ///     cummulative. Note: If using more than one reporter, the count will be reset for the first reporter which sends
        ///     the value. Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [reset on reporting]; otherwise, <c>false</c>.
        /// </value>
        public bool ResetOnReporting { get; set; }
    }
}