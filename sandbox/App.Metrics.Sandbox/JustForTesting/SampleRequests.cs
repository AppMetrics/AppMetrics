using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Scheduling;

namespace App.Metrics.Sandbox.JustForTesting
{
    public static class SampleRequests
    {
        private static readonly Uri ApiBaseAddress = new Uri("http://localhost:1111");

        public static void Run(CancellationToken token)
        {
            var scheduler = new DefaultTaskScheduler();
            var httpClient = new HttpClient
                             {
                                 BaseAddress = ApiBaseAddress
                             };

            Task.Run(
                () => scheduler.Interval(
                    TimeSpan.FromMilliseconds(200),
                    TaskCreationOptions.None,
                    async () =>
                    {
                        var satisfied = httpClient.GetAsync("api/satisfying", token);
                        var tolerating = httpClient.GetAsync("api/tolerating", token);
                        var frustrating = httpClient.GetAsync("api/frustrating", token);

                        await Task.WhenAll(satisfied, tolerating, frustrating);
                    },
                    token),
                token);

            Task.Run(
                () => scheduler.Interval(
                    TimeSpan.FromSeconds(1),
                    TaskCreationOptions.None,
                    async () =>
                    {
                        var satisfied = httpClient.GetAsync("api/randomstatuscode", token);
                        var internalException = httpClient.GetAsync("api/randomstatuscode/500", token);

                        await Task.WhenAll(satisfied, internalException);
                    },
                    token),
                token);
        }
    }
}
