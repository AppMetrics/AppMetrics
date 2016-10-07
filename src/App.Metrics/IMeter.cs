namespace App.Metrics
{
    /// <summary>
    ///     A meter measures the rate at which a set of events occur, in a few different ways.
    ///     The mean rate is the average rate of events. It’s generally useful for trivia,
    ///     but as it represents the total rate for your application’s entire lifetime (e.g., the total number of requests
    ///     handled,
    ///     divided by the number of seconds the process has been running), it doesn’t offer a sense of recency.
    ///     Luckily, meters also record three different exponentially-weighted moving average rates: the 1-, 5-, and 15-minute
    ///     moving averages.
    /// </summary>
    public interface IMeter : ResetableMetric
    {
        /// <summary>
        ///     Mark the occurrence of an event.
        /// </summary>
        void Mark();

        /// <summary>
        ///     Mark the occurrence of an event for an item in a set.
        ///     The total rate of the event is updated, but the meter will also keep track and update a specific rate for each
        ///     <paramref name="item" /> registered.
        ///     The meter value will contain the total rate and for each registered item the specific rate and percentage of total
        ///     count.
        /// </summary>
        /// <param name="item">Item from the set for which to record the event.</param>
        void Mark(string item);

        /// <summary>
        ///     Mark the occurrence of <paramref name="count" /> events.
        /// </summary>
        /// <param name="count"></param>
        void Mark(long count);

        /// <summary>
        ///     Mark the occurrence of <paramref name="count" /> events for an item in a set.
        ///     The total rate of the event is updated, but the meter will also keep track and update a specific rate for each
        ///     <paramref name="item" /> registered.
        ///     The meter value will contain the total rate and for each registered item the specific rate and percentage of total
        ///     count.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="item">Item from the set for which to record the events.</param>
        void Mark(string item, long count);
    }
}