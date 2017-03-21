// <copyright file="NoOpMetricDataSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.Serialization;
using App.Metrics.Core.Internal;

namespace App.Metrics.Serialization
{
    [AppMetricsExcludeFromCodeCoverage]
    public sealed class NoOpMetricDataSerializer : IMetricDataSerializer
    {
        public T Deserialize<T>(string value) { return default(T); }

        public string Serialize<T>(T value) { return string.Empty; }
    }
}