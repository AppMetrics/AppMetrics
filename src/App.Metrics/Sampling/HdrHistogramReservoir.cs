// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

using App.Metrics.App_Packages.Concurrency;
using App.Metrics.App_Packages.HdrHistogram;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Sampling
{
    /// <summary>
    ///     Sampling reservoir based on HdrHistogram.
    ///     Based on the java version from Marshall Pierce
    ///     https://bitbucket.org/marshallpierce/hdrhistogram-metrics-reservoir/src/83a8ec568a1e?at=master
    /// </summary>
    public sealed class HdrHistogramReservoir : IReservoir
    {
        private readonly object _maxValueLock = new object();
        private readonly object _minValueLock = new object();
        private readonly Recorder _recorder;

        private readonly HdrHistogram _runningTotals;
        private HdrHistogram _intervalHistogram;
        private string _maxUserValue;

        private AtomicLong _maxValue = new AtomicLong(0);
        private string _minUserValue;

        private AtomicLong _minValue = new AtomicLong(long.MaxValue);

        public HdrHistogramReservoir()
            : this(new Recorder(2))
        {
        }

        internal HdrHistogramReservoir(Recorder recorder)
        {
            _recorder = recorder;

            _intervalHistogram = recorder.GetIntervalHistogram();
            _runningTotals = new HdrHistogram(_intervalHistogram.NumberOfSignificantValueDigits);
        }

        public ISnapshot GetSnapshot(bool resetReservoir = false)
        {
            var snapshot = new HdrSnapshot(UpdateTotals(), _minValue.GetValue(), _minUserValue, _maxValue.GetValue(),
                _maxUserValue);
            if (resetReservoir)
            {
                Reset();
            }
            return snapshot;
        }

        public void Reset()
        {
            _recorder.Reset();
            _runningTotals.reset();
            _intervalHistogram.reset();
        }

        public void Update(long value, string userValue = null)
        {
            _recorder.RecordValue(value);
            if (userValue != null)
            {
                TrackMinMaxUserValue(value, userValue);
            }
        }

        private void SetMaxValue(long value, string userValue)
        {
            long current;
            while (value > (current = _maxValue.GetValue()))
            {
                _maxValue.CompareAndSwap(current, value);
            }

            if (value == current)
            {
                lock (_maxValueLock)
                {
                    if (value == _maxValue.GetValue())
                    {
                        _maxUserValue = userValue;
                    }
                }
            }
        }

        private void SetMinValue(long value, string userValue)
        {
            long current;
            while (value < (current = _minValue.GetValue()))
            {
                _minValue.CompareAndSwap(current, value);
            }

            if (value == current)
            {
                lock (_minValueLock)
                {
                    if (value == _minValue.GetValue())
                    {
                        _minUserValue = userValue;
                    }
                }
            }
        }

        private void TrackMinMaxUserValue(long value, string userValue)
        {
            if (value > _maxValue.NonVolatileGetValue())
            {
                SetMaxValue(value, userValue);
            }

            if (value < _minValue.NonVolatileGetValue())
            {
                SetMinValue(value, userValue);
            }
        }

        private HdrHistogram UpdateTotals()
        {
            lock (_runningTotals)
            {
                _intervalHistogram = _recorder.GetIntervalHistogram(_intervalHistogram);
                _runningTotals.add(_intervalHistogram);
                return _runningTotals.copy() as HdrHistogram;
            }
        }
    }
}