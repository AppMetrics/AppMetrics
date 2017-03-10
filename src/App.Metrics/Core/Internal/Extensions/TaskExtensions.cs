// <copyright file="TaskExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Internal;

// ReSharper disable CheckNamespace
namespace System.Threading.Tasks
{
    // ReSharper restore CheckNamespace
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