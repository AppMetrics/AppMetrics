using System;
using System.Collections.Generic;
using System.Threading;
using App.Metrics.App_Packages.Concurrency;
using App.Metrics.Utils;

namespace App.Metrics.Sampling
{
    public sealed class ExponentiallyDecayingReservoir : Reservoir, IDisposable
    {
        private const double DefaultAlpha = 0.015;
        private const int DefaultSize = 1028;
        private static readonly TimeSpan RescaleInterval = TimeSpan.FromHours(1);

        private readonly double alpha;

        private readonly Clock clock;

        private readonly Scheduler rescaleScheduler;
        private readonly int size;

        private readonly SortedList<double, WeightedSample> values;
        private AtomicLong count = new AtomicLong();

        private SpinLock @lock = new SpinLock();
        private AtomicLong startTime;

        public ExponentiallyDecayingReservoir()
            : this(DefaultSize, DefaultAlpha)
        {
        }

        public ExponentiallyDecayingReservoir(int size, double alpha)
            : this(size, alpha, Clock.Default, new ActionScheduler())
        {
        }

        public ExponentiallyDecayingReservoir(Clock clock, Scheduler scheduler)
            : this(DefaultSize, DefaultAlpha, clock, scheduler)
        {
        }

        public ExponentiallyDecayingReservoir(int size, double alpha, Clock clock, Scheduler scheduler)
        {
            this.size = size;
            this.alpha = alpha;
            this.clock = clock;

            this.values = new SortedList<double, WeightedSample>(size, ReverseOrderDoubleComparer.Instance);

            this.rescaleScheduler = scheduler;
            this.rescaleScheduler.Start(RescaleInterval, () => Rescale());

            this.startTime = new AtomicLong(clock.Seconds);
        }

        public int Size
        {
            get { return Math.Min(this.size, (int)this.count.GetValue()); }
        }

        public void Dispose()
        {
            using (this.rescaleScheduler)
            {
            }
        }

        public Snapshot GetSnapshot(bool resetReservoir = false)
        {
            var lockTaken = false;
            try
            {
                this.@lock.Enter(ref lockTaken);
                var snapshot = new WeightedSnapshot(this.count.GetValue(), this.values.Values);
                if (resetReservoir)
                {
                    ResetReservoir();
                }
                return snapshot;
            }
            finally
            {
                if (lockTaken)
                {
                    this.@lock.Exit();
                }
            }
        }

        public void Reset()
        {
            var lockTaken = false;
            try
            {
                this.@lock.Enter(ref lockTaken);
                ResetReservoir();
            }
            finally
            {
                if (lockTaken)
                {
                    this.@lock.Exit();
                }
            }
        }

        public void Update(long value, string userValue = null)
        {
            Update(value, userValue, this.clock.Seconds);
        }

        ///* "A common feature of the above techniques—indeed, the key technique that
        // * allows us to track the decayed weights efficiently—is that they maintain
        // * counts and other quantities based on g(ti − L), and only scale by g(t − L)
        // * at query time. But while g(ti −L)/g(t−L) is guaranteed to lie between zero
        // * and one, the intermediate values of g(ti − L) could become very large. For
        // * polynomial functions, these values should not grow too large, and should be
        // * effectively represented in practice by floating point values without loss of
        // * precision. For exponential functions, these values could grow quite large as
        // * new values of (ti − L) become large, and potentially exceed the capacity of
        // * common floating point types. However, since the values stored by the
        // * algorithms are linear combinations of g values (scaled sums), they can be
        // * rescaled relative to a new landmark. That is, by the analysis of exponential
        // * decay in Section III-A, the choice of L does not affect the final result. We
        // * can therefore multiply each value based on L by a factor of exp(−α(L′ − L)),
        // * and obtain the correct value as if we had instead computed relative to a new
        // * landmark L′ (and then use this new L′ at query time). This can be done with
        // * a linear pass over whatever data structure is being used."
        // */
        private void Rescale()
        {
            var lockTaken = false;
            try
            {
                this.@lock.Enter(ref lockTaken);
                var oldStartTime = this.startTime.GetValue();
                this.startTime.SetValue(this.clock.Seconds);

                var scalingFactor = Math.Exp(-this.alpha * (this.startTime.GetValue() - oldStartTime));

                var keys = new List<double>(this.values.Keys);
                foreach (var key in keys)
                {
                    var sample = this.values[key];
                    this.values.Remove(key);
                    var newKey = key * Math.Exp(-this.alpha * (this.startTime.GetValue() - oldStartTime));
                    var newSample = new WeightedSample(sample.Value, sample.UserValue, sample.Weight * scalingFactor);
                    this.values[newKey] = newSample;
                }
                // make sure the counter is in sync with the number of stored samples.
                this.count.SetValue(this.values.Count);
            }
            finally
            {
                if (lockTaken)
                {
                    this.@lock.Exit();
                }
            }
        }

        private void ResetReservoir()
        {
            this.values.Clear();
            this.count.SetValue(0L);
            this.startTime.SetValue(this.clock.Seconds);
        }

        private void Update(long value, string userValue, long timestamp)
        {
            var lockTaken = false;
            try
            {
                this.@lock.Enter(ref lockTaken);

                var itemWeight = Math.Exp(this.alpha * (timestamp - this.startTime.GetValue()));
                var sample = new WeightedSample(value, userValue, itemWeight);

                var random = 0.0;
                // Prevent division by 0
                while (random.Equals(.0))
                {
                    random = ThreadLocalRandom.NextDouble();
                }

                var priority = itemWeight / random;

                var newCount = this.count.GetValue();
                newCount++;
                this.count.SetValue(newCount);

                if (newCount <= this.size)
                {
                    this.values[priority] = sample;
                }
                else
                {
                    var first = this.values.Keys[this.values.Count - 1];
                    if (first < priority)
                    {
                        this.values.Remove(first);
                        this.values[priority] = sample;
                    }
                }
            }
            finally
            {
                if (lockTaken)
                {
                    this.@lock.Exit();
                }
            }
        }

        private class ReverseOrderDoubleComparer : IComparer<double>
        {
            public static readonly IComparer<double> Instance = new ReverseOrderDoubleComparer();

            public int Compare(double x, double y)
            {
                return y.CompareTo(x);
            }
        }
    }
}