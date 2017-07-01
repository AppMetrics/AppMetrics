// <copyright file="IHealthStatusPayloadBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics
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