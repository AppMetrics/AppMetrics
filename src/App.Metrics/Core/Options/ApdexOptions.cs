// <copyright file="ApdexOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Core.Internal;
using App.Metrics.ReservoirSampling.ExponentialDecay;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace App.Metrics.Core.Options
{
    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Apdex">Apdex</see> allows us to measure an apdex score which is a ratio of
    ///     the number of satisfied and tolerating requests to the total requests made. Each satisfied request counts as one
    ///     request, while each tolerating request counts as half a satisfied request.
    ///     <para>
    ///         Apdex tracks three response counts, counts based on samples measured by the chosen
    ///         <see cref="IReservoir">reservoir</see>, defaults to a <see cref="DefaultForwardDecayingReservoir" />.
    ///     </para>
    /// </summary>
    /// <seealso cref="MetricValueWithSamplingOption" />
    public class ApdexOptions : MetricValueWithSamplingOption
    {
        public ApdexOptions()
        {
            ApdexTSeconds = Constants.ReservoirSampling.DefaultApdexTSeconds;
            AllowWarmup = true;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to allow the app to warmup before calcaulting.
        ///     If set to <c>true</c> allows the service to warmup before starting to calculate the apdex,
        ///     the score will intitially be 1 until enough samples have been recorded.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [allow warmup]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowWarmup { get; set; }

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
        ///             <description>
        ///                 Frustrated: Greater than 4 T
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <value>
        ///     The apdex T seconds used in calculating the score on the samples collected.
        /// </value>
        public double ApdexTSeconds { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the counts and score should be reset when it is reported, otherwise values
        ///     are cummulative. Note: If using more than one reporter, the count will be reset for the first reporter which sends
        ///     the value. Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [reset on reporting]; otherwise, <c>false</c>.
        /// </value>
        public bool ResetOnReporting { get; set; }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper restore MemberCanBePrivate.Global
}