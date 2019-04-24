// <copyright file="RandomBufferGenerator.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace GrafanaCloudHostedMetricsSandboxMvc.Controllers
{
    public class RandomBufferGenerator
    {
        private readonly int _maxBufferSize;
        private readonly Random _random = new Random();
        private readonly byte[] _seedBuffer;

        public RandomBufferGenerator(int maxBufferSize)
        {
            _maxBufferSize = maxBufferSize;
            _seedBuffer = new byte[maxBufferSize];

            _random.NextBytes(_seedBuffer);
        }

        public byte[] GenerateBufferFromSeed()
        {
            var size = _random.Next(_maxBufferSize);
            var randomWindow = _random.Next(0, size);
            var buffer = new byte[size];

            Buffer.BlockCopy(_seedBuffer, randomWindow, buffer, 0, size - randomWindow);
            Buffer.BlockCopy(_seedBuffer, 0, buffer, size - randomWindow, randomWindow);

            return buffer;
        }
    }
}