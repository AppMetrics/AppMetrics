// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Diagnostics;

// ReSharper disable CheckNamespace

namespace System
// ReSharper restore CheckNamespace
{
    internal static class StringExtensions
    {
        [DebuggerStepThrough]
        internal static string CleanUrlPath(this string url)
        {
            if (string.IsNullOrWhiteSpace(url)) url = "/";

            if (url != "/" && url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return url;
        }

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
        internal static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        internal static bool IsMissingOrTooLong(this string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }
            if (value.Length > maxLength)
            {
                return true;
            }

            return false;
        }

        [DebuggerStepThrough]
        internal static bool IsPresent(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        internal static string RemoveLeadingSlash(this string url)
        {
            if (url != null && url.StartsWith("/"))
            {
                url = url.Substring(1);
            }

            return url;
        }

        [DebuggerStepThrough]
        internal static string RemoveTrailingSlash(this string url)
        {
            if (url != null && url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return url;
        }
    }
}