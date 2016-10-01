// Written by Gil Tene of Azul Systems, and released to the public domain,
// as explained at http://creativecommons.org/publicdomain/zero/1.0/
// 
// Ported to .NET by Iulian Margarintescu under the same license and terms as the java version
// Java Version repo: https://github.com/HdrHistogram/HdrHistogram
// Latest ported version is available in the Java submodule in the root of the repo
namespace App.Metrics.App_Packages.HdrHistogram
{
    /**
 * Written by Gil Tene of Azul Systems, and released to the public domain,
 * as explained at http://creativecommons.org/publicdomain/zero/1.0/
 *
 * @author Gil Tene
 */


    /**
     * Used for iterating through all recorded histogram values using the finest granularity steps supported by the
     * underlying representation. The iteration steps through all non-zero recorded value counts, and terminates when
     * all recorded histogram values are exhausted.
     */

    internal class RecordedValuesIterator : AbstractHistogramIterator
    {
        int visitedIndex;

        /**
         * Reset iterator for re-use in a fresh iteration over the same histogram data set.
         */
        public void reset()
        {
            reset(histogram);
        }

        private void reset(AbstractHistogram histogram)
        {
            resetIterator(histogram);
            visitedIndex = -1;
        }

        /**
         * @param histogram The histogram this iterator will operate on
         */
        public RecordedValuesIterator(AbstractHistogram histogram)
        {
            reset(histogram);
        }


        protected override void incrementIterationLevel()
        {
            visitedIndex = currentIndex;
        }


        protected override bool reachedIterationLevel()
        {
            long currentCount = histogram.getCountAtIndex(currentIndex);
            return (currentCount != 0) && (visitedIndex != currentIndex);
        }
    }

}
