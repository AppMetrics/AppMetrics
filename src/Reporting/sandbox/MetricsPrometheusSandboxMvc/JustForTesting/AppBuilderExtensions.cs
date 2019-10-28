// <copyright file="AppBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Scheduling;
using MetricsPrometheusSandboxMvc.JustForTesting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppBuilderExtensions
    {
        private const int ApdexSamplesInterval = 2;
        private const int GetEndpointSuccessInterval = 1;
        private const int PutAndPostRequestsInterval = 6;
        private const int RandomSamplesInterval = 10;
        private const int SlaEndpointsInterval = 2;
        private static readonly HttpClient HttpClient = new HttpClient { BaseAddress = new Uri("http://localhost:1111/") };

        public static IApplicationBuilder UseTestStuff(
            this IApplicationBuilder app,
            IHostApplicationLifetime lifetime,
            bool runSampleRequestsFromApp)
        {
            app.Use(
                (context, next) =>
                {
                    RandomClientIdForTesting.SetTheFakeClaimsPrincipal(context);
                    return next();
                });

            var token = lifetime.ApplicationStopping;

            if (runSampleRequestsFromApp)
            {
                var apdexSamples = new AppMetricsTaskScheduler(
                    TimeSpan.FromSeconds(ApdexSamplesInterval),
                    () =>
                    {
                        var satisfied = HttpClient.GetAsync("api/satisfying", token);
                        var tolerating = HttpClient.GetAsync("api/tolerating", token);
                        var frustrating = HttpClient.GetAsync("api/frustrating", token);

                        return Task.WhenAll(satisfied, tolerating, frustrating);
                    });

                apdexSamples.Start();

                var randomErrorSamples = new AppMetricsTaskScheduler(
                    TimeSpan.FromSeconds(RandomSamplesInterval),
                    () =>
                    {
                        var randomStatusCode = HttpClient.GetAsync("api/randomstatuscode", token);
                        var randomException = HttpClient.GetAsync("api/randomexception", token);

                        return Task.WhenAll(randomStatusCode, randomException);
                    });

                randomErrorSamples.Start();

                var testSamples = new AppMetricsTaskScheduler(
                    TimeSpan.FromSeconds(GetEndpointSuccessInterval),
                    () => HttpClient.GetAsync("api/test", token));

                testSamples.Start();

                var slaSamples = new AppMetricsTaskScheduler(
                    TimeSpan.FromSeconds(SlaEndpointsInterval),
                    () => HttpClient.GetAsync("api/slatest/timer", token));

                slaSamples.Start();

                var randomBufferGenerator = new RandomBufferGenerator(50000);
                var postPutSamples = new AppMetricsTaskScheduler(
                    TimeSpan.FromSeconds(PutAndPostRequestsInterval),
                    () =>
                    {
                        var putBytes = new ByteArrayContent(randomBufferGenerator.GenerateBufferFromSeed());
                        var putFormData = new MultipartFormDataContent { { putBytes, "put-file", "rnd-put" } };
                        var putRequest = HttpClient.PutAsync("api/file", putFormData, token);

                        var postBytes = new ByteArrayContent(randomBufferGenerator.GenerateBufferFromSeed());
                        var postFormData = new MultipartFormDataContent { { postBytes, "post-file", "rnd-post" } };
                        var postRequest = HttpClient.PostAsync("api/file", postFormData, token);

                        return Task.WhenAll(putRequest, postRequest);
                    });

                postPutSamples.Start();
            }

            return app;
        }
    }
}