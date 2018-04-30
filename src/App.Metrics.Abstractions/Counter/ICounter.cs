// <copyright file="ICounter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Counter
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
        ///     track and decrement another counter associated with the <paramref name="setItem" />.
        ///     The counter value will contain the total count and for each item the specific count and percentage of total count.
        /// </summary>
        /// <param name="setItem">Item from the set for which to increment the counter value.</param>
        void Decrement(string setItem);

        void Decrement(MetricSetItem setItem);

        /// <summary>
        ///     Decrement the counter value with a specified amount.
        /// </summary>
        /// <param name="amount">The amount with which to increment the counter.</param>
        void Decrement(long amount);

        /// <summary>
        ///     Decrement the counter value with a specified amount for an item from a set.
        ///     The counter value is decremented but the counter will also keep track and decrement another counter associated with
        ///     the <paramref name="setItem" />.
        ///     The counter value will contain the total count and for each item the specific count and percentage of total count.
        /// </summary>
        /// <param name="setItem">Item from the set for which to increment the counter value.</param>
        /// <param name="amount">The amount with which to increment the counter.</param>
        void Decrement(string setItem, long amount);

        void Decrement(MetricSetItem setItem, long amount);

        /// <summary>
        ///     Increment the counter value.
        /// </summary>
        void Increment();

        /// <summary>
        ///     Increment the counter value for an item from a set.
        ///     The counter value is incremented but the counter will also keep track and increment another counter associated with
        ///     the <paramref name="setItem" />.
        ///     The counter value will contain the total count and for each item the specific count and percentage of total count.
        /// </summary>
        /// <param name="setItem">Item from the set for which to increment the counter value.</param>
        void Increment(string setItem);

        void Increment(MetricSetItem setItem);

        /// <summary>
        ///     Increment the counter value with a specified amount.
        /// </summary>
        /// <param name="amount">The amount with which to increment the counter.</param>
        void Increment(long amount);

        /// <summary>
        ///     Increment the counter value with a specified amount for an item from a set.
        ///     The counter value is incremented but the counter will also keep track and increment another counter associated with
        ///     the <paramref name="setItem" />.
        ///     The counter value will contain the total count and for each item the specific count and percentage of total count.
        /// </summary>
        /// <param name="setItem">Item from the set for which to increment the counter value.</param>
        /// <param name="amount">The amount with which to increment the counter.</param>
        void Increment(string setItem, long amount);

        void Increment(MetricSetItem setItem, long amount);
    }
}