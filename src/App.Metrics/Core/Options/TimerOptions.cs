// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Internal;

namespace App.Metrics.Core.Options
{
    public class TimerOptions : MetricValueWithSamplingOption
    {
        public TimerOptions()
        {
            DurationUnit = TimeUnit.Milliseconds;
            RateUnit = TimeUnit.Minutes;
            SamplingType = SamplingType.ExponentiallyDecaying;
            SampleSize = Constants.ReservoirSampling.DefaultSampleSize;
            ExponentialDecayFactor = Constants.ReservoirSampling.DefaultExponentialDecayFactor;
        }

        /// <summary>
        ///     The duration unit used for visualization which defaults to Milliseconds
        /// </summary>
        public TimeUnit DurationUnit { get; set; }

        /// <summary>
        ///     The rate unit used for visualization which defaults to Minutes
        /// </summary>
        public TimeUnit RateUnit { get; set; }
    }
}