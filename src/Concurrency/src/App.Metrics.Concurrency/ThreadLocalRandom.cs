// <copyright file="ThreadLocalRandom.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;

// Written by Iulian Margarintescu and will retain the same license as the Java Version
// Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/ConcurrencyUtilities/ThreadLocalRandom.cs
// Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained
namespace App.Metrics.Concurrency
{
    /// <summary>
    ///     Helper class to generate Random values is a thread safe way. Not suitable for cryptographic operations.
    /// </summary>
    public static class ThreadLocalRandom
    {
        private static readonly ThreadLocal<Random> LocalRandom = new ThreadLocal<Random>(
            () => new Random(Interlocked.Increment(ref _seed)));

        private static int _seed = Environment.TickCount;

        public static int Next() { return LocalRandom.Value.Next(); }

        public static int Next(int maxValue) { return LocalRandom.Value.Next(maxValue); }

        public static int Next(int minValue, int maxValue) { return LocalRandom.Value.Next(minValue, maxValue); }

        public static void NextBytes(byte[] buffer) { LocalRandom.Value.NextBytes(buffer); }

        public static double NextDouble() { return LocalRandom.Value.NextDouble(); }

        // ReSharper disable MemberCanBePrivate.Global
        public static long NextLong()
            // ReSharper restore MemberCanBePrivate.Global
        {
            long heavy = LocalRandom.Value.Next();
            long light = LocalRandom.Value.Next();
            return heavy << 32 | light;
        }

        public static long NextLong(long max)
        {
            if (max == 0)
            {
                return 0;
            }

            const int bitsPerLong = 63;
            long bits, val;
            do
            {
                bits = NextLong() & ~(1L << bitsPerLong);
                val = bits % max;
            }
            while (bits - val + (max - 1) < 0L);

            return val;
        }
    }
}