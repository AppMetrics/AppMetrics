// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram
{
    // Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
    // Ported/Refactored to .NET Standard Library by Allan Hardy
    internal static class MathUtils
    {
        public static int NumberOfLeadingZeros(long i)
        {
            if (i == 0)
            {
                return 64;
            }

            var n = 1;
            var x = (int)unchecked((long)((ulong)i >> 32));

            if (x == 0)
            {
                n += 32;
                x = (int)i;
            }

            if ((uint)x >> 16 == 0)
            {
                n += 16;
                x <<= 16;
            }

            if ((uint)x >> 24 == 0)
            {
                n += 8;
                x <<= 8;
            }

            if ((uint)x >> 28 == 0)
            {
                n += 4;
                x <<= 4;
            }

            if ((uint)x >> 30 == 0)
            {
                n += 2;
                x <<= 2;
            }

            n -= (int)((uint)x >> 31);

            return n;
        }
    }
}