// <copyright file="IHealthStatusPayloadBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Health;

namespace App.Metrics.Reporting.Abstractions
{
    public interface IHealthStatusPayloadBuilder<out T>
    {
        void Clear();

        void Init();

        void Pack(string name, string message, HealthCheckStatus status);

        T Payload();

        string PayloadFormatted();
    }
}