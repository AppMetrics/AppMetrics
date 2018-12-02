// <copyright file="ApdexValue.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Apdex
{
    /// <summary>
    ///     The value reported by an Apdex Metric
    /// </summary>
    public sealed class ApdexValue
    {
        public ApdexValue(double score, int satisfied, int tolerating, int frustrating, int sampleSize, bool allowWarmup = true)
        {
            Satisfied = satisfied;
            Tolerating = tolerating;
            Frustrating = frustrating;
            SampleSize = sampleSize;

            // DEVNOTE: Allow some warmup time before calculating apdex scores
            if (allowWarmup && sampleSize <= AppMetricsReservoirSamplingConstants.ApdexRequiredSamplesBeforeCalculating)
            {
                Score = 1;
            }
            else
            {
                Score = double.IsNaN(score) ? 0 : Math.Round(score, 2, MidpointRounding.AwayFromZero);
            }
        }

        public int Frustrating { get; }

        public int SampleSize { get; }

        public int Satisfied { get; }

        public double Score { get; }

        public int Tolerating { get; }
    }
}