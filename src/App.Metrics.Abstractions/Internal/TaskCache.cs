// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// ReSharper disable CheckNamespace
namespace System.Threading.Tasks
    // ReSharper restore CheckNamespace
{
    internal static class TaskCache
    {
#if NET452
        public static readonly Task CompletedTask = Task.FromResult(0);
#else
        public static readonly Task CompletedTask = Task.CompletedTask;
#endif
    }
}