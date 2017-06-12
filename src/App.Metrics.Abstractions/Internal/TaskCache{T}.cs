// <copyright file="TaskCache{T}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace App.Metrics.Internal
{
    internal static class TaskCache<T>
    {
        public static Task<T> DefaultCompletedTask { get; } = Task.FromResult(default(T));
    }
}