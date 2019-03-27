// <copyright file="HealthCheckCached.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Concurrency;

namespace App.Metrics.Health
{
    /// <summary>
    ///     Cached Health Check - Allows caching of Health Check results given a duration to cache
    /// </summary>
    public partial class HealthCheck
    {
        private readonly TimeSpan _cacheDuration = TimeSpan.Zero;
        private Result _cachedResult;
        private AtomicLong _reCheckAt = new AtomicLong(0);

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        /// <param name="name">A descriptive name for the health check.</param>
        /// <param name="check">A function returning either a healthy or un-healthy result.</param>
        /// <param name="cacheDuration">The duration of which to cache the health check result.</param>
        public HealthCheck(
            string name,
            Func<ValueTask<HealthCheckResult>> check,
            TimeSpan cacheDuration)
        {
            EnsureValidCacheDuration(cacheDuration);

            Name = name;

            ValueTask<HealthCheckResult> CheckWithToken(CancellationToken token) => check();

            _check = CheckWithToken;
            _cacheDuration = cacheDuration;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        /// <param name="name">A descriptive name for the health check.</param>
        /// <param name="check">A function returning either a healthy or un-healthy result.</param>
        /// <param name="cacheDuration">The duration of which to cache the health check result.</param>
        public HealthCheck(
            string name,
            Func<CancellationToken, ValueTask<HealthCheckResult>> check,
            TimeSpan cacheDuration)
        {
            EnsureValidCacheDuration(cacheDuration);

            Name = name;

            ValueTask<HealthCheckResult> CheckWithToken(CancellationToken token) => check(token);

            _check = CheckWithToken;
            _cacheDuration = cacheDuration;
        }

        protected HealthCheck(string name, TimeSpan cacheDuration)
        {
            EnsureValidCacheDuration(cacheDuration);

            Name = name;
            _cacheDuration = cacheDuration;
            _check = token => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
        }

        private static void EnsureValidCacheDuration(TimeSpan cacheDuration)
        {
            if (cacheDuration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be greater than zero", nameof(cacheDuration));
            }
        }

        private async Task<Result> ExecuteWithCachingAsync(CancellationToken cancellationToken)
        {
            if (_reCheckAt.GetValue() >= DateTime.UtcNow.Ticks)
            {
                return _cachedResult;
            }

            var checkResult = await CheckAsync(cancellationToken);
            _cachedResult = new Result(Name, checkResult, true);

            _reCheckAt.SetValue(DateTime.UtcNow.Ticks + _cacheDuration.Ticks);

            return new Result(Name, checkResult);
        }

        private bool HasCacheDuration() { return _cacheDuration > TimeSpan.Zero; }
    }
}