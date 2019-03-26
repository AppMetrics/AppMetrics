// <copyright file="RequestDurationForApdexTesting.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace MetricsPrometheusSandboxMvc.JustForTesting
{
    public class RequestDurationForApdexTesting
    {
        private const int MaxRequestDurationFactor = 1000;
        private const int MinRequestDuration = 25;
        private static readonly Random Rnd = new Random();

        public RequestDurationForApdexTesting(double apdexTSeconds)
        {
            SatisfiedMinMilliseconds = MinRequestDuration;
            SatisfiedMaxMilliseconds = (int)(apdexTSeconds * 1000);

            ToleratingMinMilliseconds = SatisfiedMaxMilliseconds + 1;
            ToleratingMaxMilliseconds = 4 * SatisfiedMaxMilliseconds;

            FrustratingMinMilliseconds = ToleratingMaxMilliseconds + 1;
            FrustratingMaxMilliseconds = ToleratingMaxMilliseconds + MaxRequestDurationFactor;
        }

        public int FrustratingMaxMilliseconds { get; }

        public int FrustratingMinMilliseconds { get; }

        public int NextFrustratingDuration => Rnd.Next(FrustratingMinMilliseconds, FrustratingMaxMilliseconds);

        public int NextSatisfiedDuration => Rnd.Next(SatisfiedMinMilliseconds, SatisfiedMaxMilliseconds);

        public int NextToleratingDuration => Rnd.Next(ToleratingMinMilliseconds, ToleratingMaxMilliseconds);

        public int SatisfiedMaxMilliseconds { get; }

        public int SatisfiedMinMilliseconds { get; }

        public int ToleratingMaxMilliseconds { get; }

        public int ToleratingMinMilliseconds { get; }
    }
}