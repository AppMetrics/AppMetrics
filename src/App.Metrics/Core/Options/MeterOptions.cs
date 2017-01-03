// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Core.Interfaces;

namespace App.Metrics.Core.Options
{
    /// <summary>
    ///     Configuration of an <see cref="IMeterMetric" /> that will be measured
    /// </summary>
    /// <seealso cref="App.Metrics.Core.Options.MetricValueOptions" />
    public class MeterOptions : MetricValueOptions
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MeterOptions" /> class.
        /// </summary>
        public MeterOptions()
        {
            RateUnit = TimeUnit.Minutes;
        }

        /// <summary>
        ///     The rate unit used for visualization which defaults to Minutes
        /// </summary>
        public TimeUnit RateUnit { get; set; }
    }
}