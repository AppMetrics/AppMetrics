// <copyright file="AppMetricsReservoirSamplingConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics
{
    public static class AppMetricsReservoirSamplingConstants
    {
        public const int ApdexRequiredSamplesBeforeCalculating = 100;
        public const double DefaultApdexTSeconds = 0.5;
        public const double DefaultExponentialDecayFactor = 0.015;

        /// <summary>
        ///     Samples with weight of less than 10% of average should be discarded when rescaling
        /// </summary>
        public const double DefaultMinimumSampleWeight = 0.001;

        public const int DefaultSampleSize = 1028;
    }
}