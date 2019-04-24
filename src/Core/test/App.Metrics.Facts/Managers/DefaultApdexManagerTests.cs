// <copyright file="DefaultApdexManagerTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Internal.NoOp;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultApdexManagerTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly MetricCoreTestFixture _fixture;
        private readonly IMeasureApdexMetrics _manager;

        public DefaultApdexManagerTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _manager = _fixture.Managers.Apdex;
        }

        [Fact]
        public void Track_action_adds_multidimensional_metrics_to_registry()
        {
            var metricName = "test_apdex_multi";

            var options = new ApdexOptions { Name = metricName };

            _manager.Track(options, _fixture.Tags[0], async () => { await Task.Delay(10); });
            _manager.Track(options, _fixture.Tags[1], async () => { await Task.Delay(10); });

            var data = _fixture.Registry.GetData(new NullMetricsFilter());

            data.Contexts.Single().ApdexScores.FirstOrDefault(x => x.Name == _fixture.Tags[0].AsMetricName(metricName)).Should().NotBeNull();
            data.Contexts.Single().ApdexScores.FirstOrDefault(x => x.Name == _fixture.Tags[1].AsMetricName(metricName)).Should().NotBeNull();
        }

        [Fact]
        public void Track_action_adds_to_registry()
        {
            var metricName = "test_apdex";
            var options = new ApdexOptions { Name = metricName };

            _manager.Track(options, async () => { await Task.Delay(10); });

            var data = _fixture.Registry.GetData(new NullMetricsFilter());

            data.Contexts.Single().ApdexScores.FirstOrDefault(x => x.Name == metricName).Should().NotBeNull();
        }

        [Fact]
        public async Task Track_adds_multidimensional_metrics_to_registry()
        {
            var metricName = "test_apdex_no_action_multi";
            var options = new ApdexOptions { Name = metricName };

            using (_manager.Track(options, _fixture.Tags[0]))
            {
                await Task.Delay(10);
            }

            using (_manager.Track(options, _fixture.Tags[1]))
            {
                await Task.Delay(10);
            }

            var data = _fixture.Registry.GetData(new NullMetricsFilter());

            data.Contexts.Single().ApdexScores.FirstOrDefault(x => x.Name == _fixture.Tags[0].AsMetricName(metricName)).Should().NotBeNull();
            data.Contexts.Single().ApdexScores.FirstOrDefault(x => x.Name == _fixture.Tags[1].AsMetricName(metricName)).Should().NotBeNull();
        }

        [Fact]
        public async Task Track_adds_to_registry()
        {
            var metricName = "test_apdex_no_action";
            var options = new ApdexOptions { Name = metricName };

            using (_manager.Track(options))
            {
                await Task.Delay(10);
            }

            var data = _fixture.Registry.GetData(new NullMetricsFilter());

            data.Contexts.Single().ApdexScores.FirstOrDefault(x => x.Name == metricName).Should().NotBeNull();
        }
    }
}