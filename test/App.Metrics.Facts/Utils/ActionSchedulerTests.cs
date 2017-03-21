// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Scheduling;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Utils
{
    public class DefaultTaskSchedulerTests
    {
        [Fact]
        public void can_execute_scheduled_action()
        {
            using (var scheduler = new DefaultTaskScheduler())
            {
                var data = 0;
                var completionSource = new TaskCompletionSource<bool>();

                scheduler.Interval(
                    TimeSpan.FromMilliseconds(20),
                    TaskCreationOptions.LongRunning,
                    () =>
                    {
                        data++;
                        completionSource.SetResult(true);
                    });

                completionSource.Task.Wait();
                scheduler.Stop();

                data.Should().Be(1);
            }
        }

        [Fact]
        public void can_provide_own_token_with_is_then_linked()
        {
            Task scheduledTask = null;
            var token = new CancellationTokenSource();
            token.CancelAfter(200);
            Action action = () =>
            {
                using (var scheduler = new DefaultTaskScheduler())
                {
                    scheduledTask = scheduler.Interval(
                        TimeSpan.FromMilliseconds(20),
                        TaskCreationOptions.LongRunning,
                        async () =>
                        {
                            // ReSharper disable MethodSupportsCancellation
                            await Task.Delay(1000);
                            // ReSharper restore MethodSupportsCancellation
                        },
                        token.Token);

                    // ReSharper disable MethodSupportsCancellation
                    scheduledTask.Wait();
                    // ReSharper restore MethodSupportsCancellation
                }
            };

            action.ShouldNotThrow<TaskCanceledException>();
            scheduledTask.Status.Should().Be(TaskStatus.RanToCompletion);
        }

        //TODO: What did this test start to cause the test runner to hang after upgraded to VS2017
        //[Fact]
        //public void executes_scheduled_action_muliple_times()
        //{
        //    using (var scheduler = new DefaultTaskScheduler())
        //    {
        //        var data = 0;
        //        var completionSource = new TaskCompletionSource<bool>();

        //        var source = completionSource;
        //        scheduler.Interval(
        //            TimeSpan.FromMilliseconds(20),
        //            TaskCreationOptions.LongRunning,
        //            () =>
        //            {
        //                data++;
        //                source.SetResult(true);
        //            });

        //        completionSource.Task.Wait();
        //        data.Should().Be(1);

        //        completionSource = new TaskCompletionSource<bool>();
        //        completionSource.Task.Wait();
        //        data.Should().BeGreaterOrEqualTo(2);

        //        scheduler.Stop();
        //    }
        //}

        [Fact]
        public void executes_scheduled_action_with_token()
        {
            using (var scheduler = new DefaultTaskScheduler())
            {
                var data = 0;
                var token = new CancellationTokenSource();
                var completionSource = new TaskCompletionSource<bool>();

                scheduler.Interval(
                    TimeSpan.FromMilliseconds(100),
                    TaskCreationOptions.LongRunning,
                    () =>
                    {
                        data++;
                        completionSource.SetResult(true);
                    },
                    token.Token);

                completionSource.Task.Wait(token.Token);
                scheduler.Stop();
                data.Should().Be(1);
            }
        }

        [Fact]
        public void gracefully_cancel_task_if_the_action_throws()
        {
            Task scheduledTask = null;

            Action action = () =>
            {
                using (var scheduler = new DefaultTaskScheduler())
                {
                    scheduledTask = scheduler.Interval(
                        TimeSpan.FromMilliseconds(20),
                        TaskCreationOptions.LongRunning,
                        () => throw new InvalidOperationException());

                    scheduledTask.Wait();
                }
            };

            action.ShouldNotThrow();
            scheduledTask.Status.Should().Be(TaskStatus.RanToCompletion);
        }

        [Fact]
        public void if_task_has_already_started_just_return_the_started_task()
        {
            using (var scheduler = new DefaultTaskScheduler(false))
            {
                var data = 0;
                var completionSource = new TaskCompletionSource<bool>();

                scheduler.Interval(
                    TimeSpan.FromMilliseconds(20),
                    TaskCreationOptions.LongRunning,
                    () =>
                    {
                        data++;
                        completionSource.SetResult(true);
                    });

                scheduler.Interval(
                    TimeSpan.FromMilliseconds(20),
                    TaskCreationOptions.LongRunning,
                    () =>
                    {
                        data++;
                        completionSource.SetResult(true);
                    });

                completionSource.Task.Wait();
                scheduler.Stop();

                data.Should().Be(1);
            }
        }

        [Fact]
        public void throws_if_poll_interval_is_zero()
        {
            Action action = () =>
            {
                using (var scheduler = new DefaultTaskScheduler())
                {
                    var completionSource = new TaskCompletionSource<bool>();

                    scheduler.Interval(TimeSpan.Zero, TaskCreationOptions.LongRunning, () => { completionSource.SetResult(true); });

                    completionSource.Task.Wait();
                    scheduler.Stop();
                }
            };

            action.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}