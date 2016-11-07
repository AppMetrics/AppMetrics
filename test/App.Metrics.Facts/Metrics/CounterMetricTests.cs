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
        public void can_be_incremented_on_multiple_threads()
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
        public void can_compute_percent_with_zero_total()
        {
            _counter.Increment("A");
            _counter.Decrement("A");

            _counter.Value.Count.Should().Be(0);

            _counter.Value.Items[0].Item.Should().Be("A");
            _counter.Value.Items[0].Count.Should().Be(0);
            _counter.Value.Items[0].Percent.Should().Be(0);
        }

        [Fact]
        public void can_count_for_multiple_set_items()
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
        public void can_count_for_set_item()
        {
            _counter.Increment("A");
            _counter.Value.Count.Should().Be(1L);
            _counter.Value.Items.Should().HaveCount(1);

            _counter.Value.Items[0].Item.Should().Be("A");
            _counter.Value.Items[0].Count.Should().Be(1);
            _counter.Value.Items[0].Percent.Should().Be(100);
        }

        [Fact]
        public void can_decrement()
        {
            _counter.Decrement();
            _counter.Value.Count.Should().Be(-1L);
        }

        [Fact]
        public void can_decrement_multiple_times()
        {
            _counter.Decrement();
            _counter.Decrement();
            _counter.Decrement();
            _counter.Value.Count.Should().Be(-3L);
        }

        [Fact]
        public void can_decrement_with_value()
        {
            _counter.Decrement(32L);
            _counter.Value.Count.Should().Be(-32L);
        }

        [Fact]
        public void can_increment()
        {
            _counter.Increment();
            _counter.Value.Count.Should().Be(1L);
        }

        [Fact]
        public void can_increment_multiple_times()
        {
            _counter.Increment();
            _counter.Increment();
            _counter.Increment();
            _counter.Value.Count.Should().Be(3L);
        }

        [Fact]
        public void can_increment_with_value()
        {
            _counter.Increment(32L);
            _counter.Value.Count.Should().Be(32L);
        }

        [Fact]
        public void can_reset()
        {
            _counter.Increment();
            _counter.Value.Count.Should().Be(1L);
            _counter.Reset();
            _counter.Value.Count.Should().Be(0L);
        }

        [Fact]
        public void can_reset_item()
        {
            _counter.Increment("A");
            _counter.Value.Items[0].Count.Should().Be(1);
            _counter.Reset();
            _counter.Value.Items[0].Count.Should().Be(0L);
        }

        [Fact]
        public void count_should_start_from_zero()
        {
            _counter.Value.Count.Should().Be(0L);
        }
    }
}