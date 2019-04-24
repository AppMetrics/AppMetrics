// <copyright file="AzureQueueStorageHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Health.Logging;
using Microsoft.WindowsAzure.Storage;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class AzureQueueStorageHealthCheckBuilderExtensions
    {
        private static readonly ILog Logger = LogProvider.For<IRunHealthChecks>();

        public static IHealthBuilder AddAzureQueueStorageCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            string queueName,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(name, CheckQueueExists(name, storageAccount, queueName), cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string queueName,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(name, CheckQueueExists(name, CloudStorageAccount.Parse(connectionString), queueName), cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            string queueName)
        {
            builder.AddCheck(name, CheckQueueExists(name, storageAccount, queueName));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string queueName)
        {
            builder.AddCheck(name, CheckQueueExists(name, CloudStorageAccount.Parse(connectionString), queueName));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckStorageAccountConnectivity(name, storageAccount),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckStorageAccountConnectivity(name, CloudStorageAccount.Parse(connectionString)),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount)
        {
            builder.AddCheck(
                name,
                CheckStorageAccountConnectivity(name, storageAccount));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString)
        {
            builder.AddCheck(
                name,
                CheckStorageAccountConnectivity(name, CloudStorageAccount.Parse(connectionString)));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageMessageCountCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            string queueName,
            long degradedThreshold = 1,
            long? unhealthyThreshold = null)
        {
            if (unhealthyThreshold.HasValue && unhealthyThreshold < degradedThreshold)
            {
                throw new ArgumentException("Unhealthy threshold must not be less than degraded threshold.", nameof(unhealthyThreshold));
            }

            if (degradedThreshold < 0)
            {
                throw new ArgumentException("must be greater than 0", nameof(degradedThreshold));
            }

            if (unhealthyThreshold < 0)
            {
                throw new ArgumentException("must be greater than 0", nameof(unhealthyThreshold));
            }

            builder.AddCheck(name, CheckMessageCount(name, storageAccount, queueName, degradedThreshold, unhealthyThreshold));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageMessageCountCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string queueName,
            long degradedThreshold = 1,
            long? unhealthyThreshold = null)
        {
            if (unhealthyThreshold.HasValue && unhealthyThreshold < degradedThreshold)
            {
                throw new ArgumentException("Unhealthy threshold must not be less than degraded threshold.", nameof(unhealthyThreshold));
            }

            if (degradedThreshold < 0)
            {
                throw new ArgumentException("must be greater than 0", nameof(degradedThreshold));
            }

            if (unhealthyThreshold < 0)
            {
                throw new ArgumentException("must be greater than 0", nameof(unhealthyThreshold));
            }

            builder.AddCheck(name, CheckMessageCount(name, CloudStorageAccount.Parse(connectionString), queueName, degradedThreshold, unhealthyThreshold));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageMessageCountCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            string queueName,
            TimeSpan cacheDuration,
            long degradedThreshold = 1,
            long? unhealthyThreshold = null)
        {
            if (unhealthyThreshold.HasValue && unhealthyThreshold < degradedThreshold)
            {
                throw new ArgumentException("Unhealthy threshold must not be less than degraded threshold.", nameof(unhealthyThreshold));
            }

            if (degradedThreshold < 0)
            {
                throw new ArgumentException("must be greater than 0", nameof(degradedThreshold));
            }

            if (unhealthyThreshold < 0)
            {
                throw new ArgumentException("must be greater than 0", nameof(unhealthyThreshold));
            }

            builder.AddCachedCheck(name, CheckMessageCount(name, storageAccount, queueName, degradedThreshold, unhealthyThreshold), cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureQueueStorageMessageCountCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string queueName,
            TimeSpan cacheDuration,
            long degradedThreshold = 1,
            long? unhealthyThreshold = null)
        {
            if (unhealthyThreshold.HasValue && unhealthyThreshold < degradedThreshold)
            {
                throw new ArgumentException("Unhealthy threshold must not be less than degraded threshold.", nameof(unhealthyThreshold));
            }

            if (degradedThreshold < 0)
            {
                throw new ArgumentException("must be greater than 0", nameof(degradedThreshold));
            }

            if (unhealthyThreshold < 0)
            {
                throw new ArgumentException("must be greater than 0", nameof(unhealthyThreshold));
            }

            builder.AddCachedCheck(name, CheckMessageCount(name, CloudStorageAccount.Parse(connectionString), queueName, degradedThreshold, unhealthyThreshold), cacheDuration);

            return builder.Builder;
        }

        private static Func<ValueTask<HealthCheckResult>> CheckQueueExists(string name, CloudStorageAccount storageAccount, string queueName)
        {
            var queue = storageAccount
                .CreateCloudQueueClient()
                .GetQueueReference(queueName);

            return async () =>
            {
                bool result;

                try
                {
                    result = await queue.ExistsAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException($"{name} failed.", ex);

                    result = false;
                }

                return result
                    ? HealthCheckResult.Healthy($"OK. '{queueName}' is available.")
                    : HealthCheckResult.Unhealthy($"Failed. '{queueName}' is unavailable.");
            };
        }

        private static Func<ValueTask<HealthCheckResult>> CheckMessageCount(string name, CloudStorageAccount storageAccount, string queueName, long degradedThreshold, long? unhealthyThreshold)
        {
            var queue = storageAccount
                .CreateCloudQueueClient()
                .GetQueueReference(queueName);

            return async () =>
            {
                int? result = null;

                try
                {
                    await queue.FetchAttributesAsync().ConfigureAwait(false);
                    result = queue.ApproximateMessageCount;
                }
                catch (Exception ex)
                {
                    Logger.ErrorException($"{name} failed.", ex);

                    return HealthCheckResult.Unhealthy($"Failed. Unable to check queue '{queueName}'.");
                }

                if (result > 0 && result >= unhealthyThreshold)
                {
                    return HealthCheckResult.Unhealthy($"Unhealthy. '{queueName}' has {result.Value} messages.");
                }

                if (result > 0 && result >= degradedThreshold)
                {
                    return HealthCheckResult.Degraded($"Degraded. '{queueName}' has {result.Value} messages.");
                }

                return HealthCheckResult.Healthy($"OK. '{queueName}' has {result} messages.");
            };
        }

        private static Func<ValueTask<HealthCheckResult>> CheckStorageAccountConnectivity(string name, CloudStorageAccount storageAccount)
        {
            var queueClient = storageAccount.CreateCloudQueueClient();
            return async () =>
            {
                var result = true;

                try
                {
                    await queueClient.GetServicePropertiesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException($"{name} failed.", ex);

                    result = false;
                }

                return result
                    ? HealthCheckResult.Healthy($"OK. '{storageAccount.BlobStorageUri}' is available.")
                    : HealthCheckResult.Unhealthy($"Failed. '{storageAccount.BlobStorageUri}' is unavailable.");
            };
        }
    }
}