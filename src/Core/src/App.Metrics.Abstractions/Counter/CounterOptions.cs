// <copyright file="CounterOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Counter
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
    }
}
