// <copyright file="ActiveRequestCounterEndpointMiddlewareTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Tracking.Middleware;
using App.Metrics.Counter;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Metrics.AspNetCore.Tracking.Facts.Middleware
{
    public class ActiveRequestCounterEndpointMiddlewareTests
    {
        private static Expression<Func<CounterOptions, bool>> WithActiveName() => counterOptions => counterOptions.Name == "Active";
        private static Expression<Func<CounterOptions, bool>> WithActiveWebSocketName() => counterOptions => counterOptions.Name == "Active Web Sockets";


        private readonly Mock<IMetrics> _mockMetrics;
        private readonly Mock<IMeasureCounterMetrics> _mockCounterMetrics;

        public ActiveRequestCounterEndpointMiddlewareTests() {
            _mockMetrics = new Mock<IMetrics>();

            var mockMeasure = new Mock<IMeasureMetrics>();
            _mockMetrics.Setup(_ => _.Measure).Returns(mockMeasure.Object);
            _mockCounterMetrics = new Mock<IMeasureCounterMetrics>();
            mockMeasure.Setup(_ => _.Counter).Returns(_mockCounterMetrics.Object);
        }

        [Fact]
        public async Task Middleware_increments_count_before_invoke_and_decrements_after_invoke()
        {
            var tcs = new TaskCompletionSource<bool>();
            
            var sut = CreateMiddleware(context => tcs.Task);

            var invocation =  sut.Invoke(new DefaultHttpContext());

            _mockCounterMetrics.Verify(_ => _.Increment(It.Is<CounterOptions>(WithActiveName())), Times.Once);
            _mockCounterMetrics.Verify(_ => _.Decrement(It.Is<CounterOptions>(WithActiveName())), Times.Never);

            tcs.SetResult(true);

            await invocation;

            _mockCounterMetrics.Verify(_ => _.Increment(It.Is<CounterOptions>(WithActiveName())), Times.Once);
            _mockCounterMetrics.Verify(_ => _.Decrement(It.Is<CounterOptions>(WithActiveName())), Times.Once);

            _mockCounterMetrics.Verify(_ => _.Increment(It.Is<CounterOptions>(WithActiveWebSocketName())), Times.Never);
            _mockCounterMetrics.Verify(_ => _.Decrement(It.Is<CounterOptions>(WithActiveWebSocketName())), Times.Never);
        }

        [Fact]
        public void An_erroring_next_still_decrements_active_count()
        {
            var expectedException = new Exception("Test error");

            var sut = CreateMiddleware(context => Task.FromException(expectedException));

            Func<Task> act = () => sut.Invoke(new DefaultHttpContext());

            act.Should().Throw<Exception>().Which.IsSameOrEqualTo(expectedException);

            _mockCounterMetrics.Verify(_ => _.Increment(It.Is<CounterOptions>(WithActiveName())), Times.Once);
            _mockCounterMetrics.Verify(_ => _.Decrement(It.Is<CounterOptions>(WithActiveName())), Times.Once);
        }

        [Fact]
        public async Task Middleware_tracks_web_socket_requests_separately()
        {
            var tcs = new TaskCompletionSource<bool>();

            var sut = CreateMiddleware(context => tcs.Task);

            var invocation = sut.Invoke(CreateWebSocketRequest());

            _mockCounterMetrics.Verify(_ => _.Increment(It.Is<CounterOptions>(WithActiveWebSocketName())), Times.Once);
            _mockCounterMetrics.Verify(_ => _.Decrement(It.Is<CounterOptions>(WithActiveWebSocketName())), Times.Never);

            tcs.SetResult(true);

            await invocation;

            _mockCounterMetrics.Verify(_ => _.Increment(It.Is<CounterOptions>(WithActiveWebSocketName())), Times.Once);
            _mockCounterMetrics.Verify(_ => _.Decrement(It.Is<CounterOptions>(WithActiveWebSocketName())), Times.Once);

            _mockCounterMetrics.Verify(_ => _.Increment(It.Is<CounterOptions>(WithActiveName())), Times.Never);
            _mockCounterMetrics.Verify(_ => _.Decrement(It.Is<CounterOptions>(WithActiveName())), Times.Never);

        }

        private static DefaultHttpContext CreateWebSocketRequest()
        {
            var mockWebSocketFeature = new Mock<IHttpWebSocketFeature>();
            var featureCollection = new FeatureCollection();
            featureCollection.Set(mockWebSocketFeature.Object);
            mockWebSocketFeature.SetupGet(_ => _.IsWebSocketRequest).Returns(true);
            return new DefaultHttpContext(featureCollection);
        }

        private ActiveRequestCounterEndpointMiddleware CreateMiddleware(RequestDelegate next)
        {
            return new ActiveRequestCounterEndpointMiddleware(next, new Mock<ILogger<ActiveRequestCounterEndpointMiddleware>>().Object, _mockMetrics.Object);
        }
    }
}
