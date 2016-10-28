// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace App.Metrics.Core
{
    public class TimerOptions : MetricValueWithSamplingOption
    {
        public TimerOptions()
        {
            DurationUnit = TimeUnit.Milliseconds;
            RateUnit = TimeUnit.Milliseconds;
            SamplingType = SamplingType.ExponentiallyDecaying;
        }
        public TimeUnit DurationUnit { get; set; }

        public TimeUnit RateUnit { get; set; }

    }
}