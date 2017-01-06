// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// ReSharper disable CheckNamespace

using App.Metrics.Internal;

namespace System.Threading.Tasks
// ReSharper restore CheckNamespace
{
    [AppMetricsExcludeFromCodeCoverage]
    internal static class TaskExtensions
    {
        public static async Task WithAggregateException(this Task source)
        {
            try
            {
                await source.ConfigureAwait(false);
            }
            catch
            {
                if (source.Exception != null)
                {
                    throw source.Exception;
                }                
            }
        }
    }
}