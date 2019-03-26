// <copyright file="AzureBlobStorageHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
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
    public static class AzureBlobStorageHealthCheckBuilderExtensions
    {
        private static readonly ILog Logger = LogProvider.For<IRunHealthChecks>();

        public static IHealthBuilder AddAzureBlobStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckBlobStorageConnectivity(name, storageAccount),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureBlobStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckBlobStorageConnectivity(name, CloudStorageAccount.Parse(connectionString)),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureBlobStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount)
        {
            builder.AddCheck(name, CheckBlobStorageConnectivity(name, storageAccount));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureBlobStorageConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString)
        {
            builder.AddCheck(name, CheckBlobStorageConnectivity(name, CloudStorageAccount.Parse(connectionString)));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureBlobStorageContainerCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            string containerName,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckAzureBlobStorageContainerExists(name, storageAccount, containerName),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureBlobStorageContainerCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string containerName,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckAzureBlobStorageContainerExists(name, CloudStorageAccount.Parse(connectionString), containerName),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureBlobStorageContainerCheck(
            this IHealthCheckBuilder builder,
            string name,
            CloudStorageAccount storageAccount,
            string containerName)
        {
            builder.AddCheck(name, CheckAzureBlobStorageContainerExists(name, storageAccount, containerName));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureBlobStorageContainerCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string containerName)
        {
            builder.AddCheck(name, CheckAzureBlobStorageContainerExists(name, CloudStorageAccount.Parse(connectionString), containerName));

            return builder.Builder;
        }

        private static Func<ValueTask<HealthCheckResult>> CheckAzureBlobStorageContainerExists(
            string name,
            CloudStorageAccount storageAccount,
            string containerName)
        {
            return async () =>
            {
                bool result;

                try
                {
                    var queueClient = storageAccount.CreateCloudBlobClient();

                    var blobContainer = queueClient.GetContainerReference(containerName);

                    result = await blobContainer.ExistsAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException($"{name} failed.", ex);

                    result = false;
                }

                return result
                    ? HealthCheckResult.Healthy($"OK. '{containerName}' is available.")
                    : HealthCheckResult.Unhealthy($"Failed. '{containerName}' is unavailable.");
            };
        }

        private static Func<ValueTask<HealthCheckResult>> CheckBlobStorageConnectivity(string name, CloudStorageAccount storageAccount)
        {
            return async () =>
            {
                var result = true;

                try
                {
                    var blobStorageClient = storageAccount.CreateCloudBlobClient();

                    await blobStorageClient.GetServicePropertiesAsync().ConfigureAwait(false);
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