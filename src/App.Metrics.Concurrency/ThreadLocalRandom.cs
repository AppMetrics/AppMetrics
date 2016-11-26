// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

//Written by Iulian Margarintescu and will retain the same license as the Java Version
//Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/ConcurrencyUtilities/ThreadLocalRandom.cs
//Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained

using System;
using System.Threading;

namespace App.Metrics.Concurrency
{
    /// <summary>
    ///     Helper class to generate Random values is a thread safe way. Not suitable for cryptographic operations.
    /// </summary>
    public static class ThreadLocalRandom
    {
        private static readonly ThreadLocal<Random> LocalRandom = new ThreadLocal<Random>(() => 
        new Random(Thread.CurrentThread.ManagedThreadId));

        public static int Next()
        {
            return LocalRandom.Value.Next();
        }

        public static int Next(int maxValue)
        {
            return LocalRandom.Value.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return LocalRandom.Value.Next(minValue, maxValue);
        }

        public static void NextBytes(byte[] buffer)
        {
            LocalRandom.Value.NextBytes(buffer);
        }

        public static double NextDouble()
        {
            return LocalRandom.Value.NextDouble();
        }

        public static long NextLong()
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

            const int BitsPerLong = 63;
            long bits, val;
            do
            {
                bits = NextLong() & (~(1L << BitsPerLong));
                val = bits % max;
            } while (bits - val + (max - 1) < 0L);
            return val;
        }
    }
}