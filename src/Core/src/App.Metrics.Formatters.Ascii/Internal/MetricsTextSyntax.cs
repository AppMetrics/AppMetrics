// <copyright file="MetricsTextSyntax.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;

namespace App.Metrics.Formatters.Ascii.Internal
{
    internal static class MetricsTextSyntax
    {
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

        public static string FormatReadable(string label, string value, string separator, int padding)
        {
            var pad = string.Empty;

            if (label.Length + 2 + separator.Length < padding)
            {
                pad = new string(' ', padding - label.Length - 1 - separator.Length);
            }

            return $"{pad}{label} {separator} {value}";
        }

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