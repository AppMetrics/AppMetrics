// <copyright file="SqlHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health.Checks.Sql
    // ReSharper restore CheckNamespace
{
    public static class SqlHealthCheckBuilderExtensions
    {
        public static IHealthBuilder AddSqlCachedCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Func<IDbConnection> newDbConnection,
            TimeSpan timeout,
            TimeSpan cacheDuration,
            bool degradedOnError = false)
        {
            EnsureValidTimeout(timeout);

            healthCheckBuilder.AddCachedCheck(
                name,
                cancellationToken => ExecuteSqlCheckAsync(name, newDbConnection, timeout, degradedOnError, cancellationToken),
                cacheDuration);

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddSqlQuiteTimeCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Func<IDbConnection> newDbConnection,
            TimeSpan timeout,
            HealthCheck.QuiteTime quiteTime,
            bool degradedOnError = false)
        {
            EnsureValidTimeout(timeout);

            healthCheckBuilder.AddQuiteTimeCheck(
                name,
                cancellationToken => ExecuteSqlCheckAsync(name, newDbConnection, timeout, degradedOnError, cancellationToken),
                quiteTime);

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddSqlCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Func<IDbConnection> newDbConnection,
            TimeSpan timeout,
            bool degradedOnError = false)
        {
            EnsureValidTimeout(timeout);

            healthCheckBuilder.AddCheck(
                name,
                cancellationToken => ExecuteSqlCheckAsync(name, newDbConnection, timeout, degradedOnError, cancellationToken));

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddSqlCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            string connectionString,
            TimeSpan timeout,
            bool degradedOnError = false)
        {
            return healthCheckBuilder.AddSqlCheck(name, () => new SqlConnection(connectionString), timeout, degradedOnError);
        }

        public static IHealthBuilder AddSqlCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            string connectionString,
            TimeSpan timeout,
            TimeSpan cacheDuration,
            bool degradedOnError = false)
        {
            return healthCheckBuilder.AddSqlCachedCheck(name, () => new SqlConnection(connectionString), timeout, cacheDuration, degradedOnError);
        }

        public static IHealthBuilder AddSqlCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            string connectionString,
            TimeSpan timeout,
            HealthCheck.QuiteTime quiteTime,
            bool degradedOnError = false)
        {
            return healthCheckBuilder.AddSqlQuiteTimeCheck(name, () => new SqlConnection(connectionString), timeout, quiteTime, degradedOnError);
        }

        private static void EnsureValidTimeout(TimeSpan timeout)
        {
            if (timeout <= TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(timeout)} must be greater than 0");
            }
        }

        private static ValueTask<HealthCheckResult> ExecuteSqlCheckAsync(
            string name,
            Func<IDbConnection> newDbConnection,
            TimeSpan timeout,
            bool degradedOnError,
            CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();

            try
            {
                using (var tokenWithTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    tokenWithTimeout.CancelAfter(timeout);

                    sw.Start();
                    using (var connection = newDbConnection())
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT 1";
                            var commandResult = Convert.ToInt64(command.ExecuteScalar());

                            var result = commandResult == 1
                                ? HealthCheckResult.Healthy($"OK. {name}.")
                                : HealthCheckResultOnError($"FAILED. {name} SELECT failed. Time taken: {sw.ElapsedMilliseconds}ms.", degradedOnError);

                            return new ValueTask<HealthCheckResult>(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var failedResult = degradedOnError
                    ? HealthCheckResult.Degraded(ex)
                    : HealthCheckResult.Unhealthy(ex);

                return new ValueTask<HealthCheckResult>(failedResult);
            }
        }

        /// <summary>
        ///     Create a failure (degraded or unhealthy) status response.
        /// </summary>
        /// <param name="message">Status message.</param>
        /// <param name="degradedOnError">
        ///     If true, create a degraded status response.
        ///     Otherwise create an unhealthy status response. (default: false)
        /// </param>
        /// <returns>Failure status response.</returns>
        private static HealthCheckResult HealthCheckResultOnError(string message, bool degradedOnError)
        {
            return degradedOnError
                ? HealthCheckResult.Degraded(message)
                : HealthCheckResult.Unhealthy(message);
        }
    }
}