// <copyright file="IMetricDataSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Abstractions.Serialization
{
    public interface IMetricDataSerializer
    {
        T Deserialize<T>(string value);

        string Serialize<T>(T value);
    }
}