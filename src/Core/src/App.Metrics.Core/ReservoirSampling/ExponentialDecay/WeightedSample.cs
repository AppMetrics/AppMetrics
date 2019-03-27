// <copyright file="WeightedSample.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.ReservoirSampling.ExponentialDecay
{
    // Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
    // Ported/Refactored to .NET Standard Library by Allan Hardy
    public struct WeightedSample
    {
        public readonly string UserValue;
        public readonly long Value;
        public readonly double Weight;

        public WeightedSample(long value, string userValue, double weight)
        {
            Value = value;
            UserValue = userValue;
            Weight = weight;
        }

        public static bool operator ==(WeightedSample left, WeightedSample right) { return left.Equals(right); }

        public static bool operator !=(WeightedSample left, WeightedSample right) { return !left.Equals(right); }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is WeightedSample && Equals((WeightedSample)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = UserValue?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ Value.GetHashCode();
                hashCode = (hashCode * 397) ^ Weight.GetHashCode();
                return hashCode;
            }
        }

        // ReSharper disable MemberCanBePrivate.Global
        public bool Equals(WeightedSample other)
            // ReSharper restore MemberCanBePrivate.Global
        {
            // ReSharper disable ImpureMethodCallOnReadonlyValueField
            return string.Equals(UserValue, other.UserValue) && Value == other.Value && Weight.Equals(other.Weight);
            // ReSharper restore ImpureMethodCallOnReadonlyValueField
        }
    }
}