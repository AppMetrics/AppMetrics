// <copyright file="AzureServiceBusQueueHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Health.Checks.AzureServiceBus;
using App.Metrics.Health.Logging;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
// ReSharper restore CheckNamespace
{
    public static class AzureServiceBusQueueHealthCheckBuilderExtensions
    {
        private static readonly ILog Logger = LogProvider.For<IRunHealthChecks>();
        private static readonly Message HealthMessage = new Message(Encoding.UTF8.GetBytes("Queue Health Check"));
        private static readonly DateTimeOffset HealthMessageTestSchedule = new DateTimeOffset(DateTime.UtcNow).AddDays(1);
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

        public static IHealthBuilder AddAzureServiceBusQueueConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            QueueClient queueClient,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(name, CheckServiceBusQueueConnectivity(name, queueClient), cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureServiceBusQueueConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            QueueClient queueClient)
        {
            builder.AddCheck(name, CheckServiceBusQueueConnectivity(name, queueClient));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureServiceBusQueueConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string queueName,
            TimeSpan cacheDuration)
        {
            var queueClient = new QueueClient(connectionString, queueName, receiveMode: ReceiveMode.PeekLock) { OperationTimeout = DefaultTimeout };

            builder.AddCachedCheck(name, CheckServiceBusQueueConnectivity(name, queueClient), cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureServiceBusQueueConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string queueName)
        {
            var queueClient = new QueueClient(connectionString, queueName, receiveMode: ReceiveMode.PeekLock) { OperationTimeout = DefaultTimeout };

            builder.AddCheck(name, CheckServiceBusQueueConnectivity(name, queueClient));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureServiceBusQueueDeadLetterQueueCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string queueName,
            long deadLetterWarningThreshold = 1,
            long? deadLetterErrorThreshold = null)
        {
            if (deadLetterErrorThreshold.HasValue && (deadLetterWarningThreshold > deadLetterErrorThreshold))
            {
                throw new ArgumentException("Error threshold must exceed warning threshold", nameof(deadLetterErrorThreshold));
            }

            var managementClient = new ManagementClient(connectionString);
            builder.AddCheck(
                name,
                ServiceBusHealthChecks.CheckDeadLetterQueueCount(Logger, queueName, name, GetQueueMessageCount, deadLetterWarningThreshold, deadLetterErrorThreshold));
            return builder.Builder;

            async Task<MessageCountDetails> GetQueueMessageCount()
            {
                var info = await managementClient.GetQueueRuntimeInfoAsync(queueName);
                return info.MessageCountDetails;
            }
        }

        public static IHealthBuilder AddAzureServiceBusQueueDeadLetterQueueCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string queueName,
            TimeSpan cacheDuration,
            long deadLetterWarningThreshold = 1,
            long? deadLetterErrorThreshold = null)
        {
            if (deadLetterErrorThreshold.HasValue && (deadLetterWarningThreshold > deadLetterErrorThreshold))
            {
                throw new ArgumentException("Error threshold must exceed warning threshold", nameof(deadLetterErrorThreshold));
            }

            var managementClient = new ManagementClient(connectionString);
            builder.AddCachedCheck(
                name,
                ServiceBusHealthChecks.CheckDeadLetterQueueCount(Logger, queueName, name, GetQueueMessageCount, deadLetterWarningThreshold, deadLetterErrorThreshold),
                cacheDuration);
            return builder.Builder;

            async Task<MessageCountDetails> GetQueueMessageCount()
            {
                var info = await managementClient.GetQueueRuntimeInfoAsync(queueName);
                return info.MessageCountDetails;
            }
        }

        private static Func<ValueTask<HealthCheckResult>> CheckServiceBusQueueConnectivity(string name, QueueClient queueClient)
        {
            return async () =>
            {
                var result = true;

                try
                {
                    var id = await queueClient.ScheduleMessageAsync(HealthMessage, HealthMessageTestSchedule).ConfigureAwait(false);
                    await queueClient.CancelScheduledMessageAsync(id);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException($"{name} failed.", ex);

                    result = false;
                }

                return result
                    ? HealthCheckResult.Healthy($"OK. '{queueClient.Path}/{queueClient.QueueName}' is available.")
                    : HealthCheckResult.Unhealthy($"Failed. '{queueClient.Path}/{queueClient.QueueName}' is unavailable.");
            };
        }
    }
}