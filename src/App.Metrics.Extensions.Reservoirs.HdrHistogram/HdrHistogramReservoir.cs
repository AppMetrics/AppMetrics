// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Concurrency;
using App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram;
using App.Metrics.ReservoirSampling;

// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Extensions.Reservoirs.HdrHistogram
{
    /// <summary>
    ///     A Histogram that supports recording and analyzing sampled data value counts across a configurable integer value
    ///     range with configurable value precision within the range. Value precision is expressed as the number of significant
    ///     digits in the value recording, and provides control over value quantization behavior across the value range and the
    ///     subsequent value resolution at any given level.
    /// </summary>
    /// <remarks>
    ///     Based on the
    ///     <see href="https://bitbucket.org/marshallpierce/hdrhistogram-metrics-reservoir/src/83a8ec568a1e?at=master">
    ///         java version from Marshall Pierce
    ///     </see>
    /// </remarks>
    /// <seealso cref="IReservoir" />
    public sealed class HdrHistogramReservoir : IReservoir
    {
        private readonly object _maxValueLock = new object();
        private readonly object _minValueLock = new object();
        private readonly Recorder _recorder;

        private readonly HdrHistogram.HdrHistogram _runningTotals;
        private HdrHistogram.HdrHistogram _intervalHistogram;
        private string _maxUserValue;

        private AtomicLong _maxValue = new AtomicLong(0);
        private string _minUserValue;

        private AtomicLong _minValue = new AtomicLong(long.MaxValue);

        /// <summary>
        ///     Initializes a new instance of the <see cref="HdrHistogramReservoir" /> class.
        /// </summary>
        public HdrHistogramReservoir()
            : this(new Recorder(2)) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HdrHistogramReservoir" /> class.
        /// </summary>
        /// <param name="recorder">
        ///     Records integer values, and provides stable interval histogram samples from
        ///     live recorded data without interrupting or stalling active recording of values
        /// </param>
        public HdrHistogramReservoir(Recorder recorder)
        {
            _recorder = recorder;

            _intervalHistogram = recorder.GetIntervalHistogram();
            _runningTotals = new HdrHistogram.HdrHistogram(_intervalHistogram.NumberOfSignificantValueDigits);
        }

        /// <inheritdoc cref="IReservoir" />
        public IReservoirSnapshot GetSnapshot(bool resetReservoir)
        {
            var snapshot = new HdrSnapshot(
                UpdateTotals(),
                _minValue.GetValue(),
                _minUserValue,
                _maxValue.GetValue(),
                _maxUserValue);

            if (resetReservoir)
            {
                Reset();
            }

            return snapshot;
        }

        /// <inheritdoc />
        public IReservoirSnapshot GetSnapshot() { return GetSnapshot(false); }

        /// <inheritdoc cref="IReservoir" />
        public void Reset()
        {
            _recorder.Reset();
            _runningTotals.reset();
            _intervalHistogram.reset();
        }

        /// <inheritdoc cref="IReservoir" />
        public void Update(long value, string userValue)
        {
            _recorder.RecordValue(value);
            if (userValue != null)
            {
                TrackMinMaxUserValue(value, userValue);
            }
        }

        /// <inheritdoc />
        public void Update(long value) { Update(value, null); }

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

        private HdrHistogram.HdrHistogram UpdateTotals()
        {
            lock (_runningTotals)
            {
                _intervalHistogram = _recorder.GetIntervalHistogram(_intervalHistogram);
                _runningTotals.add(_intervalHistogram);
                return _runningTotals.copy() as HdrHistogram.HdrHistogram;
            }
        }
    }
}