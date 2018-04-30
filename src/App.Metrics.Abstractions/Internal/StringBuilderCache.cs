// <copyright file="StringBuilderCache.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text;

namespace App.Metrics.Internal
{
    /// <summary>
    ///     Original Source: https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Text/StringBuilderCache.cs
    /// </summary>
    internal static class StringBuilderCache
    {
        // The value 360 was chosen in discussion with performance experts as a compromise between using
        // as little memory (per thread) as possible and still covering a large part of short-lived
        // StringBuilder creations on the startup path of VS designers.
        private const int MaxBuilderSize = 360;

        [ThreadStatic]
        private static StringBuilder _cachedInstance;

        /// <summary>
        ///     Acquire is used to get a string builder to use of a particular size.It can be called any number of times, if a
        ///     stringbuilder is in the
        ///     cache then it will be returned and the cache emptied. Subsequent calls will return a new stringbuilder.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>A new or cached string buildeer for this thread</returns>
        public static StringBuilder Acquire(int capacity = 16)
        {
            if (capacity > MaxBuilderSize)
            {
                return new StringBuilder(capacity);
            }

            var sb = _cachedInstance;

            // Avoid stringbuilder block fragmentation by getting a new StringBuilder
            // when the requested size is larger than the current capacity
            if (capacity <= sb?.Capacity)
            {
                _cachedInstance = null;
                sb.Clear();
                return sb;
            }

            return new StringBuilder(capacity);
        }

        /// <summary>
        ///     ToString() the string builder, release it to the cache and return the resulting string
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <returns>The resulting string</returns>
        public static string GetStringAndRelease(StringBuilder sb)
        {
            var result = sb.ToString();
            Release(sb);
            return result;
        }

        /// <summary>
        ///     Place the specified builder in the cache if it is not too big. The stringbuilder should not be used after it has
        ///     been released.
        ///     Unbalanced Releases are perfectly acceptable. It will merely cause the runtime to create a new stringbuilder next
        ///     time Acquire is
        ///     called.
        /// </summary>
        /// <param name="sb">The string builder instance.</param>
        // ReSharper disable MemberCanBePrivate.Global
        public static void Release(StringBuilder sb)
            // ReSharper restore MemberCanBePrivate.Global
        {
            if (sb.Capacity <= MaxBuilderSize)
            {
                _cachedInstance = sb;
            }
        }
    }
}