// <copyright file="TaskExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace
namespace System.Threading.Tasks
    // ReSharper restore CheckNamespace
{
    [ExcludeFromCodeCoverage]
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