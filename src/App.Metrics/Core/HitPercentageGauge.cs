// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Data;

namespace App.Metrics.Core
{
    public class HitPercentageGauge : PercentageGauge
    {
        /// <summary>
        ///     Creates a new HitPercentageGauge with externally tracked Meters, and uses the OneMinuteRate from the MeterValue of
        ///     the meters.
        /// </summary>
        /// <param name="hitMeter"></param>
        /// <param name="totalMeter"></param>
        public HitPercentageGauge(IMeter hitMeter, IMeter totalMeter)
            : this(hitMeter, totalMeter, value => value.OneMinuteRate)
        {
        }

        /// <summary>
        ///     Creates a new HitPercentageGauge with externally tracked Meters, and uses the provided meter rate function to
        ///     extract the value for the percentage.
        /// </summary>
        /// <param name="hitMeter">The numerator meter to use.</param>
        /// <param name="totalMeter">The denominator meter to use.</param>
        /// <param name="meterRateFunc">
        ///     The function to extract a value from the MeterValue. Will be applied to both the numerator
        ///     and denominator meters.
        /// </param>
        public HitPercentageGauge(IMeter hitMeter, IMeter totalMeter, Func<MeterValue, double> meterRateFunc)
            : base(() => meterRateFunc(ValueReader.GetCurrentValue(hitMeter)), () => meterRateFunc(ValueReader.GetCurrentValue(totalMeter)))
        {
        }


        /// <summary>
        ///     Creates a new HitPercentageGauge with externally tracked Meter and Timer, and uses the OneMinuteRate from the
        ///     MeterValue of the meters.
        /// </summary>
        /// <param name="hitMeter">The numerator meter to use.</param>
        /// <param name="totalTimer">The denominator meter to use.</param>
        public HitPercentageGauge(IMeter hitMeter, ITimer totalTimer)
            : this(hitMeter, totalTimer, value => value.OneMinuteRate)
        {
        }


        /// <summary>
        ///     Creates a new HitPercentageGauge with externally tracked Meter and Timer, and uses the provided meter rate function
        ///     to extract the value for the percentage.
        /// </summary>
        /// <param name="hitMeter">The numerator meter to use.</param>
        /// <param name="totalTimer">The denominator timer to use.</param>
        /// <param name="meterRateFunc">
        ///     The function to extract a value from the MeterValue. Will be applied to both the numerator
        ///     and denominator meters.
        /// </param>
        public HitPercentageGauge(IMeter hitMeter, ITimer totalTimer, Func<MeterValue, double> meterRateFunc)
            : base(() => meterRateFunc(ValueReader.GetCurrentValue(hitMeter)), () => meterRateFunc(ValueReader.GetCurrentValue(totalTimer).Rate))
        {
        }
    }
}