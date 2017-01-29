// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Infrastructure
{
    public struct EnvironmentInfoEntry
    {
        public string Name { get; }

        public string Value { get; }

        public EnvironmentInfoEntry(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public static bool operator ==(EnvironmentInfoEntry left, EnvironmentInfoEntry right) { return left.Equals(right); }

        public static bool operator !=(EnvironmentInfoEntry left, EnvironmentInfoEntry right) { return !left.Equals(right); }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is EnvironmentInfoEntry && Equals((EnvironmentInfoEntry)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0) * 397) ^ (Value?.GetHashCode() ?? 0);
            }
        }

        public bool Equals(EnvironmentInfoEntry other) { return string.Equals(Name, other.Name) && string.Equals(Value, other.Value); }
    }
}