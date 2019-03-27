// <copyright file="LineProtocolSyntax.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    internal class LineProtocolSyntax
    {
        private static readonly DateTime Origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static readonly Dictionary<Type, Func<object, string>> Formatters = new Dictionary<Type, Func<object, string>>
                                                                                    {
                                                                                        { typeof(sbyte), FormatInteger },
                                                                                        { typeof(byte), FormatInteger },
                                                                                        { typeof(short), FormatInteger },
                                                                                        { typeof(ushort), FormatInteger },
                                                                                        { typeof(int), FormatInteger },
                                                                                        { typeof(uint), FormatInteger },
                                                                                        { typeof(long), FormatInteger },
                                                                                        { typeof(ulong), FormatInteger },
                                                                                        { typeof(float), FormatFloat },
                                                                                        { typeof(double), FormatFloat },
                                                                                        { typeof(decimal), FormatFloat },
                                                                                        { typeof(bool), FormatBoolean },
                                                                                        { typeof(TimeSpan), FormatTimespan }
                                                                                    };

        public static string EscapeName(string nameOrKey)
        {
            if (nameOrKey == null)
            {
                throw new ArgumentNullException(nameof(nameOrKey));
            }

            return nameOrKey
                .Replace("=", "\\=")
                .Replace(" ", "\\ ")
                .Replace(",", "\\,");
        }

        public static string FormatTimestamp(DateTime utcTimestamp)
        {
            var t = utcTimestamp - Origin;
            return (t.Ticks * 100L).ToString(CultureInfo.InvariantCulture);
        }

        public static string FormatValue(object value)
        {
            var v = value ?? string.Empty;
            Func<object, string> format;

            return Formatters.TryGetValue(v.GetType(), out format)
                ? format(v)
                : FormatString(v.ToString());
        }

        private static string FormatBoolean(object b) { return (bool)b ? "t" : "f"; }

        private static string FormatFloat(object f) { return ((IFormattable)f).ToString(null, CultureInfo.InvariantCulture); }

        private static string FormatInteger(object i) { return ((IFormattable)i).ToString(null, CultureInfo.InvariantCulture) + "i"; }

        private static string FormatString(string s) { return "\"" + s.Replace("\"", "\\\"") + "\""; }

        private static string FormatTimespan(object ts) { return ((TimeSpan)ts).TotalMilliseconds.ToString(CultureInfo.InvariantCulture); }
    }
}