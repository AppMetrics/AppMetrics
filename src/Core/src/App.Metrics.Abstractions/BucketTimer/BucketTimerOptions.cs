﻿// <copyright file="TimerOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global.
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Collections.Generic;

namespace App.Metrics.BucketTimer
{
    public class BucketTimerOptions : MetricValueOptionsBase
    {
        public BucketTimerOptions()
        {
            DurationUnit = TimeUnit.Milliseconds;
            RateUnit = TimeUnit.Minutes;
        }

        /// <summary>
        ///     Gets or sets the duration unit used for visualization which defaults to Milliseconds
        /// </summary>
        /// <value>
        ///     The duration unit.
        /// </value>
        public TimeUnit DurationUnit { get; set; }

        /// <summary>
        ///     Gets or sets the rate unit used for visualization which defaults to Minutes
        /// </summary>
        /// <value>
        ///     The rate unit.
        /// </value>
        public TimeUnit RateUnit { get; set; }

        public IEnumerable<double> Buckets { get; set; }
    }

    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper restore MemberCanBePrivate.Global
    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
}