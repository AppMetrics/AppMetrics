// <copyright file="ApdexSnapshot.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Apdex
{
    public struct ApdexSnapshot
    {
        private static readonly double ApdexTimeUnitFactor = TimeUnit.Seconds.ScalingFactorFor(TimeUnit.Nanoseconds);

        public ApdexSnapshot(IEnumerable<long> samples, double apdexTSeconds)
        {
            var sampleSet = samples as long[] ?? samples.ToArray();
            var apdexTNanoseconds = apdexTSeconds * ApdexTimeUnitFactor;

            FrustratingSize = sampleSet.Count(t => t > 4.0 * apdexTNanoseconds);
            SatisfiedSize = sampleSet.Count(t => t <= apdexTNanoseconds);
            ToleratingSize = sampleSet.Count(t => t > apdexTNanoseconds && t <= 4.0 * apdexTNanoseconds);
        }

        public int FrustratingSize { get; }

        public int SatisfiedSize { get; }

        public int ToleratingSize { get; }

        public static bool operator ==(ApdexSnapshot left, ApdexSnapshot right) { return left.Equals(right); }

        public static bool operator !=(ApdexSnapshot left, ApdexSnapshot right) { return !left.Equals(right); }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is ApdexSnapshot && Equals((ApdexSnapshot)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = FrustratingSize;
                hashCode = (hashCode * 397) ^ SatisfiedSize;
                hashCode = (hashCode * 397) ^ ToleratingSize;
                return hashCode;
            }
        }

        // ReSharper disable MemberCanBePrivate.Global
        public bool Equals(ApdexSnapshot other)
        {
            return FrustratingSize == other.FrustratingSize && SatisfiedSize == other.SatisfiedSize && ToleratingSize == other.ToleratingSize;
        }

        // ReSharper restore MemberCanBePrivate.Global
    }
}