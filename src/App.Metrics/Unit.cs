// <copyright file="Unit.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Globalization;

namespace App.Metrics
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public struct Unit
    {
        public static readonly Unit Bytes = new Unit("bytes");
        public static readonly Unit Calls = new Unit("Calls");
        public static readonly Unit Commands = new Unit("Commands");
        public static readonly Unit Errors = new Unit("Errors");
        public static readonly Unit Events = new Unit("Events");
        public static readonly Unit Items = new Unit("Items");
        public static readonly Unit KiloBytes = new Unit("Kb");
        public static readonly Unit MegaBytes = new Unit("Mb");
        public static readonly Unit None = new Unit(string.Empty);
        public static readonly Unit Percent = new Unit("%");
        public static readonly Unit Requests = new Unit("Requests");
        public static readonly Unit Results = new Unit("Results");
        public static readonly Unit Threads = new Unit("Threads");

        private Unit(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public static Unit Custom(string name) { return new Unit(name); }

        public static bool operator ==(Unit left, Unit right) { return left.Equals(right); }

        public static implicit operator Unit(string name) { return Custom(name); }

        public static bool operator !=(Unit left, Unit right) { return !left.Equals(right); }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Unit && Equals((Unit)obj);
        }

        public override int GetHashCode() { return Name?.GetHashCode() ?? 0; }

        public override string ToString() { return Name; }

        // ReSharper disable MemberCanBePrivate.Global
        public bool Equals(Unit other) { return string.Equals(Name, other.Name); }
        // ReSharper restore MemberCanBePrivate.Global

        public string FormatCount(long value)
        {
            return !string.IsNullOrEmpty(Name)
                ? $"{value.ToString(CultureInfo.InvariantCulture)} {Name}"
                : value.ToString();
        }

        public string FormatDuration(double value, TimeUnit? timeUnit)
        {
            return $"{value.ToString("F2", CultureInfo.InvariantCulture)} {(timeUnit.HasValue ? timeUnit.Value.Unit() : Name)}";
        }

        public string FormatRate(double value, TimeUnit timeUnit)
        {
            return $"{value.ToString("F2", CultureInfo.InvariantCulture)} {Name}/{timeUnit.Unit()}";
        }

        public string FormatValue(double value)
        {
            return !string.IsNullOrEmpty(Name)
                ? $"{value.ToString("F2", CultureInfo.InvariantCulture)} {Name}"
                : value.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}