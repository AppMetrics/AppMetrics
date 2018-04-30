// <copyright file="CounterMetricTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Counter;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Counter
{
    public class CounterMetricTests
    {
        private readonly DefaultCounterMetric _counter = new DefaultCounterMetric();

        [Fact]
        public void Can_be_incremented_on_multiple_threads()
        {
            const int threadCount = 16;
            const long iterations = 1000 * 100;

            var threads = new List<Thread>();
            var tcs = new TaskCompletionSource<int>();

            for (var i = 0; i < threadCount; i++)
            {
                threads.Add(
                    new Thread(
                        s =>
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
        public void Can_compute_percent_with_zero_total()
        {
            _counter.Increment("A");
            _counter.Decrement("A");

            _counter.Value.Count.Should().Be(0);

            _counter.Value.Items[0].Item.Should().Be("A");
            _counter.Value.Items[0].Count.Should().Be(0);
            _counter.Value.Items[0].Percent.Should().Be(0);
        }

        [Fact]
        public void Can_count_for_multiple_set_items()
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
        public void Can_count_for_set_item()
        {
            _counter.Increment("A");
            _counter.Value.Count.Should().Be(1L);
            _counter.Value.Items.Should().HaveCount(1);

            _counter.Value.Items[0].Item.Should().Be("A");
            _counter.Value.Items[0].Count.Should().Be(1);
            _counter.Value.Items[0].Percent.Should().Be(100);
        }

        [Fact]
        public void Can_decrement()
        {
            _counter.Decrement();
            _counter.Value.Count.Should().Be(-1L);
        }

        [Fact]
        public void Can_decrement_item_by_amount()
        {
            _counter.Increment("test-item", 2L);
            _counter.Decrement("test-item");
            _counter.Value.Items.First().Count.Should().Be(1L);
        }

        [Fact]
        public void Can_decrement_multiple_times()
        {
            _counter.Decrement();
            _counter.Decrement();
            _counter.Decrement();
            _counter.Value.Count.Should().Be(-3L);
        }

        [Fact]
        public void Can_decrement_with_value()
        {
            _counter.Decrement(32L);
            _counter.Value.Count.Should().Be(-32L);
        }

        [Fact]
        public void Can_get_value()
        {
            _counter.Increment();
            _counter.GetValue().Count.Should().Be(1L);
        }

        [Fact]
        public void Can_get_value_and_reset()
        {
            _counter.Increment();
            var value = _counter.GetValue(resetMetric: true);
            value.Count.Should().Be(1L);
            _counter.Value.Count.Should().Be(0);
        }

        [Fact]
        public void Can_increment()
        {
            _counter.Increment();
            _counter.Value.Count.Should().Be(1L);
        }

        [Fact]
        public void Can_increment_and_decrement_metric_item()
        {
            var item = new MetricSetItem("test-item", "value");
            _counter.Increment(item, 2L);
            _counter.Decrement(item);
            _counter.Value.Items.First().Count.Should().Be(1L);
        }

        [Fact]
        public void Can_increment_multiple_times()
        {
            _counter.Increment();
            _counter.Increment();
            _counter.Increment();
            _counter.Value.Count.Should().Be(3L);
        }

        [Fact]
        public void Can_increment_with_value()
        {
            _counter.Increment(32L);
            _counter.Value.Count.Should().Be(32L);
        }

        [Fact]
        public void Can_reset()
        {
            _counter.Increment();
            _counter.Value.Count.Should().Be(1L);
            _counter.Reset();
            _counter.Value.Count.Should().Be(0L);
        }

        [Fact]
        public void Can_reset_item()
        {
            _counter.Increment("A");
            _counter.GetValueOrDefault().Items[0].Count.Should().Be(1);
            _counter.Reset();
            _counter.GetValueOrDefault().Items[0].Count.Should().Be(0L);
        }

        [Fact]
        public void Count_should_start_from_zero()
        {
            _counter.Value.Count.Should().Be(0L);
        }
    }
}