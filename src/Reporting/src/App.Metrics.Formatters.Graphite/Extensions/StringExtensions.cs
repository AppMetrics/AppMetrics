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
        internal static bool IsMissing(this string value) { return string.IsNullOrWhiteSpace(value); }

        [DebuggerStepThrough]
        internal static bool IsPresent(this string value) { return !string.IsNullOrWhiteSpace(value); }
    }
}