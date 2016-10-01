// Written by Iulian Margarintescu
// 
// Ported to .NET Standard Library by Allan Hardy
// Original repo: https://github.com/etishor/Metrics.NET

using System;
using System.Diagnostics;
using App.Metrics.App_Packages.Concurrency;

namespace App.Metrics.Utils
{
    /// <summary>
    /// An exponentially-weighted moving average.
    /// <a href="http://www.teamquest.com/pdfs/whitepaper/ldavg1.pdf">UNIX Load Average Part 1: How It Works</a>
    /// <a href="http://www.teamquest.com/pdfs/whitepaper/ldavg2.pdf">UNIX Load Average Part 2: Not Your Average Average</a>
    /// <a href="http://en.wikipedia.org/wiki/Moving_average#Exponential_moving_average">EMA</a>
    /// </summary>
    public sealed class EWMA
    {
        private const int Interval = 5;
        private const double SecondsPerMinute = 60.0;
        private const int OneMinute = 1;
        private const int FiveMinutes = 5;
        private const int FifteenMinutes = 15;
        private static readonly double M1Alpha = 1 - Math.Exp(-Interval / SecondsPerMinute / OneMinute);
        private static readonly double M5Alpha = 1 - Math.Exp(-Interval / SecondsPerMinute / FiveMinutes);
        private static readonly double M15Alpha = 1 - Math.Exp(-Interval / SecondsPerMinute / FifteenMinutes);

        private volatile bool initialized;
        private VolatileDouble rate = new VolatileDouble(0.0);

        private readonly StripedLongAdder uncounted = new StripedLongAdder();
        private readonly double alpha;
        private readonly double interval;

        public static EWMA OneMinuteEWMA()
        {
            return new EWMA(M1Alpha, Interval, TimeUnit.Seconds);
        }

        public static EWMA FiveMinuteEWMA()
        {
            return new EWMA(M5Alpha, Interval, TimeUnit.Seconds);
        }

        public static EWMA FifteenMinuteEWMA()
        {
            return new EWMA(M15Alpha, Interval, TimeUnit.Seconds);
        }

        public EWMA(double alpha, long interval, TimeUnit intervalUnit)
        {
            Debug.Assert(interval > 0);
            this.interval = intervalUnit.ToNanoseconds(interval);
            this.alpha = alpha;
        }

        public void Update(long value)
        {
            this.uncounted.Add(value);
        }

        public void Tick(long externallyCounted = 0L)
        {
            var count = this.uncounted.GetAndReset() + externallyCounted;

            var instantRate = count / this.interval;
            if (this.initialized)
            {
                var doubleRate = this.rate.GetValue();
                this.rate.SetValue(doubleRate + this.alpha * (instantRate - doubleRate));
            }
            else
            {
                this.rate.SetValue(instantRate);
                this.initialized = true;
            }
        }

        public double GetRate(TimeUnit rateUnit)
        {
            return this.rate.GetValue() * rateUnit.ToNanoseconds(1L);
        }

        public void Reset()
        {
            this.uncounted.Reset();
            this.rate.SetValue(0.0);
        }
    }
}
