// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Abstractions.Serialization
{
    public interface IMetricDataSerializer
    {
        T Deserialize<T>(string value);

        string Serialize<T>(T value);
    }
}