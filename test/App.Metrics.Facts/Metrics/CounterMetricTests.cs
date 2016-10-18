using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class CounterMetricTests
    {
        private readonly CounterMetric _counter = new CounterMetric();

        [Fact]
        public void CounterMetric_CanBeIncrementedOnMultipleThreads()
        {
            const int threadCount = 16;
            const long iterations = 1000 * 100;

            var threads = new List<Thread>();
            var tcs = new TaskCompletionSource<int>();

            for (var i = 0; i < threadCount; i++)
            {
                threads.Add(new Thread(s =>
                {
                    tcs.Task.Wait();
                    for (long j = 0; j < iterations; j++)
                    {
                        _counter.Increment();
                    }
                }));
            }

            threads.ForEach(t => t.Start());
            tcs.SetResult(0);
            threads.ForEach(t => t.Join());

            _counter.Value.Count.Should().Be(threadCount * iterations);
        }

        [Fact]
        public void CounterMetric_CanComputePercentWithZeroTotal()
        {
            _counter.Increment("A");
            _counter.Decrement("A");

            _counter.Value.Count.Should().Be(0);

            _counter.Value.Items[0].Item.Should().Be("A");
            _counter.Value.Items[0].Count.Should().Be(0);
            _counter.Value.Items[0].Percent.Should().Be(0);
        }

        [Fact]
        public void CounterMetric_CanCountForMultipleSetItem()
        {
            _counter.Increment("A");
            _counter.Increment("B");

            _counter.Value.Count.Should().Be(2L);
            _counter.Value.Items.Should().HaveCount(2);

            _counter.Value.Items[0].Item.Should().Be("A");
            _counter.Value.Items[0].Count.Should().Be(1);
            _counter.Value.Items[0].Percent.Should().Be(50);

            _counter.Value.Items[1].Item.Should().Be("B");
            _counter.Value.Items[1].Count.Should().Be(1);
            _counter.Value.Items[1].Percent.Should().Be(50);
        }

        [Fact]
        public void CounterMetric_CanCountForSetItem()
        {
            _counter.Increment("A");
            _counter.Value.Count.Should().Be(1L);
            _counter.Value.Items.Should().HaveCount(1);

            _counter.Value.Items[0].Item.Should().Be("A");
            _counter.Value.Items[0].Count.Should().Be(1);
            _counter.Value.Items[0].Percent.Should().Be(100);
        }

        [Fact]
        public void CounterMetric_CanDecrement()
        {
            _counter.Decrement();
            _counter.Value.Count.Should().Be(-1L);
        }

        [Fact]
        public void CounterMetric_CanDecrementMultipleTimes()
        {
            _counter.Decrement();
            _counter.Decrement();
            _counter.Decrement();
            _counter.Value.Count.Should().Be(-3L);
        }

        [Fact]
        public void CounterMetric_CanDecrementWithValue()
        {
            _counter.Decrement(32L);
            _counter.Value.Count.Should().Be(-32L);
        }

        [Fact]
        public void CounterMetric_CanIncrement()
        {
            _counter.Increment();
            _counter.Value.Count.Should().Be(1L);
        }

        [Fact]
        public void CounterMetric_CanIncrementMultipleTimes()
        {
            _counter.Increment();
            _counter.Increment();
            _counter.Increment();
            _counter.Value.Count.Should().Be(3L);
        }

        [Fact]
        public void CounterMetric_CanIncrementWithValue()
        {
            _counter.Increment(32L);
            _counter.Value.Count.Should().Be(32L);
        }

        [Fact]
        public void CounterMetric_CanReset()
        {
            _counter.Increment();
            _counter.Value.Count.Should().Be(1L);
            _counter.Reset();
            _counter.Value.Count.Should().Be(0L);
        }

        [Fact]
        public void CounterMetric_CanResetSetItem()
        {
            _counter.Increment("A");
            _counter.Value.Items[0].Count.Should().Be(1);
            _counter.Reset();
            _counter.Value.Items[0].Count.Should().Be(0L);
        }

        [Fact]
        public void CounterMetric_StartsFromZero()
        {
            _counter.Value.Count.Should().Be(0L);
        }
    }
}