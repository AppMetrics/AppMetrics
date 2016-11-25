using System.Collections.Generic;
using System.Threading;
using App.Metrics.Concurrency.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class ConcurrencyTests
    {
        [Theory]
        [
            InlineData(1000000, 16),
            InlineData(1000000, 8),
            InlineData(1000000, 4),
            InlineData(1000000, 2),
            InlineData(1000000, 1),
        ]
        public void Concurrency_AtomicIntArray_IsCorrectWithConcurrency(int total, int threadCount)
        {
            var array = new AtomicIntArray(100);
            var thread = new List<Thread>();

            for (var i = 0; i < threadCount; i++)
            {
                thread.Add(new Thread(() =>
                {
                    int index = 0;
                    for (long j = 0; j < total; j++)
                    {
                        array.Increment(index++);
                        if (index == array.Length)
                        {
                            index = 0;
                        }
                    }
                }));
            }

            thread.ForEach(t => t.Start());
            thread.ForEach(t => t.Join());

            long sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array.GetValue(i);
            }

            sum.Should().Be(total * threadCount);
        }

        [Theory]
        [
            InlineData(1000000, 16),
            InlineData(1000000, 8),
            InlineData(1000000, 4),
            InlineData(1000000, 2),
            InlineData(1000000, 1),
        ]
        public void Concurrency_AtomicInteger_IsCorrectWithConcurrency(int total, int threadCount)
        {
            ConcurrencyTest<AtomicInteger, int>(total, threadCount);
        }

        [Theory]
        [
            InlineData(1000000, 16),
            InlineData(1000000, 8),
            InlineData(1000000, 4),
            InlineData(1000000, 2),
            InlineData(1000000, 1),
        ]
        public void Concurrency_AtomicLong_IsCorrectWithConcurrency(long total, int threadCount)
        {
            ConcurrencyTest<AtomicLong, long>(total, threadCount);
        }

        [Theory]
        [
            InlineData(1000000, 16),
            InlineData(1000000, 8),
            InlineData(1000000, 4),
            InlineData(1000000, 2),
            InlineData(1000000, 1),
        ]
        public void Concurrency_AtomicLongArray_IsCorrectWithConcurrency(int total, int threadCount)
        {
            var array = new AtomicLongArray(100);
            var thread = new List<Thread>();

            for (var i = 0; i < threadCount; i++)
            {
                thread.Add(new Thread(() =>
                {
                    int index = 0;
                    for (long j = 0; j < total; j++)
                    {
                        array.Increment(index++);
                        if (index == array.Length)
                        {
                            index = 0;
                        }
                    }
                }));
            }

            thread.ForEach(t => t.Start());
            thread.ForEach(t => t.Join());

            long sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array.GetValue(i);
            }

            sum.Should().Be(total * threadCount);
        }

        [Theory]
        [
            InlineData(1000000, 16),
            InlineData(1000000, 8),
            InlineData(1000000, 4),
            InlineData(1000000, 2),
            InlineData(1000000, 1),
        ]
        public void Concurrency_PaddedAtomicLong_IsCorrectWithConcurrency(long total, int threadCount)
        {
            ConcurrencyTest<PaddedAtomicLong, long>(total, threadCount);
        }

        [Theory]
        [
            InlineData(1000000, 16),
            InlineData(1000000, 8),
            InlineData(1000000, 4),
            InlineData(1000000, 2),
            InlineData(1000000, 1),
        ]
        public void Concurrency_StripedLongAdder_IsCorrectWithConcurrency(long total, int threadCount)
        {
            ConcurrencyTest<StripedLongAdder, long>(total, threadCount);
        }

        [Theory]
        [
            InlineData(1000000, 16),
            InlineData(1000000, 8),
            InlineData(1000000, 4),
            InlineData(1000000, 2),
            InlineData(1000000, 1),
        ]
        public void Concurrency_ThreadLocalLongAdder_IsCorrectWithConcurrency(long total, int threadCount)
        {
            ConcurrencyTest<ThreadLocalLongAdder, long>(total, threadCount);
        }

        private static void ConcurrencyTest<T, U>(long total, int threadCount) where T : IValueAdder<U>, new()
        {
            var value = new T();
            var thread = new List<Thread>();

            for (var i = 0; i < threadCount; i++)
            {
                thread.Add(new Thread(() =>
                {
                    for (long j = 0; j < total; j++)
                    {
                        value.Increment();
                    }
                }));
            }

            thread.ForEach(t => t.Start());
            thread.ForEach(t => t.Join());

            var result = value.GetValue();
            if (result is int)
            {
                AssertionExtensions.Should((object)value.GetValue()).Be((int)total * threadCount);
            }
            else
            {
                AssertionExtensions.Should((object)value.GetValue()).Be(total * threadCount);
            }
        }
    }
}