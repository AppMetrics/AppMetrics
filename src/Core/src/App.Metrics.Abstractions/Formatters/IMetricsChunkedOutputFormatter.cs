using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Formatters
{
    public interface IMetricsChunkedOutputFormatter : IMetricsOutputFormatter
    {
        /// <summary>
        ///     Writes the specified <see cref="MetricsDataValueSource" /> to the given stream, streaming one
        /// data point at a time.
        /// </summary>
        /// <param name="metricsData">The <see cref="MetricsDataValueSource" /> being written.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous write operation.</returns>
        Task<List<string>> WriteAsync(
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default);
    }
}
