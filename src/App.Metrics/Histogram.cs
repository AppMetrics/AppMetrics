namespace App.Metrics
{
    /// <summary>
    ///     A Histogram measures the distribution of values in a stream of data: e.g., the number of results returned by a
    ///     search.
    /// </summary>
    public interface Histogram : ResetableMetric
    {
        /// <summary>
        ///     Records a value.
        /// </summary>
        /// <param name="value">Value to be added to the histogram.</param>
        /// <param name="userValue">
        ///     A custom user value that will be associated to the results.
        ///     Useful for tracking (for example) for which id the max or min value was recorded.
        /// </param>
        void Update(long value, string userValue = null);
    }
}