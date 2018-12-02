// <copyright file="AlgorithmRReservoirSamplingBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Internal;
using App.Metrics.ReservoirSampling.Uniform;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Sampling
{
    public class AlgorithmRReservoirSamplingBenchmark : DefaultBenchmarkBase
    {
        private static readonly TimeSpan TickInterval = TimeSpan.FromMilliseconds(10);
        private readonly object _syncLock = new object();
        private MetricsCoreTestFixture _fixture;
        private DefaultAlgorithmRReservoir _reservoir;
        private IMetricsTaskSchedular _scheduler;
        private volatile bool _disposing;

        public override void Setup()
        {
            _fixture = new MetricsCoreTestFixture();

            _reservoir = new DefaultAlgorithmRReservoir();

            _scheduler = new DefaultMetricsTaskSchedular(c => Tick());
        }

        [Benchmark]
        public void Update() { _reservoir.Update(_fixture.Rnd.Next(0, 1000), _fixture.RandomUserValue); }

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

        private void SetScheduler()
        {
            _scheduler.Start(TickInterval);
        }

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
    }
}