// <copyright file="AzureServiceBusTopicHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
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
    public static class AzureServiceBusTopicHealthCheckBuilderExtensions
    {
        private static readonly ILog Logger = LogProvider.For<IRunHealthChecks>();
        private static readonly Message HealthMessage = new Message(Encoding.UTF8.GetBytes("Topic Health Check"));
        private static readonly DateTimeOffset HealthMessageTestSchedule = new DateTimeOffset(DateTime.UtcNow).AddDays(1);
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

        public static IHealthBuilder AddAzureServiceBusTopicConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            TopicClient topicClient,
            TimeSpan cacheDuration)
        {
            builder.AddCachedCheck(name, CheckServiceBusTopicConnectivity(name, topicClient), cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureServiceBusTopicConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            TopicClient topicClient)
        {
            builder.AddCheck(name, CheckServiceBusTopicConnectivity(name, topicClient));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureServiceBusTopicConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string topicName,
            TimeSpan cacheDuration)
        {
            var topicClient = new TopicClient(connectionString, topicName) { OperationTimeout = DefaultTimeout };

            builder.AddCachedCheck(name, CheckServiceBusTopicConnectivity(name, topicClient), cacheDuration);

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureServiceBusTopicConnectivityCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string topicName)
        {
            var topicClient = new TopicClient(connectionString, topicName) { OperationTimeout = DefaultTimeout };

            builder.AddCheck(name, CheckServiceBusTopicConnectivity(name, topicClient));

            return builder.Builder;
        }

        public static IHealthBuilder AddAzureServiceBusTopicSubscriptionDeadLetterQueueCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string topicName,
            string subscriptionName,
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
                ServiceBusHealthChecks.CheckDeadLetterQueueCount(Logger, EntityNameHelper.FormatSubscriptionPath(topicName, subscriptionName), name, GetQueueMessageCount, deadLetterWarningThreshold, deadLetterErrorThreshold));
            return builder.Builder;

            async Task<MessageCountDetails> GetQueueMessageCount()
            {
                var info = await managementClient.GetSubscriptionRuntimeInfoAsync(topicName, subscriptionName);
                return info.MessageCountDetails;
            }
        }

        public static IHealthBuilder AddAzureServiceBusTopicSubscriptionDeadLetterQueueCheck(
            this IHealthCheckBuilder builder,
            string name,
            string connectionString,
            string topicName,
            string subscriptionName,
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
                ServiceBusHealthChecks.CheckDeadLetterQueueCount(Logger, EntityNameHelper.FormatSubscriptionPath(topicName, subscriptionName), name, GetQueueMessageCount, deadLetterWarningThreshold, deadLetterErrorThreshold),
                cacheDuration);
            return builder.Builder;

            async Task<MessageCountDetails> GetQueueMessageCount()
            {
                var info = await managementClient.GetSubscriptionRuntimeInfoAsync(topicName, subscriptionName);
                return info.MessageCountDetails;
            }
        }

        private static Func<ValueTask<HealthCheckResult>> CheckServiceBusTopicConnectivity(string name, TopicClient topicClient)
        {
            return async () =>
            {
                var result = true;

                try
                {
                    var id = await topicClient.ScheduleMessageAsync(HealthMessage, HealthMessageTestSchedule).ConfigureAwait(false);
                    await topicClient.CancelScheduledMessageAsync(id);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException($"{name} failed.", ex);

                    result = false;
                }

                return result
                    ? HealthCheckResult.Healthy($"OK. '{topicClient.Path}/{topicClient.TopicName}' is available.")
                    : HealthCheckResult.Unhealthy($"Failed. '{topicClient.Path}/{topicClient.TopicName}' is unavailable.");
            };
        }
    }
}