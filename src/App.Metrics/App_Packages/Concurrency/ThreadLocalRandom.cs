// This is a collection of .NET concurrency utilities, inspired by the classes
// available in java. This utilities are written by Iulian Margarintescu as described here
// https://github.com/etishor/ConcurrencyUtilities
// 
//
// Striped64 & LongAdder classes were ported from Java and had this copyright:
// 
// Written by Doug Lea with assistance from members of JCP JSR-166
// Expert Group and released to the public domain, as explained at
// http://creativecommons.org/publicdomain/zero/1.0/
// 
// Source: http://gee.cs.oswego.edu/cgi-bin/viewcvs.cgi/jsr166/src/jsr166e/Striped64.java?revision=1.8

//
// By default all added classes are internal to your assembly. 
// To make them public define you have to define the conditional compilation symbol CONCURRENCY_UTILS_PUBLIC in your project properties.
//

#pragma warning disable 1591

// ReSharper disable All

using System;
using System.Threading;

namespace App.Metrics.App_Packages.Concurrency
{
    /// <summary>
    ///     Helper class to generate Random values is a thread safe way. Not suitable for cryptographic operations.
    /// </summary>
#if CONCURRENCY_UTILS_PUBLIC
public
#else
    internal
#endif
        static class ThreadLocalRandom
    {
        private static readonly ThreadLocal<Random> LocalRandom = new ThreadLocal<Random>(() => new Random(Thread.CurrentThread.ManagedThreadId));

        public static int Next()
        {
            return LocalRandom.Value.Next();
        }

        public static int Next(int maxValue)
        {
            return LocalRandom.Value.Next();
        }

        public static int Next(int minValue, int maxValue)
        {
            return LocalRandom.Value.Next();
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