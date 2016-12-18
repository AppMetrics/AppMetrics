using App.Metrics.Internal;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Core.Options
{
    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Apdex">Apdex</see> allows us to measure an apdex score which is a ratio of
    ///     the number of satisfied and tolerating requests to the total requests made. Each satisfied request counts as one
    ///     request, while each tolerating request counts as half a satisfied request.
    ///     <para>
    ///         Apdex tracks three response counts, counts based on samples measured by the chosen
    ///         <see cref="IReservoir">reservoir</see>
    ///         <remarks>
    ///             The <see cref="IReservoir">reservoir</see>  is determined by the <see cref="SamplingType" />sampling
    ///             type
    ///         </remarks>
    ///         when the apdex metric instance is instantiated.
    ///     </para>
    /// </summary>
    /// <seealso cref="App.Metrics.Core.Options.MetricValueWithSamplingOption" />
    public class ApdexOptions : MetricValueWithSamplingOption
    {
        public ApdexOptions()
        {
            SamplingType = SamplingType.ExponentiallyDecaying;
            SampleSize = Constants.ReservoirSampling.DefaultSampleSize;
            ExponentialDecayFactor = Constants.ReservoirSampling.DefaultExponentialDecayFactor;
            ApdexTSeconds = Constants.ReservoirSampling.DefaultApdexTSeconds;
        }

        /// <summary>
        ///     Gets or sets the apdex t seconds.
        ///     <para>
        ///         Satisfied, Tolerated and Frustrated request counts are calculated as follows using a user value of T seconds.
        ///     </para>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 Satisfied: T or less
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 Tolerated: Greater than T or less than 4T
        ///             </description>
        ///         </item>
        ///         <item>
        ///             </description>
        ///             Frustrated: Greater than 4 T
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <value>
        ///     The apdex T seconds used in calculating the score on the samples collected.
        /// </value>
        public double ApdexTSeconds { get; set; }
    }
}