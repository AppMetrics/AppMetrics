// <copyright file="StatsDSampler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Formatting.StatsD.Internal
{
    internal class StatsDSampler
    {
        private readonly ThreadSafeRandom _random;

        public StatsDSampler()
        {
            _random = new ThreadSafeRandom();
        }

        public bool ShouldSend(double sampleRate)
        {
            var d = _random.NextDouble;
            return d < sampleRate;
        }
    }
}