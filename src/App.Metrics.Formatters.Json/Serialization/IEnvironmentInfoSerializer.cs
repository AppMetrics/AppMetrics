// <copyright file="IEnvironmentInfoSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Formatters.Json.Serialization
{
    public interface IEnvironmentInfoSerializer
    {
        T Deserialize<T>(string value);

        string Serialize<T>(T value);
    }
}