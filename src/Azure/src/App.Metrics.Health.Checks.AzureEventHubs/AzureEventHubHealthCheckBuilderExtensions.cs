// <copyright file="AzureEventHubHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Health.Logging;
using Microsoft.Azure.EventHubs;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class AzureEventHubHealthCheckBuilderExtensions
    {
        private static readonly ILog Logger = LogProvider.For<IRunHealthChecks>();

        public static IHealthBuilder AddAzureEventHubConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            EventHubClient eventHubClient,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(
                name,
                CheckEventHubConnectivity(name, eventHubClient),
                cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureEventHubConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string eventHubName)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(connectionString)
                                          {
                                              EntityPath = eventHubName
                                          };

            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            builder.AddCheck(name, CheckEventHubConnectivity(name, eventHubClient));

            return builder.Builder;
        }

        private static Func<ValueTask<HealthCheckResult>> CheckEventHubConnectivity(string name, EventHubClient eventHubClient)
        {
            return async () =>
            {
                var result = true;

                try
                {
                    await eventHubClient.GetRuntimeInformationAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException($"{name} failed.", ex);

                    result = false;
                }

                return result
                    ? HealthCheckResult.Healthy($"OK. '{eventHubClient.EventHubName}' is available.")
                    : HealthCheckResult.Unhealthy($"Failed. '{eventHubClient.EventHubName}' is unavailable.");
            };
        }
    }
}