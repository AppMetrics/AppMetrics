// <copyright file="HealthMediaTypeValue.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Health.Formatters
{
    public struct HealthMediaTypeValue
    {
        public HealthMediaTypeValue(string type, string subType, string version, string format)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException(nameof(type));
            }

            if (string.IsNullOrWhiteSpace(subType))
            {
                throw new ArgumentException(nameof(subType));
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException(nameof(version));
            }

            if (string.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentException(nameof(format));
            }

            Type = type;
            SubType = subType;
            Format = format;
            Version = version;
        }

        public string ContentType => $"{Type}/{Format}";

        public string Format { get; }

        public string SubType { get; }

        public string Type { get; }

        public string Version { get; }

        public static bool operator ==(HealthMediaTypeValue left, HealthMediaTypeValue right) { return left.Equals(right); }

        public static bool operator !=(HealthMediaTypeValue left, HealthMediaTypeValue right) { return !left.Equals(right); }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is HealthMediaTypeValue && Equals((HealthMediaTypeValue)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Format.GetHashCode();
                hashCode = (hashCode * 397) ^ Type.GetHashCode();
                hashCode = (hashCode * 397) ^ SubType.GetHashCode();
                hashCode = (hashCode * 397) ^ Version.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString() { return $"{Type}/{SubType}-{Version}+{Format}"; }

        public bool Equals(HealthMediaTypeValue other)
        {
            return string.Equals(Format, other.Format) && string.Equals(Type, other.Type) && string.Equals(SubType, other.SubType) &&
                   string.Equals(Version, other.Version);
        }
    }
}