// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Concurrent;
using System.Linq;

namespace App.Metrics
{
    /// <summary>
    ///     <para>
    ///         Metric items can be used with <see cref="Counter" /> or <see cref="Meter" /> <see cref="MetricType" />s
    ///     </para>
    ///     <para>
    ///         They provide the ability to track either a count or rate for each item in a counters or meters finite set
    ///         respectively. They also track the overall percentage of each item in the set.
    ///     </para>
    ///     <para>
    ///         This is useful for example if we needed to track the total number of emails sent but also the count of each
    ///         type of emails sent or The total rate of emails sent but also the rate at which type of email was sent.
    ///     </para>
    /// </summary>
    /// <seealso cref="App.Metrics.Metric" />
    public sealed class MetricItem : ConcurrentDictionary<string, string>
    {
        public override string ToString() { return string.Join("|", this.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Key + ":" + kvp.Value)); }

        public MetricItem With(string tag, string value)
        {
            TryAdd(tag, value);

            return this;
        }
    }
}