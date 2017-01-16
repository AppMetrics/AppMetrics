using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Interfaces;
using App.Metrics.Internal;
using App.Metrics.Internal.Managers;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultTimerManagerTests : IClassFixture<MetricManagerTestFixture>
    {
        private readonly MetricManagerTestFixture _fixture;
        private readonly IMeasureTimerMetrics _manager;

        public DefaultTimerManagerTests(MetricManagerTestFixture fixture)
        {
            _fixture = fixture;
            _manager = new DefaultTimerManager(_fixture.Advanced, _fixture.Registry);
        }

        [Fact]
        public void can_decrement_counter_by_amount()
        {
            var metricName = "test_decrement_counter";
            var options = new CounterOptions() { Name = metricName };

            //_manager.Decrement(options, 2L);

            //var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            //data.Contexts.Single().CounterValueFor(metricName).Count.Should().Be(-2L);
        }
    }
}