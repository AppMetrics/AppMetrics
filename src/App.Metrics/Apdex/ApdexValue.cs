// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Internal;

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
            if (allowWarmup && sampleSize <= Constants.ReservoirSampling.ApdexRequiredSamplesBeforeCalculating)
            {
                Score = 1;
            }
            else
            {
                Score = double.IsNaN(score) ? 0 : Math.Round(score, 2, MidpointRounding.AwayFromZero);
            }
        }

        public int Frustrating { get; private set; }

        public int SampleSize { get; private set; }

        public int Satisfied { get; private set; }

        public double Score { get; private set; }

        public int Tolerating { get; private set; }
    }
}