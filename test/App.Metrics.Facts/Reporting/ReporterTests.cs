// <copyright file="ReporterTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.Formatters;
using App.Metrics.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Reporting
{
    public class ReporterTests : IClassFixture<MetricsReportingFixture>
    {
        private readonly MetricsReporterCollection _reporters;
        private readonly MetricsReportingFixture _fixture;

        public ReporterTests(MetricsReportingFixture fixture)
        {
            _fixture = fixture;
            _reporters = new MetricsReporterCollection();
        }

        [Fact]
        public void Can_generate_report_successfully()
        {
            var reporter = new DefaultMetricsReportRunner(_fixture.Metrics(), _reporters);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.RunAllAsync(token.Token); };

            action.Should().NotThrow();
        }

        [Fact]
        public void Metrics_are_required()
        {
            Action action = () =>
            {
                var unused = new DefaultMetricsReportRunner(null, _reporters);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Reporter_cannot_be_null()
        {
            Action action = () =>
            {
                var unused = new DefaultMetricsReportRunner(_fixture.Metrics(), null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_metric_report_fails_should_not_throw()
        {
            var metricsReporter = new TestReporter(TimeSpan.FromMilliseconds(10), false, new Exception());
            var metrics = _fixture.Metrics();

            var reporters = new MetricsReporterCollection { metricsReporter };
            var reportRunner = new DefaultMetricsReportRunner(metrics, reporters);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reportRunner.RunAllAsync(token.Token); };

            action.Should().NotThrow();
        }

        [Fact]
        public void When_metric_reporter_fails_continues_to_retry()
        {
            var metricsReporter = new TestReporter(TimeSpan.FromMilliseconds(10), false);
            var metrics = _fixture.Metrics();
            var reporters = new MetricsReporterCollection { metricsReporter };
            var reporter = new DefaultMetricsReportRunner(metrics, reporters);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.RunAllAsync(token.Token); };

            action.Should().NotThrow();
        }

        [Fact]
        public void When_null_reporters_doest_throw()
        {
            var metrics = _fixture.Metrics();
            var reporter = new DefaultMetricsReportRunner(metrics, _reporters);

            reporter.RunAllAsync(CancellationToken.None);
        }
    }
}