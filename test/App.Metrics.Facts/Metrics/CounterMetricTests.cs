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
        private readonly CounterMetric counter = new CounterMetric();

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
                        counter.Increment();
                    }
                }));
            }

            threads.ForEach(t => t.Start());
            tcs.SetResult(0);
            threads.ForEach(t => t.Join());

            counter.Value.Count.Should().Be(threadCount * iterations);
        }

        [Fact]
        public void CounterMetric_CanComputePercentWithZeroTotal()
        {
            counter.Increment("A");
            counter.Decrement("A");

            counter.Value.Count.Should().Be(0);

            counter.Value.Items[0].Item.Should().Be("A");
            counter.Value.Items[0].Count.Should().Be(0);
            counter.Value.Items[0].Percent.Should().Be(0);
        }

        [Fact]
        public void CounterMetric_CanCountForMultipleSetItem()
        {
            counter.Increment("A");
            counter.Increment("B");

            counter.Value.Count.Should().Be(2L);
            counter.Value.Items.Should().HaveCount(2);

            counter.Value.Items[0].Item.Should().Be("A");
            counter.Value.Items[0].Count.Should().Be(1);
            counter.Value.Items[0].Percent.Should().Be(50);

            counter.Value.Items[1].Item.Should().Be("B");
            counter.Value.Items[1].Count.Should().Be(1);
            counter.Value.Items[1].Percent.Should().Be(50);
        }

        [Fact]
        public void CounterMetric_CanCountForSetItem()
        {
            counter.Increment("A");
            counter.Value.Count.Should().Be(1L);
            counter.Value.Items.Should().HaveCount(1);

            counter.Value.Items[0].Item.Should().Be("A");
            counter.Value.Items[0].Count.Should().Be(1);
            counter.Value.Items[0].Percent.Should().Be(100);
        }

        [Fact]
        public void CounterMetric_CanDecrement()
        {
            counter.Decrement();
            counter.Value.Count.Should().Be(-1L);
        }

        [Fact]
        public void CounterMetric_CanDecrementMultipleTimes()
        {
            counter.Decrement();
            counter.Decrement();
            counter.Decrement();
            counter.Value.Count.Should().Be(-3L);
        }

        [Fact]
        public void CounterMetric_CanDecrementWithValue()
        {
            counter.Decrement(32L);
            counter.Value.Count.Should().Be(-32L);
        }

        [Fact]
        public void CounterMetric_CanIncrement()
        {
            counter.Increment();
            counter.Value.Count.Should().Be(1L);
        }

        [Fact]
        public void CounterMetric_CanIncrementMultipleTimes()
        {
            counter.Increment();
            counter.Increment();
            counter.Increment();
            counter.Value.Count.Should().Be(3L);
        }

        [Fact]
        public void CounterMetric_CanIncrementWithValue()
        {
            counter.Increment(32L);
            counter.Value.Count.Should().Be(32L);
        }

        [Fact]
        public void CounterMetric_CanReset()
        {
            counter.Increment();
            counter.Value.Count.Should().Be(1L);
            counter.Reset();
            counter.Value.Count.Should().Be(0L);
        }

        [Fact]
        public void CounterMetric_CanResetSetItem()
        {
            counter.Increment("A");
            counter.Value.Items[0].Count.Should().Be(1);
            counter.Reset();
            counter.Value.Items[0].Count.Should().Be(0L);
        }

        [Fact]
        public void CounterMetric_StartsFromZero()
        {
            counter.Value.Count.Should().Be(0L);
        }
    }
}