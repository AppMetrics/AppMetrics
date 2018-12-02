// <copyright file="DefaultReservoirRescaleSchedulerTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Internal;
using App.Metrics.ReservoirSampling;
using App.Metrics.Scheduling;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Scheduling
{
    public class DefaultReservoirRescaleSchedulerTests
    {
        [Fact]
        public void Uses_default_rescale_period_if_none_given()
        {
            using (var rescaleScheduler = new DefaultReservoirRescaleScheduler())
            {
                rescaleScheduler.RescalePeriod.Should().Be(TimeSpan.FromHours(1));
            }
        }

        [Fact]
        public void Applies_rescale_period_to_inner_scheduler()
        {
            var metricsScheduler = new Mock<IMetricsTaskSchedular>();
            using (var unused = new DefaultReservoirRescaleScheduler(TimeSpan.FromSeconds(10), metricsScheduler.Object))
            {
                metricsScheduler.Verify(s => s.Start(It.Is<TimeSpan>(ts => ts.Equals(TimeSpan.FromSeconds(10)))));
            }
        }

        [Fact]
        public async Task Invokes_rescaling_operation_on_reservoir()
        {
            var metricsScheduler = new Mock<IMetricsTaskSchedular>();
            Func<CancellationToken, Task> rescaleProc = null;
            metricsScheduler.Setup(s => s.SetTaskSource(It.IsAny<Func<CancellationToken, Task>>()))
                .Callback<Func<CancellationToken, Task>>(ts => rescaleProc = ts);

            var reservoir = new Mock<IRescalingReservoir>();

            using (var rescaleScheduler = new DefaultReservoirRescaleScheduler(TimeSpan.FromSeconds(1), metricsScheduler.Object))
            {
                rescaleProc.Should().NotBeNull($"{nameof(DefaultReservoirRescaleScheduler)} constructor should have set the rescale proc on inner scheduler");

                rescaleScheduler.ScheduleReScaling(reservoir.Object);
                await rescaleProc(CancellationToken.None);
                reservoir.Verify(r => r.Rescale(), Times.Once());
            }
        }
    }
}
