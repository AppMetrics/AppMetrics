// Ported to .NET Standard Library by Allan Hardy
// Original repo: https://github.com/etishor/Metrics.NET

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

        private readonly double _alpha;

        private readonly Clock _clock;

        private readonly Scheduler _rescaleScheduler;
        private readonly int _size;

        private readonly SortedList<double, WeightedSample> _values;
        private AtomicLong _count = new AtomicLong();

        private SpinLock _lock = new SpinLock();
        private AtomicLong _startTime;

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
            _size = size;
            _alpha = alpha;
            _clock = clock;

            _values = new SortedList<double, WeightedSample>(size, ReverseOrderDoubleComparer.Instance);

            _rescaleScheduler = scheduler;
            _rescaleScheduler.Start(RescaleInterval, () => Rescale());

            _startTime = new AtomicLong(clock.Seconds);
        }

        public int Size
        {
            get { return Math.Min(_size, (int)_count.GetValue()); }
        }

        public void Dispose()
        {
            using (_rescaleScheduler)
            {
            }
        }

        public Snapshot GetSnapshot(bool resetReservoir = false)
        {
            var lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);
                var snapshot = new WeightedSnapshot(_count.GetValue(), _values.Values);
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
                    _lock.Exit();
                }
            }
        }

        public void Reset()
        {
            var lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);
                ResetReservoir();
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                }
            }
        }

        public void Update(long value, string userValue = null)
        {
            Update(value, userValue, _clock.Seconds);
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
                _lock.Enter(ref lockTaken);
                var oldStartTime = _startTime.GetValue();
                _startTime.SetValue(_clock.Seconds);

                var scalingFactor = Math.Exp(-_alpha * (_startTime.GetValue() - oldStartTime));

                var keys = new List<double>(_values.Keys);
                foreach (var key in keys)
                {
                    var sample = _values[key];
                    _values.Remove(key);
                    var newKey = key * Math.Exp(-_alpha * (_startTime.GetValue() - oldStartTime));
                    var newSample = new WeightedSample(sample.Value, sample.UserValue, sample.Weight * scalingFactor);
                    _values[newKey] = newSample;
                }
                // make sure the counter is in sync with the number of stored samples.
                _count.SetValue(_values.Count);
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
                }
            }
        }

        private void ResetReservoir()
        {
            _values.Clear();
            _count.SetValue(0L);
            _startTime.SetValue(_clock.Seconds);
        }

        private void Update(long value, string userValue, long timestamp)
        {
            var lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);

                var itemWeight = Math.Exp(_alpha * (timestamp - _startTime.GetValue()));
                var sample = new WeightedSample(value, userValue, itemWeight);

                var random = 0.0;
                // Prevent division by 0
                while (random.Equals(.0))
                {
                    random = ThreadLocalRandom.NextDouble();
                }

                var priority = itemWeight / random;

                var newCount = _count.GetValue();
                newCount++;
                _count.SetValue(newCount);

                if (newCount <= _size)
                {
                    _values[priority] = sample;
                }
                else
                {
                    var first = _values.Keys[_values.Count - 1];
                    if (first < priority)
                    {
                        _values.Remove(first);
                        _values[priority] = sample;
                    }
                }
            }
            finally
            {
                if (lockTaken)
                {
                    _lock.Exit();
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