// <copyright file="StatsDSampler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Formatting.StatsD.Internal
{
    internal class StatsDSampler
    {
        private double _accumulator;
        private long _lastTimestamp;
        private double _sampleRate;

        public StatsDSampler(double sampleRate)
        {
            SampleRate = sampleRate;
            _accumulator = 1.0;
            _lastTimestamp = 0;
        }

        public double SampleRate
        {
            get => _sampleRate;
            set
            {
                if (value < 0.0 || value > 1.0)
                {
                    throw new IndexOutOfRangeException("SampleRate must be in the range of 0.0 and 1.0 inclusive.");
                }

                _sampleRate = value;
            }
        }

        public bool Sample()
            => Sample(StatsDSyntax.FormatTimestamp(DateTime.UtcNow));

        public bool Sample(long timeStamp)
        {
            if (_lastTimestamp == timeStamp)
            {
                return true;
            }

            _lastTimestamp = timeStamp;

            var epsilon = 1.0 - double.Epsilon;
            var result = _accumulator >= epsilon;

            if (result)
            {
                _accumulator--;
            }

            _accumulator += SampleRate;

            return result;
        }
    }
}