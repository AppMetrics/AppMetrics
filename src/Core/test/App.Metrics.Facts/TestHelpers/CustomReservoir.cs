// <copyright file="CustomReservoir.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.Uniform;

namespace App.Metrics.Facts.TestHelpers
{
    public class CustomReservoir : IReservoir, IDisposable
    {
        private readonly List<long> _values = new List<long>();

        public long Count => _values.Count;

        public int Size => _values.Count;

        public IEnumerable<long> Values => _values;

        public void Dispose() { Dispose(true); }

        public IReservoirSnapshot GetSnapshot(bool resetReservoir) { return new UniformSnapshot(_values.Count, _values.Sum(), _values); }

        public IReservoirSnapshot GetSnapshot() { return new UniformSnapshot(_values.Count, _values.Sum(), _values); }

        public void Reset() { _values.Clear(); }

        public void Update(long value, string userValue) { _values.Add(value); }

        public void Update(long value) { _values.Add(value); }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}