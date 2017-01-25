// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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