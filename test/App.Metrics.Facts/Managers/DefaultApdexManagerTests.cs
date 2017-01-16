// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
    public class DefaultApdexManagerTests : IClassFixture<MetricManagerTestFixture>
    {
        private readonly MetricManagerTestFixture _fixture;
        private readonly IMeasureApdexMetrics _manager;

        public DefaultApdexManagerTests(MetricManagerTestFixture fixture)
        {
            _fixture = fixture;
            _manager = new DefaultApdexManager(_fixture.Advanced, _fixture.Registry);
        }

        [Fact]
        public void track_action_adds_to_registry()
        {
            var metricName = "test_apdex";
            var options = new ApdexOptions { Name = metricName };

            _manager.Track(options, () => { Task.Delay(10); });

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().ApdexScores.FirstOrDefault(x => x.Name == metricName).Should().NotBeNull();
        }

        [Fact]
        public void track_adds_to_registry()
        {
            var metricName = "test_apdex_no_action";
            var options = new ApdexOptions { Name = metricName };

            using (_manager.Track(options))
            {
                Task.Delay(10);
            }

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().ApdexScores.FirstOrDefault(x => x.Name == metricName).Should().NotBeNull();
        }
    }
}