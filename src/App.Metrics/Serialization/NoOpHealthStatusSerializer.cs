// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.Serialization;
using App.Metrics.Core.Internal;

namespace App.Metrics.Serialization
{
    [AppMetricsExcludeFromCodeCoverage]
    public sealed class NoOpHealthStatusSerializer : IHealthStatusSerializer
    {
        public T Deserialize<T>(string value) { return default(T); }

        public string Serialize<T>(T value) { return string.Empty; }
    }
}