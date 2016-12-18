using System;

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

        public ApdexValue(double score, int satisfied, int tolerating, int frustrating, int sampleSize)
        {
            Satisfied = satisfied;
            Tolerating = tolerating;
            Frustrating = frustrating;
            SampleSize = sampleSize;
            Score = double.IsNaN(score) ? 0 : Math.Round(score, 2, MidpointRounding.AwayFromZero);
        }
    }
}