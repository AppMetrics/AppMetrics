// <copyright file="ServiceBusHealthChecks.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Health.Logging;
using Microsoft.Azure.ServiceBus.Management;

namespace App.Metrics.Health.Checks.AzureServiceBus
{
    internal static class ServiceBusHealthChecks
    {
        internal static Func<ValueTask<HealthCheckResult>> CheckDeadLetterQueueCount(ILog logger, string entityPath, string name, Func<Task<MessageCountDetails>> getMessageCount, long deadLetterWarningThreshold, long? deadLetterErrorThreshold)
        {
            return async () =>
            {
                var deadLetteredMessages = 0L;
                try
                {
                    deadLetteredMessages = (await getMessageCount()).DeadLetterMessageCount;
                }
                catch (Exception ex)
                {
                    logger.ErrorException($"{name} failed.", ex);

                    return HealthCheckResult.Unhealthy(ex);
                }

                if (deadLetterErrorThreshold.HasValue && deadLetteredMessages >= deadLetterErrorThreshold)
                {
                    return HealthCheckResult.Unhealthy($"Unhealthy. '{entityPath}' has {deadLetteredMessages} dead letter messages.");
                }
                else if (deadLetteredMessages >= deadLetterWarningThreshold)
                {
                    return HealthCheckResult.Degraded($"Degraded. '{entityPath}' has {deadLetteredMessages} dead letter messages.");
                }

                return HealthCheckResult.Healthy($"OK. {entityPath} has {deadLetteredMessages} dead letter messages.");
            };
        }
    }
}
