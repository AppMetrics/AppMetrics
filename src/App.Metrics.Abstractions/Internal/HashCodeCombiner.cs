// <copyright file="HashCodeCombiner.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace App.Metrics.Internal
{
    /// <summary>
    /// Copied from Microsoft.Extensions.HashCodeCombiner.Sources - Microsoft.Extensions.Internal.HashCodeCombiner
    /// </summary>
    internal struct HashCodeCombiner
    {
        private long _combinedHash64;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private HashCodeCombiner(long seed)
        {
            _combinedHash64 = seed;
        }

        public int CombinedHash
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _combinedHash64.GetHashCode(); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(HashCodeCombiner self)
        {
            return self.CombinedHash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashCodeCombiner Start()
        {
            return new HashCodeCombiner(0x1505L);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(IEnumerable e)
        {
            if (e == null)
            {
                Add(0);
            }
            else
            {
                var count = 0;
                foreach (object o in e)
                {
                    Add(o);
                    count++;
                }

                Add(count);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(int i)
        {
            _combinedHash64 = ((_combinedHash64 << 5) + _combinedHash64) ^ i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(string s)
        {
            var hashCode = s != null ? s.GetHashCode() : 0;
            Add(hashCode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(object o)
        {
            var hashCode = o != null ? o.GetHashCode() : 0;
            Add(hashCode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add<TValue>(TValue value, IEqualityComparer<TValue> comparer)
        {
            var hashCode = value != null ? comparer.GetHashCode(value) : 0;
            Add(hashCode);
        }
    }
}