// <copyright file="HitRatioGauge.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Meter;
using App.Metrics.Timer;

// ReSharper disable MemberCanBePrivate.Global
namespace App.Metrics.Gauge
    // ReSharper restore MemberCanBePrivate.Global
{
    public sealed class HitRatioGauge : RatioGauge
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HitRatioGauge" /> class.
        /// </summary>
        /// <param name="hitMeter">The hit meter.</param>
        /// <param name="totalMeter">The total meter.</param>
        /// <remarks>
        ///     Creates a new HitRatioGauge with externally tracked Meters, and uses the OneMinuteRate from the MeterValue of the
        ///     meters.
        /// </remarks>
        public HitRatioGauge(IMeter hitMeter, IMeter totalMeter)
            : this(hitMeter, totalMeter, value => value.OneMinuteRate) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HitRatioGauge" /> class.
        /// </summary>
        /// <param name="hitMeter">The numerator meter to use for the ratio.</param>
        /// <param name="totalMeter">The denominator meter to use for the ratio.</param>
        /// <param name="meterRateFunc">
        ///     The function to extract a value from the MeterValue. Will be applied to both the numerator
        ///     and denominator meters.
        /// </param>
        /// <remarks>
        ///     Creates a new HitRatioGauge with externally tracked Meters, and uses the provided meter rate function to extract
        ///     the value for the ratio.
        /// </remarks>
        public HitRatioGauge(IMeter hitMeter, IMeter totalMeter, Func<MeterValue, double> meterRateFunc)
            : base(() => meterRateFunc(hitMeter.GetValueOrDefault()), () => meterRateFunc(totalMeter.GetValueOrDefault())) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HitRatioGauge" /> class.
        /// </summary>
        /// <param name="hitMeter">The numerator meter to use for the ratio.</param>
        /// <param name="totalTimer">The denominator meter to use for the ratio.</param>
        /// <remarks>
        ///     Creates a new HitRatioGauge with externally tracked Meter and Timer, and uses the OneMinuteRate from the MeterValue
        ///     of the meters.
        /// </remarks>
        public HitRatioGauge(IMeter hitMeter, ITimer totalTimer)
            : this(hitMeter, totalTimer, value => value.OneMinuteRate) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HitRatioGauge" /> class.
        /// </summary>
        /// <param name="hitMeter">The numerator meter to use for the ratio.</param>
        /// <param name="totalTimer">The denominator timer to use for the ratio.</param>
        /// <param name="meterRateFunc">
        ///     The function to extract a value from the MeterValue. Will be applied to both the numerator
        ///     and denominator meters.
        /// </param>
        /// <remarks>
        ///     Creates a new HitRatioGauge with externally tracked Meter and Timer, and uses the provided meter rate function to
        ///     extract the value for the ratio.
        /// </remarks>
        public HitRatioGauge(IMeter hitMeter, ITimer totalTimer, Func<MeterValue, double> meterRateFunc)
            : base(() => meterRateFunc(hitMeter.GetValueOrDefault()), () => meterRateFunc(totalTimer.GetValueOrDefault().Rate)) { }
    }

    // ReSharper restore MemberCanBePrivate.Global
}