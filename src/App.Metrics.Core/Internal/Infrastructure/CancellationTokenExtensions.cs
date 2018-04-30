// <copyright file="CancellationTokenExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>
// ReSharper disable CheckNamespace
namespace System.Threading
    // ReSharper restore CheckNamespace
{
    internal static class CancellationTokenExtensions
    {
        internal static bool WaitCancellationRequested(
            this CancellationToken token,
            TimeSpan timeout) { return token.WaitHandle.WaitOne(timeout); }
    }
}