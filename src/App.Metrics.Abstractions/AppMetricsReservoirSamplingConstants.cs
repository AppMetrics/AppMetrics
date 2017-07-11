// <copyright file="AppMetricsReservoirSamplingConstants.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics
{
    public static class AppMetricsReservoirSamplingConstants
    {
        public const int ApdexRequiredSamplesBeforeCalculating = 100;
        public const double DefaultApdexTSeconds = 0.5;
        public const double DefaultExponentialDecayFactor = 0.015;
        public const int DefaultSampleSize = 1028;
    }
}