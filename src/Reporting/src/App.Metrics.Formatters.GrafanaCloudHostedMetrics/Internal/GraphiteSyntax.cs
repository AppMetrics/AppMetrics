// <copyright file="GraphiteSyntax.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal
{
    public static class GraphiteSyntax
    {
        private const RegexOptions RegexOptions = System.Text.RegularExpressions.RegexOptions.CultureInvariant | System.Text.RegularExpressions.RegexOptions.Compiled;
        private static readonly Regex InvalidAllowDotsRegex = new Regex(@"[^a-zA-Z0-9\-%&.]+", RegexOptions);
        private static readonly Regex InvalidRegex = new Regex(@"[^a-zA-Z0-9\-%&]+", RegexOptions);

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

        public static string EscapeName(string nameOrKey) => EscapeName(nameOrKey, false);

        public static string EscapeName(string nameOrKey, bool allowDots)
        {
            if (nameOrKey == null)
            {
                throw new ArgumentNullException(nameof(nameOrKey));
            }

            var regex = allowDots ? InvalidAllowDotsRegex : InvalidRegex;
            return regex.Replace(nameOrKey, "_");
        }

        public static string FormatTimestamp(DateTime utcTimestamp)
        {
            return ((int)utcTimestamp.Subtract(Origin).TotalSeconds).ToString("D", CultureInfo.InvariantCulture);
        }

        public static string FormatValue(object value)
        {
            var v = value ?? string.Empty;

            return Formatters.TryGetValue(v.GetType(), out Func<object, string> format)
                ? format(v)
                : FormatString(v.ToString());
        }

        private static string FormatBoolean(object b) { return (bool)b ? "t" : "f"; }

        private static string FormatFloat(object f) { return ((IFormattable)f).ToString("F", CultureInfo.InvariantCulture); }

        private static string FormatInteger(object i) { return ((IFormattable)i).ToString("D", CultureInfo.InvariantCulture); }

        private static string FormatString(string s) { return s; }

        private static string FormatTimespan(object ts) { return ((TimeSpan)ts).TotalMilliseconds.ToString(CultureInfo.InvariantCulture); }
    }
}