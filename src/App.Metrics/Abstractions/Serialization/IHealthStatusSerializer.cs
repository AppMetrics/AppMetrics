// <copyright file="IHealthStatusSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Abstractions.Serialization
{
    public interface IHealthStatusSerializer
    {
        T Deserialize<T>(string value);

        string Serialize<T>(T value);
    }
}