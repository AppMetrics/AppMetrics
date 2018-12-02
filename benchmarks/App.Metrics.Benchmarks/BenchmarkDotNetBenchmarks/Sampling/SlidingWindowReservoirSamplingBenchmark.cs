// <copyright file="SlidingWindowReservoirSamplingBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Internal;
using App.Metrics.ReservoirSampling.SlidingWindow;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Sampling
{
    public class SlidingWindowReservoirSamplingBenchmark : DefaultBenchmarkBase
    {
        private static readonly TimeSpan TickInterval = TimeSpan.FromMilliseconds(10);
        private readonly object _syncLock = new object();
        private MetricsCoreTestFixture _fixture;
        private DefaultSlidingWindowReservoir _reservoir;
        private IMetricsTaskSchedular _scheduler;
        private volatile bool _disposing;

        public override void Setup()
        {
            _fixture = new MetricsCoreTestFixture();

            _reservoir = new DefaultSlidingWindowReservoir();

            _scheduler = new DefaultMetricsTaskSchedular(c => Tick());
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            lock (_syncLock)
            {
                if (_disposing)
                {
                    return;
                }

                _disposing = true;
            }

            _scheduler.Dispose();

            Tick().GetAwaiter().GetResult();
        }

        [Benchmark]
        public void Update() { _reservoir.Update(_fixture.Rnd.Next(0, 1000), _fixture.RandomUserValue); }

        private Task Tick()
        {
            try
            {
                _reservoir.GetSnapshot();
                _reservoir.Reset();
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                lock (_syncLock)
                {
                    if (!_disposing)
                    {
                        SetScheduler();
                    }
                }
            }

            return Task.CompletedTask;
        }

        private void SetScheduler()
        {
            _scheduler.Start(TickInterval);
        }
    }
}