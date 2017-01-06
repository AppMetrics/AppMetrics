// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Internal;

namespace App.Metrics.Data
{
    /// <summary>
    ///     The value reported by an Apdex Metric
    /// </summary>
    public sealed class ApdexValue
    {
        public readonly int Frustrating;
        public readonly int SampleSize;

        public readonly int Satisfied;
        public readonly double Score;
        public readonly int Tolerating;

        public ApdexValue(double score, int satisfied, int tolerating, int frustrating, int sampleSize, bool allowWarmup = true)
        {
            Satisfied = satisfied;
            Tolerating = tolerating;
            Frustrating = frustrating;
            SampleSize = sampleSize;

            //DEVNOTE: Allow some warmup time before calculating apdex scores
            if (allowWarmup && sampleSize <= Constants.ReservoirSampling.ApdexRequiredSamplesBeforeCalculating)
            {
                Score = 1;
            }
            else
            {
                Score = double.IsNaN(score) ? 0 : Math.Round(score, 2, MidpointRounding.AwayFromZero);
            }
        }
    }
}