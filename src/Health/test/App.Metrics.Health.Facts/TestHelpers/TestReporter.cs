// <copyright file="TestReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Facts.TestHelpers
{
    public class TestReporter : IReportHealthStatus
    {
        private readonly bool _pass;
        private readonly Exception _throwEx;

        public TestReporter()
        {
        }

        public TestReporter(bool pass = true, Exception throwEx = null)
        {
            _pass = throwEx == null && pass;
            _throwEx = throwEx;
            ReportInterval = TimeSpan.FromMilliseconds(10);
        }

        public TestReporter(TimeSpan interval, bool pass = true, Exception throwEx = null)
        {
            ReportInterval = interval;
            _pass = throwEx == null && pass;
            _throwEx = throwEx;
        }

        public TimeSpan ReportInterval { get; set; }

        /// <inheritdoc />
        public Task ReportAsync(HealthOptions options, HealthStatus status, CancellationToken cancellationToken = default)
        {
            if (_throwEx != null)
            {
                throw _throwEx;
            }

            return Task.FromResult(_pass);
        }
    }
}