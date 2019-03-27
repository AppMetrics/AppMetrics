// <copyright file="HealthCheckQuiteTime.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health
{
    /// <summary>
    ///     Quite Time Health Check - Ignores health check during "quite time"
    /// </summary>
    public partial class HealthCheck
    {
        private QuiteTime _quiteTime;

        public HealthCheck(
            string name,
            Func<ValueTask<HealthCheckResult>> check,
            QuiteTime quiteTime)
        {
            Name = name;

            ValueTask<HealthCheckResult> CheckWithToken(CancellationToken token) => check();

            _check = CheckWithToken;
            _quiteTime = quiteTime;
        }

        public HealthCheck(
            string name,
            Func<CancellationToken, ValueTask<HealthCheckResult>> check,
            QuiteTime quiteTime)
        {
            Name = name;

            ValueTask<HealthCheckResult> CheckWithToken(CancellationToken token) => check(token);

            _check = CheckWithToken;
            _quiteTime = quiteTime;
        }

        protected HealthCheck(string name, QuiteTime quiteTime)
        {
            Name = name;
            _quiteTime = quiteTime;
            _check = token => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
        }

        private async Task<Result> ExecuteWithQuiteTimeAsync(CancellationToken cancellationToken)
        {
            if (!_quiteTime.ShouldCheck && _quiteTime.UtcNowIsQuiteTime())
            {
                return new Result(Name, HealthCheckResult.Ignore($"Quite Time {_quiteTime.From} to {_quiteTime.To}"));
            }

            var checkResult = await CheckAsync(cancellationToken);

            if (checkResult.Status != HealthCheckStatus.Healthy && _quiteTime.UtcNowIsQuiteTime())
            {
                return new Result(Name, HealthCheckResult.Ignore($"[Quite Time {_quiteTime.From} to {_quiteTime.To}] - {checkResult.Message}"));
            }

            return new Result(Name, checkResult);
        }

        private bool HasQuiteTime() { return _quiteTime.From > TimeSpan.Zero && _quiteTime.To > TimeSpan.Zero; }

        public struct QuiteTime
        {
            public readonly TimeSpan From;
            public readonly bool ShouldCheck;
            public readonly TimeSpan To;
            public readonly DayOfWeek[] ExcludeDays;

            /// <summary>
            ///     Initializes a new instance of the <see cref="QuiteTime" /> struct.
            /// </summary>
            /// <param name="from">
            ///     A <see cref="TimeSpan" /> representing the (UTC) from hours and minutes of the day to ignore health
            ///     checks.
            /// </param>
            /// <param name="to">
            ///     A <see cref="TimeSpan" /> representing the (UTC) to hours and minutes of the day to ignore health
            ///     checks.
            /// </param>
            public QuiteTime(TimeSpan from, TimeSpan to)
            {
                EnsureValid(from, to);

                From = from;
                To = to;
                ShouldCheck = true;
#if !NETSTANDARD1_6
                ExcludeDays = new DayOfWeek[0];
#else
                ExcludeDays = Array.Empty<DayOfWeek>();
#endif
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="QuiteTime" /> struct.
            /// </summary>
            /// <param name="from">
            ///     A <see cref="TimeSpan" /> representing the (UTC) from hours and minutes of the day to ignore health
            ///     checks.
            /// </param>
            /// <param name="to">
            ///     A <see cref="TimeSpan" /> representing the (UTC) to hours and minutes of the day to ignore health
            ///     checks.
            /// </param>
            /// <param name="excludeDays">Days to exclude the quite time.</param>
            public QuiteTime(TimeSpan from, TimeSpan to, DayOfWeek[] excludeDays)
            {
                EnsureValid(from, to);

                From = from;
                To = to;
                ShouldCheck = true;
                ExcludeDays = excludeDays;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="QuiteTime" /> struct.
            /// </summary>
            /// <param name="from">
            ///     A <see cref="TimeSpan" /> representing the (UTC) from hours and minutes of the day to ignore health
            ///     checks.
            /// </param>
            /// <param name="to">
            ///     A <see cref="TimeSpan" /> representing the (UTC) to hours and minutes of the day to ignore health
            ///     checks.
            /// </param>
            /// <param name="shouldCheck">If
            ///     <value>True</value>
            ///     , the health check will be executed but ignored when not healthy, otherwise the health check will not be executed.
            /// </param>
            public QuiteTime(TimeSpan from, TimeSpan to, bool shouldCheck)
            {
                EnsureValid(from, to);

                From = from;
                To = to;
                ShouldCheck = shouldCheck;
#if !NETSTANDARD1_6
                ExcludeDays = new DayOfWeek[0];
#else
                ExcludeDays = Array.Empty<DayOfWeek>();
#endif
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="QuiteTime" /> struct.
            /// </summary>
            /// <param name="from">
            ///     A <see cref="TimeSpan" /> representing the (UTC) from hours and minutes of the day to ignore health
            ///     checks.
            /// </param>
            /// <param name="to">
            ///     A <see cref="TimeSpan" /> representing the (UTC) to hours and minutes of the day to ignore health
            ///     checks.
            /// </param>
            /// <param name="shouldCheck">If
            ///     <value>True</value>
            ///     , the health check will be executed but ignored when not healthy, otherwise the health check will not be executed.
            /// </param>
            /// <param name="excludeDays">Days to exclude the quite time.</param>
            public QuiteTime(TimeSpan from, TimeSpan to, bool shouldCheck, DayOfWeek[] excludeDays)
            {
                EnsureValid(from, to);

                From = from;
                To = to;
                ShouldCheck = shouldCheck;
                ExcludeDays = excludeDays;
            }

            public bool UtcNowIsQuiteTime()
            {
                if (ExcludeDays.Contains(DateTime.UtcNow.DayOfWeek))
                {
                    return false;
                }

                var now = DateTime.UtcNow.TimeOfDay;
                return now >= From && now <= To;
            }

            private static void EnsureValid(TimeSpan from, TimeSpan to)
            {
                if (from.Days != 0)
                {
                    throw new ArgumentException("Days must be zero", nameof(From));
                }

                if (to.Days != 0)
                {
                    throw new ArgumentException("Days must be zero", nameof(To));
                }

                if (from <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Must be greater than zero", nameof(From));
                }

                if (to <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Must be greater than zero", nameof(To));
                }

                if (from >= to)
                {
                    throw new ArgumentException($"{from} must be greater than {to}", nameof(QuiteTime));
                }
            }
        }
    }
}