// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using App.Metrics.Data;

namespace App.Metrics.Core
{
    public sealed class HitRatioGauge : RatioGauge
    {
        /// <summary>
        ///     Creates a new HitRatioGauge with externally tracked Meters, and uses the OneMinuteRate from the MeterValue of the
        ///     meters.
        /// </summary>
        /// <param name="hitMeter"></param>
        /// <param name="totalMeter"></param>
        public HitRatioGauge(IMeter hitMeter, IMeter totalMeter)
            : this(hitMeter, totalMeter, value => value.OneMinuteRate)
        {
        }

        /// <summary>
        ///     Creates a new HitRatioGauge with externally tracked Meters, and uses the provided meter rate function to extract
        ///     the value for the ratio.
        /// </summary>
        /// <param name="hitMeter">The numerator meter to use for the ratio.</param>
        /// <param name="totalMeter">The denominator meter to use for the ratio.</param>
        /// <param name="meterRateFunc">
        ///     The function to extract a value from the MeterValue. Will be applied to both the numerator
        ///     and denominator meters.
        /// </param>
        public HitRatioGauge(IMeter hitMeter, IMeter totalMeter, Func<MeterValue, double> meterRateFunc)
            : base(() => meterRateFunc(ValueReader.GetCurrentValue(hitMeter)), () => meterRateFunc(ValueReader.GetCurrentValue(totalMeter)))
        {
        }


        /// <summary>
        ///     Creates a new HitRatioGauge with externally tracked Meter and Timer, and uses the OneMinuteRate from the MeterValue
        ///     of the meters.
        /// </summary>
        /// <param name="hitMeter">The numerator meter to use for the ratio.</param>
        /// <param name="totalTimer">The denominator meter to use for the ratio.</param>
        public HitRatioGauge(IMeter hitMeter, ITimer totalTimer)
            : this(hitMeter, totalTimer, value => value.OneMinuteRate)
        {
        }


        /// <summary>
        ///     Creates a new HitRatioGauge with externally tracked Meter and Timer, and uses the provided meter rate function to
        ///     extract the value for the ratio.
        /// </summary>
        /// <param name="hitMeter">The numerator meter to use for the ratio.</param>
        /// <param name="totalTimer">The denominator timer to use for the ratio.</param>
        /// <param name="meterRateFunc">
        ///     The function to extract a value from the MeterValue. Will be applied to both the numerator
        ///     and denominator meters.
        /// </param>
        public HitRatioGauge(IMeter hitMeter, ITimer totalTimer, Func<MeterValue, double> meterRateFunc)
            : base(() => meterRateFunc(ValueReader.GetCurrentValue(hitMeter)), () => meterRateFunc(ValueReader.GetCurrentValue(totalTimer).Rate))
        {
        }
    }
}