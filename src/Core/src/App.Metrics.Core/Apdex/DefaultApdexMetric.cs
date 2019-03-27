// <copyright file="DefaultApdexMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Apdex
{
    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Apdex">Apdex</see> Metric allows us to measure an apdex score which is a
    ///     ratio of the number of satisfied and tolerating requests to the total requests made. Each satisfied request counts
    ///     as one request, while each tolerating request counts as half a satisfied request.
    /// </summary>
    /// <seealso>
    ///     <cref>App.Metrics.Facts.Metrics.IApdexMetric</cref>
    /// </seealso>
    public sealed class DefaultApdexMetric : IApdexMetric
    {
        private readonly bool _allowWarmup;
        private readonly IApdexProvider _apdexProvider;
        private readonly IClock _clock;
        private bool _disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultApdexMetric" /> class.
        /// </summary>
        /// <param name="reservoir">The reservoir to user for sampling.</param>
        /// <param name="apdexTSeconds">The apdex t seconds value between 0 and 1.</param>
        /// <param name="clock">The clock to use to measure processing duration.</param>
        /// <param name="allowWarmup">
        ///     if set to <c>true</c> allows the service to warmup before starting to calculate the apdex,
        ///     the score will intitially be 1 until enough samples have been recorded.
        /// </param>
        public DefaultApdexMetric(IReservoir reservoir, double apdexTSeconds, IClock clock, bool allowWarmup)
            : this(new ApdexProvider(reservoir, apdexTSeconds), clock, allowWarmup)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultApdexMetric" /> class.
        /// </summary>
        /// <param name="reservoir">The reservoir to user for sampling.</param>
        /// <param name="clock">The clock to use to measure processing duration.</param>
        /// <param name="allowWarmup">
        ///     if set to <c>true</c> allows the service to warmup before starting to calculate the apdex,
        ///     the score will intitially be 1 until enough samples have been recorded.
        /// </param>
        public DefaultApdexMetric(IReservoir reservoir, IClock clock, bool allowWarmup)
            // ReSharper disable RedundantArgumentDefaultValue
            : this(new ApdexProvider(reservoir, AppMetricsReservoirSamplingConstants.DefaultApdexTSeconds), clock, allowWarmup)
        // ReSharper restore RedundantArgumentDefaultValue
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultApdexMetric" /> class.
        /// </summary>
        /// <param name="apdexProvider">The apdexProvider implementation to use for sampling values to generate the apdex score.</param>
        /// <param name="clock">The clock to use to measure processing duration.</param>
        /// <param name="allowWarmup">
        ///     if set to <c>true</c> allows the service to warmup before starting to calculate the apdex,
        ///     the score will intitially be 1 until enough samples have been recorded.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     clock and apdexProvider are required.
        /// </exception>
        public DefaultApdexMetric(IApdexProvider apdexProvider, IClock clock, bool allowWarmup)
        {
            _apdexProvider = apdexProvider ?? throw new ArgumentNullException(nameof(apdexProvider));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _allowWarmup = allowWarmup;
        }

        /// <inheritdoc />
        public ApdexValue Value => GetValue();

        /// <inheritdoc />
        public long CurrentTime()
        {
            return _clock.Nanoseconds;
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
        }

        /// <inheritdoc />
        public long EndRecording()
        {
            return _clock.Nanoseconds;
        }

        /// <inheritdoc />
        public ApdexValue GetValue(bool resetMetric = false)
        {
            var snapshot = _apdexProvider.GetSnapshot(resetMetric);

            var totalSamples = snapshot.SatisfiedSize + snapshot.ToleratingSize + snapshot.FrustratingSize;

            // ReSharper disable ArrangeRedundantParentheses
            var apdex = (snapshot.SatisfiedSize + ((double)snapshot.ToleratingSize / 2)) / totalSamples;

            // ReSharper restore ArrangeRedundantParentheses
            if (resetMetric)
            {
                Reset();
            }

            return new ApdexValue(apdex, snapshot.SatisfiedSize, snapshot.ToleratingSize, snapshot.FrustratingSize, totalSamples, _allowWarmup);
        }

        /// <inheritdoc />
        public ApdexContext NewContext()
        {
            return new ApdexContext(this);
        }

        /// <inheritdoc />
        public void Reset()
        {
            _apdexProvider.Reset();
        }

        /// <inheritdoc />
        public long StartRecording()
        {
            return _clock.Nanoseconds;
        }

        /// <inheritdoc />
        public void Track(long duration)
        {
            if (duration < 0)
            {
                return;
            }

            _apdexProvider.Update(duration);
        }

        /// <inheritdoc />
        public void Track(Action action)
        {
            var start = _clock.Nanoseconds;
            try
            {
                action();
            }
            finally
            {
                Track(_clock.Nanoseconds - start);
            }
        }

        // <inheritdoc />
        public T Track<T>(Func<T> action)
        {
            var start = _clock.Nanoseconds;
            try
            {
                return action();
            }
            finally
            {
                Track(_clock.Nanoseconds - start);
            }
        }

        [ExcludeFromCodeCoverage]
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                }
            }

            _disposed = true;
        }
    }
}