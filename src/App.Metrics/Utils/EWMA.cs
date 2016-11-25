// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using System.Diagnostics;
using App.Metrics.Concurrency;

namespace App.Metrics.Utils
{
    /// <summary>
    ///     An exponentially-weighted moving average.
    ///     <a href="http://www.teamquest.com/pdfs/whitepaper/ldavg1.pdf">UNIX Load Average Part 1: How It Works</a>
    ///     <a href="http://www.teamquest.com/pdfs/whitepaper/ldavg2.pdf">UNIX Load Average Part 2: Not Your Average Average</a>
    ///     <a href="http://en.wikipedia.org/wiki/Moving_average#Exponential_moving_average">EMA</a>
    /// </summary>
    public sealed class EWMA
    {
        private const int FifteenMinutes = 15;
        private const int FiveMinutes = 5;
        private const int Interval = 5;
        private const int OneMinute = 1;
        private const double SecondsPerMinute = 60.0;
        private static readonly double M1Alpha = 1 - Math.Exp(-Interval / SecondsPerMinute / OneMinute);
        private static readonly double M5Alpha = 1 - Math.Exp(-Interval / SecondsPerMinute / FiveMinutes);
        private static readonly double M15Alpha = 1 - Math.Exp(-Interval / SecondsPerMinute / FifteenMinutes);


        private readonly double _alpha;
        private readonly double _interval;

        private readonly StripedLongAdder _uncounted = new StripedLongAdder();

        private volatile bool _initialized;
        private VolatileDouble _rate = new VolatileDouble(0.0);

        public EWMA(double alpha, long interval, TimeUnit intervalUnit)
        {
            Debug.Assert(interval > 0);
            _interval = intervalUnit.ToNanoseconds(interval);
            _alpha = alpha;
        }

        public static EWMA FifteenMinuteEWMA()
        {
            return new EWMA(M15Alpha, Interval, TimeUnit.Seconds);
        }

        public static EWMA FiveMinuteEWMA()
        {
            return new EWMA(M5Alpha, Interval, TimeUnit.Seconds);
        }

        public static EWMA OneMinuteEWMA()
        {
            return new EWMA(M1Alpha, Interval, TimeUnit.Seconds);
        }

        public double GetRate(TimeUnit rateUnit)
        {
            return _rate.GetValue() * rateUnit.ToNanoseconds(1L);
        }

        public void Reset()
        {
            _uncounted.Reset();
            _rate.SetValue(0.0);
        }

        public void Tick(long externallyCounted = 0L)
        {
            var count = _uncounted.GetAndReset() + externallyCounted;

            var instantRate = count / _interval;
            if (_initialized)
            {
                var doubleRate = _rate.GetValue();
                _rate.SetValue(doubleRate + _alpha * (instantRate - doubleRate));
            }
            else
            {
                _rate.SetValue(instantRate);
                _initialized = true;
            }
        }

        public void Update(long value)
        {
            _uncounted.Add(value);
        }
    }
}