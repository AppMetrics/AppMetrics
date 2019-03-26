// <copyright file="StringExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics;

// ReSharper disable CheckNamespace
namespace System
    // ReSharper restore CheckNamespace
{
    internal static class StringExtensions
    {
        [DebuggerStepThrough]
        internal static string EnsureLeadingSlash(this string url)
        {
            if (!url.StartsWith("/"))
            {
                return "/" + url;
            }

            return url;
        }

        [DebuggerStepThrough]
        internal static string EnsureTrailingSlash(this string url)
        {
            if (!url.EndsWith("/"))
            {
                return url + "/";
            }

            return url;
        }

        [DebuggerStepThrough]
        internal static string GetSafeString(Func<string> action)
        {
            try
            {
                return action();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        [DebuggerStepThrough]
        internal static bool IsMissing(this string value) { return string.IsNullOrWhiteSpace(value); }

        [DebuggerStepThrough]
        internal static bool IsPresent(this string value) { return !string.IsNullOrWhiteSpace(value); }

        [DebuggerStepThrough]
        internal static string RemoveLeadingSlash(this string url)
        {
            if (url != null && url.StartsWith("/"))
            {
                url = url.Substring(1);
            }

            return url;
        }
    }
}