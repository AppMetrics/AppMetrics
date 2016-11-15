// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;

namespace App.Metrics.Internal.Extensions
{
    internal static class CancellationTokenExtensions
    {
        internal static bool WaitCancellationRequested(
            this CancellationToken token,
            TimeSpan timeout)
        {
            return token.WaitHandle.WaitOne(timeout);
        }
    }
}