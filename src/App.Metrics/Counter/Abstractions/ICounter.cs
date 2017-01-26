// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Counter.Abstractions
{
    /// <summary>
    ///     A counter is a simple incrementing and decrementing 64-bit integer.
    ///     Each operation can also be applied to a item from a set and the counter will store individual count for each set
    ///     item.
    /// </summary>
    public interface ICounter : IResetableMetric
    {
        void Decrement();

        /// <summary>
        ///     Decrement the counter value for an item from a set. The counter value is decremented but the counter will also keep
        ///     track and decrement another counter associated with the <paramref name="item" />.
        ///     The counter value will contain the total count and for each item the specific count and percentage of total count.
        /// </summary>
        /// <param name="item">Item from the set for which to increment the counter value.</param>
        void Decrement(string item);

        void Decrement(MetricItem item);

        /// <summary>
        ///     Decrement the counter value with a specified amount.
        /// </summary>
        /// <param name="amount">The amount with which to increment the counter.</param>
        void Decrement(long amount);

        /// <summary>
        ///     Decrement the counter value with a specified amount for an item from a set.
        ///     The counter value is decremented but the counter will also keep track and decrement another counter associated with
        ///     the <paramref name="item" />.
        ///     The counter value will contain the total count and for each item the specific count and percentage of total count.
        /// </summary>
        /// <param name="item">Item from the set for which to increment the counter value.</param>
        /// <param name="amount">The amount with which to increment the counter.</param>
        void Decrement(string item, long amount);

        void Decrement(MetricItem item, long amount);

        /// <summary>
        ///     Increment the counter value.
        /// </summary>
        void Increment();

        /// <summary>
        ///     Increment the counter value for an item from a set.
        ///     The counter value is incremented but the counter will also keep track and increment another counter associated with
        ///     the <paramref name="item" />.
        ///     The counter value will contain the total count and for each item the specific count and percentage of total count.
        /// </summary>
        /// <param name="item">Item from the set for which to increment the counter value.</param>
        void Increment(string item);

        void Increment(MetricItem item);

        /// <summary>
        ///     Increment the counter value with a specified amount.
        /// </summary>
        /// <param name="amount">The amount with which to increment the counter.</param>
        void Increment(long amount);

        /// <summary>
        ///     Increment the counter value with a specified amount for an item from a set.
        ///     The counter value is incremented but the counter will also keep track and increment another counter associated with
        ///     the <paramref name="item" />.
        ///     The counter value will contain the total count and for each item the specific count and percentage of total count.
        /// </summary>
        /// <param name="item">Item from the set for which to increment the counter value.</param>
        /// <param name="amount">The amount with which to increment the counter.</param>
        void Increment(string item, long amount);

        void Increment(MetricItem item, long amount);
    }
}