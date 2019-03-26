// <copyright file="CounterMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Formatters.Json
{
    /// <summary>
    ///     <para>
    ///         Counter metric types track how many times something happens, they can incremented or decremented.
    ///     </para>
    ///     <para>
    ///         Counters represent a 64-bit integer value.
    ///     </para>
    ///     <para>
    ///         Counters provide the ability to track a counter for each item in a finite set, as well as tracking a per item
    ///         count the overall percentage is also recorded. This is useful for example if we needed to track the total
    ///         number of emails sent but also the count of each type of emails sent.
    ///     </para>
    /// </summary>
    /// <seealso cref="MetricBase" />
    public sealed class CounterMetric : MetricBase
    {
        public long Count { get; set; }

        public IEnumerable<SetItem> Items { get; set; } = Enumerable.Empty<SetItem>();

        public sealed class SetItem
        {
            public long Count { get; set; }

            public string Item { get; set; }

            public double Percent { get; set; }
        }
    }
}