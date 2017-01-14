// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Data
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