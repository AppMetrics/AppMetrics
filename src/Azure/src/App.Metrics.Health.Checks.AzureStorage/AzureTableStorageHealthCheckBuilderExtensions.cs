// <copyright file="AzureTableStorageHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
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
    public static class AzureTableStorageHealthCheckBuilderExtensions
    {
        private static readonly ILog Logger = LogProvider.For<IRunHealthChecks>();

        public static IHealthBuilder AddAzureTableStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckTableStorageConnectivity(name, storageAccount),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureTableStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckTableStorageConnectivity(name, CloudStorageAccount.Parse(connectionString)),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureTableStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount)
        {
            builder.AddCheck(name, CheckTableStorageConnectivity(name, storageAccount));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureTableStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString)
        {
            builder.AddCheck(name, CheckTableStorageConnectivity(name, CloudStorageAccount.Parse(connectionString)));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureTableStorageTableCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            string tableName,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckAzureTableStorageTableExists(name, storageAccount, tableName),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureTableStorageTableCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string tableName,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckAzureTableStorageTableExists(name, CloudStorageAccount.Parse(connectionString), tableName),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureTableStorageTableCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            string tableName)
        {
            builder.AddCheck(name, CheckAzureTableStorageTableExists(name, storageAccount, tableName));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureTableStorageTableCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string tableName)
        {
            builder.AddCheck(name, CheckAzureTableStorageTableExists(name, CloudStorageAccount.Parse(connectionString), tableName));

            return builder.Builder;
        }

        private static Func<ValueTask<HealthCheckResult>> CheckAzureTableStorageTableExists(
            string name,
            CloudStorageAccount storageAccount,
            string tableName)
        {
            return async () =>
            {
                bool result;

                try
                {
                    var tableStorageClient = storageAccount.CreateCloudTableClient();

                    var tableReference = tableStorageClient.GetTableReference(tableName);

                    result = await tableReference.ExistsAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException($"{name} failed.", ex);

                    result = false;
                }

                return result
                    ? HealthCheckResult.Healthy($"OK. '{tableName}' is available.")
                    : HealthCheckResult.Unhealthy($"Failed. '{tableName}' is unavailable.");
            };
        }

        private static Func<ValueTask<HealthCheckResult>> CheckTableStorageConnectivity(string name, CloudStorageAccount storageAccount)
        {
            return async () =>
            {
                var result = true;

                try
                {
                    var tableStorageClient = storageAccount.CreateCloudTableClient();

                    await tableStorageClient.GetServicePropertiesAsync().ConfigureAwait(false);
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