// Written by Iulian Margarintescu
// 
// Ported to .NET Standard Library by Allan Hardy
// Original repo: https://github.com/etishor/Metrics.NET

using System;
using System.Linq;
using App.Metrics.App_Packages.Concurrency;

namespace App.Metrics.Sampling
{
    public sealed class SlidingWindowReservoir : Reservoir
    {
        private const int DefaultSize = 1028;

        private readonly UserValueWrapper[] _values;
        private AtomicLong count = new AtomicLong();

        public SlidingWindowReservoir()
            : this(DefaultSize)
        {
        }

        public SlidingWindowReservoir(int size)
        {
            _values = new UserValueWrapper[size];
        }

        public Snapshot GetSnapshot(bool resetReservoir = false)
        {
            var size = Math.Min((int)count.GetValue(), _values.Length);
            if (size == 0)
            {
                return new UniformSnapshot(0, Enumerable.Empty<long>());
            }

            var snapshotValues = new UserValueWrapper[size];
            Array.Copy(_values, snapshotValues, size);

            if (resetReservoir)
            {
                Array.Clear(_values, 0, snapshotValues.Length);
                count.SetValue(0L);
            }

            Array.Sort(snapshotValues, UserValueWrapper.Comparer);
            var minValue = snapshotValues[0].UserValue;
            var maxValue = snapshotValues[size - 1].UserValue;
            return new UniformSnapshot(count.GetValue(), snapshotValues.Select(v => v.Value), valuesAreSorted: true, minUserValue: minValue,
                maxUserValue: maxValue);
        }

        public void Reset()
        {
            Array.Clear(_values, 0, _values.Length);
            count.SetValue(0L);
        }

        public void Update(long value, string userValue = null)
        {
            var newCount = count.Increment();
            _values[(int)((newCount - 1) % _values.Length)] = new UserValueWrapper(value, userValue);
        }
    }
}