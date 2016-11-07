// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace App.Metrics.Serialization
{
    internal sealed class NullHealthStatusSerializer : IHealthStatusSerializer
    {
        public T Deserialize<T>(string json)
        {
            return default(T);
        }

        public string Serialize<T>(T value)
        {
            return string.Empty;
        }
    }
}