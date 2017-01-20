#region copyright

// // Copyright (c) Allan Hardy. All rights reserved.
// // Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#endregion

using System;
using System.Collections.Generic;
using App.Metrics.Sampling;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Facts
{
    public class CustomReservoir : IReservoir, IDisposable
    {
        private readonly List<long> _values = new List<long>();

        public long Count => _values.Count;

        public int Size => _values.Count;

        public IEnumerable<long> Values => _values;

        public void Dispose() { Dispose(true); }

        public ISnapshot GetSnapshot(bool resetReservoir) { return new UniformSnapshot(_values.Count, _values); }

        public ISnapshot GetSnapshot() { return new UniformSnapshot(_values.Count, _values);}

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