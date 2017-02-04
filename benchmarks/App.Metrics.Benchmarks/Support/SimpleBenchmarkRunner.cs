// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace App.Metrics.Benchmarks.Support
{
    public static class SimpleBenchmarkRunner
    {
        private const int NumberOfRuns = 10000;
        private const int WarmupRuns = 10;
        private static readonly int ThreadCount = Environment.ProcessorCount;

        public static void Run(Action action, bool warmup = false)
        {
            if (warmup)
            {
                Warmup(action);
            }

            var thread = new List<Thread>();

            for (var i = 0; i < ThreadCount; i++)
            {
                thread.Add(
                    new Thread(
                        () =>
                        {
                            for (long j = 0; j < NumberOfRuns; j++)
                            {
                                action();
                            }
                        }));
            }

            thread.ForEach(t => t.Start());
            thread.ForEach(t => t.Join());
        }

        private static void PerformCollection()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Thread.Sleep(200);
        }

        private static void Warmup(Action action)
        {
            foreach (var i in Enumerable.Range(1, WarmupRuns))
            {
                action();
            }

            PerformCollection();
        }
    }
}