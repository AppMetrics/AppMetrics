// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Timer.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Providers
{
    public class DefaultTimerMetricProviderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IFilterMetrics _filter = new DefaultMetricsFilter().WhereType(MetricType.Timer);
        private readonly MetricCoreTestFixture _fixture;
        private readonly IProvideTimerMetrics _provider;


        public DefaultTimerMetricProviderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _provider = _fixture.Providers.Timer;
        }

        [Fact]
        public void can_add_add_new_instance_to_registry()
        {
            var metricName = "timer_provider_metric_test";
            var options = new TimerOptions
                          {
                              Name = metricName
                          };

            var reservoir = new Lazy<IReservoir>(() => new DefaultAlgorithmRReservoir(1028));

            var timerMetric = _fixture.Builder.Timer.Build(reservoir, _fixture.Clock);

            _provider.Instance(options, () => timerMetric);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Timers.Count().Should().Be(1);
        }

        [Fact]
        public void can_add_instance_to_registry()
        {
            var metricName = "timer_provider_test";
            var options = new TimerOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Timers.Count().Should().Be(1);
        }

        [Fact]
        public void can_add_instance_with_histogram()
        {
            var reservoirMock = new Mock<IHistogramMetric>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>(), null));
            reservoirMock.Setup(r => r.Reset());

            var options = new TimerOptions
                          {
                              Name = "timer_custom_histogram"
                          };

            var timer = _provider.WithHistogram(options, () => reservoirMock.Object);

            using (timer.NewContext())
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 100);
            }

            reservoirMock.Verify(r => r.Update(It.IsAny<long>(), null), Times.Once);
        }


        [Fact]
        public void can_use_custom_reservoir()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>(), null));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100L, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var reservoir = new Lazy<IReservoir>(() => reservoirMock.Object);

            var options = new TimerOptions
                          {
                              Name = "timer_provider_custom_test",
                              Reservoir = reservoir
                          };

            var timer = _provider.Instance(options);

            using (timer.NewContext())
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 100);
            }

            reservoirMock.Verify(r => r.Update(It.IsAny<long>(), null), Times.Once);
        }
    }
}