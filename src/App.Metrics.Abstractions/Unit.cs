// <copyright file="Unit.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Globalization;

namespace App.Metrics
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public struct Unit
    {
        public static readonly Unit Bits = new Unit("b");
        public static readonly Unit Bytes = new Unit("B");
        public static readonly Unit Calls = new Unit("calls");
        public static readonly Unit Commands = new Unit("command");
        public static readonly Unit Connections = new Unit("conn");
        public static readonly Unit Errors = new Unit("err");
        public static readonly Unit Events = new Unit("event");
        public static readonly Unit Items = new Unit("items");
        public static readonly Unit KiloBytes = new Unit("kB");
        public static readonly Unit MegaBytes = new Unit("MB");
        public static readonly Unit GigaBytes = new Unit("GB");
        public static readonly Unit TeraBytes = new Unit("TB");
        public static readonly Unit PetaBytes = new Unit("PB");
        public static readonly Unit None = new Unit("none");
        public static readonly Unit Percent = new Unit("percent");

        /// <summary>
        /// HTTP requests, database queries, etc
        /// </summary>
        public static readonly Unit Requests = new Unit("req");
        public static readonly Unit Results = new Unit("result");
        public static readonly Unit Threads = new Unit("thread");
        public static readonly Unit Warnings = new Unit("warn");

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