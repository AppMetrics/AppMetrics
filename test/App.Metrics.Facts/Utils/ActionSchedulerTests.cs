using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Utils;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Utils
{
    public class ActionSchedulerTests
    {
        [Fact]
        public void ActionScheduler_ExecutesScheduledAction()
        {
            using (var scheduler = new ActionScheduler())
            {
                var tcs = new TaskCompletionSource<bool>();
                var data = 0;

                scheduler.Start(TimeSpan.FromMilliseconds(10), t =>
                {
                    data++;
                    tcs.SetResult(true);
                });

                tcs.Task.Wait();
                scheduler.Stop();

                data.Should().Be(1);
            }
        }

        [Fact]
        public void ActionScheduler_ExecutesScheduledActionMultipleTimes()
        {
            using (var scheduler = new ActionScheduler())
            {
                var data = 0;
                var tcs = new TaskCompletionSource<bool>();

                scheduler.Start(TimeSpan.FromMilliseconds(10), () =>
                {
                    data++;
                    tcs.SetResult(true);
                });

                tcs.Task.Wait();
                data.Should().Be(1);

                tcs = new TaskCompletionSource<bool>();
                tcs.Task.Wait();
                data.Should().Be(2);

                scheduler.Stop();
            }
        }

        [Fact]
        public void ActionScheduler_ExecutesScheduledActionWithToken()
        {
            using (var scheduler = new ActionScheduler())
            {
                var data = 0;
                var tcs = new TaskCompletionSource<bool>();

                scheduler.Start(TimeSpan.FromMilliseconds(10), t =>
                {
                    data++;
                    tcs.SetResult(true);
                });

                tcs.Task.Wait();
                scheduler.Stop();
                data.Should().Be(1);
            }
        }

        [Fact]
        public void ActionScheduler_ExecutesScheduledFunction()
        {
            using (var scheduler = new ActionScheduler())
            {
                var tcs = new TaskCompletionSource<bool>();
                var data = 0;

                Func<CancellationToken, Task> function = (t) => Task.Factory.StartNew(() =>
                {
                    data++;
                    tcs.SetResult(true);
                });

                scheduler.Start(TimeSpan.FromMilliseconds(10), function);
                tcs.Task.Wait();
                scheduler.Stop();

                data.Should().Be(1);
            }
        }

        [Fact(Skip = "refactor static metrics class and allow error handler to be added")]
        public void ActionScheduler_ReportsExceptionWithGlobalMetricHandler()
        {
            //Exception x = null;
            //var tcs = new TaskCompletionSource<bool>();

            //Metric.Config.WithErrorHandler(e =>
            //{
            //    x = e;
            //    tcs.SetResult(true);
            //});

            //using (var scheduler = new ActionScheduler())
            //{
            //    scheduler.Start(TimeSpan.FromMilliseconds(10), t => { throw new InvalidOperationException("boom"); });

            //    tcs.Task.Wait(1000);
            //    scheduler.Stop();
            //}

            //x.Should().NotBeNull();
        }
    }
}