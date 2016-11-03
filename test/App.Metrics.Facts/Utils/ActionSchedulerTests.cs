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

                scheduler.Interval(TimeSpan.FromMilliseconds(10), () =>
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
        public void executes_scheduled_action_muliple_times()
        {
            using (var scheduler = new DefaultTaskScheduler())
            {
                var data = 0;
                var completionSource = new TaskCompletionSource<bool>();

                scheduler.Interval(TimeSpan.FromMilliseconds(10), () =>
                {
                    data++;
                    completionSource.SetResult(true);
                });

                completionSource.Task.Wait();
                data.Should().Be(1);

                completionSource = new TaskCompletionSource<bool>();
                completionSource.Task.Wait();
                data.Should().Be(2);

                scheduler.Stop();
            }
        }

        [Fact]
        public void executes_scheduled_action_with_token()
        {
            using (var scheduler = new DefaultTaskScheduler())
            {
                var data = 0;
                var token = new CancellationTokenSource();                
                var completionSource = new TaskCompletionSource<bool>();

                scheduler.Interval(TimeSpan.FromMilliseconds(10), () =>
                 {
                     data++;
                     completionSource.SetResult(true);
                 }, token.Token);

                completionSource.Task.Wait(token.Token);
                scheduler.Stop();
                data.Should().Be(1);
            }
        }        
    }
}