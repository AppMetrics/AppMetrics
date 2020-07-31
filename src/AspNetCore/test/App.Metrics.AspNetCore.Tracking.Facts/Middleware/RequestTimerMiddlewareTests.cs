// <copyright file="RequestTimerMiddlewareTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Tracking.Middleware;
using App.Metrics.Timer;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using ITimer = App.Metrics.Timer.ITimer;

namespace App.Metrics.AspNetCore.Tracking.Facts.Middleware
{
    public class RequestTimerMiddlewareTests : IDisposable
    {
        private readonly Mock<ITimer> _mockTimer;
        private readonly Mock<IProvideTimerMetrics> _mockTimerMetrics;
        private readonly Mock<IMetrics> _mockMetrics;

        public RequestTimerMiddlewareTests() {

            _mockMetrics = new Mock<IMetrics>();
            var mockProvider = new Mock<IProvideMetrics>();
            _mockTimer = new Mock<ITimer>();
            _mockTimerMetrics = new Mock<IProvideTimerMetrics>();


            _mockMetrics.Setup(_ => _.Provider).Returns(mockProvider.Object);
            mockProvider.Setup(_ => _.Timer).Returns(_mockTimerMetrics.Object);
            _mockTimerMetrics.Setup(_ => _.Instance(It.IsAny<TimerOptions>())).Returns(_mockTimer.Object).Verifiable("Timer was not created.");
            _mockTimer.Setup(_ => _.NewContext()).Returns(() => new TimerContext(_mockTimer.Object, null));
        }

        [Fact]
        public async Task Middleware_starts_a_timer_before_invocation_of_next_and_ends_after_completion()
        {
            var tcs = new TaskCompletionSource<bool>();
            
            var sut = CreateMiddleware(context => tcs.Task);

            var invocation =  sut.Invoke(new DefaultHttpContext());

            _mockTimer.Verify(_ => _.StartRecording(), Times.Once);
            _mockTimer.Verify(_ => _.EndRecording(), Times.Never);

            tcs.SetResult(true);

            await invocation;

            _mockTimer.Verify(_ => _.StartRecording(), Times.Once);
            _mockTimer.Verify(_ => _.EndRecording(), Times.Once);
        }

        [Fact]
        public void An_erroring_next_still_closes_the_timer_session()
        {
            var expectedException = new Exception("Test error");

            var sut = CreateMiddleware(context => Task.FromException(expectedException));

            Func<Task> act = () => sut.Invoke(new DefaultHttpContext());

            act.Should().Throw<Exception>().Which.IsSameOrEqualTo(expectedException);

            _mockTimer.Verify(_ => _.StartRecording(), Times.Once);
            _mockTimer.Verify(_ => _.EndRecording(), Times.Once);
        }

        [Fact]
        public async Task Middleware_ignores_web_socket_requests()
        {
            var tcs = new TaskCompletionSource<bool>();

            var sut = CreateMiddleware(context => tcs.Task);

            var invocation = sut.Invoke(CreateWebSocketRequest());

            _mockTimer.Verify(_ => _.StartRecording(), Times.Never);
            _mockTimer.Verify(_ => _.EndRecording(), Times.Never);

            tcs.SetResult(true);

            await invocation;

            _mockTimer.Verify(_ => _.StartRecording(), Times.Never);
            _mockTimer.Verify(_ => _.EndRecording(), Times.Never);
        }

        private static DefaultHttpContext CreateWebSocketRequest()
        {
            var mockWebSocketFeature = new Mock<IHttpWebSocketFeature>();
            var featureCollection = new FeatureCollection();
            featureCollection.Set(mockWebSocketFeature.Object);
            mockWebSocketFeature.SetupGet(_ => _.IsWebSocketRequest).Returns(true);
            return new DefaultHttpContext(featureCollection);
        }

        private RequestTimerMiddleware CreateMiddleware(RequestDelegate next)
        {
            return new RequestTimerMiddleware(next, new OptionsWrapper<MetricsWebTrackingOptions>(new MetricsWebTrackingOptions()), new Mock<ILogger<RequestTimerMiddleware>>().Object, _mockMetrics.Object);
        }

        public void Dispose()
        {
            _mockTimerMetrics.Verify();
        }
    }
}
