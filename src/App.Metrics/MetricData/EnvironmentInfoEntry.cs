// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


namespace App.Metrics.MetricData
{
    public struct EnvironmentInfoEntry
    {
        public readonly string Name;
        public readonly string Value;

        public EnvironmentInfoEntry(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}