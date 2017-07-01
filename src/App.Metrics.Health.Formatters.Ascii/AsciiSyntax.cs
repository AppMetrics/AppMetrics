// <copyright file="AsciiSyntax.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;

namespace App.Metrics.Health.Formatters.Ascii
{
    public class AsciiSyntax
    {
        private const int Padding = 20;

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

        public static string FormatReadable(string label, string value, string sign = "=")
        {
            var pad = string.Empty;

            if (label.Length + 2 + sign.Length < Padding)
            {
                pad = new string(' ', Padding - label.Length - 1 - sign.Length);
            }

            return $"{pad}{label} {sign} {value}";
        }

        public static string FormatTimestamp(DateTime timestamp) { return timestamp.ToString("yyyyMMddHHmmss"); }

        public static string FormatValue(object value)
        {
            var v = value ?? string.Empty;

            return Formatters.TryGetValue(v.GetType(), out Func<object, string> format)
                ? format(v)
                : v.ToString();
        }

        private static string FormatBoolean(object b) { return (bool)b ? "true" : "false"; }

        private static string FormatFloat(object f) { return ((IFormattable)f).ToString(null, CultureInfo.InvariantCulture); }

        private static string FormatInteger(object i) { return ((IFormattable)i).ToString(null, CultureInfo.InvariantCulture); }

        private static string FormatTimespan(object ts) { return ((TimeSpan)ts).TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "ms"; }
    }
}