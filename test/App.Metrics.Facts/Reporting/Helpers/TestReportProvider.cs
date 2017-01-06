using System;
using App.Metrics.Reporting.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Facts.Reporting.Helpers
{
    public class TestReportProvider : IReporterProvider
    {
        private readonly bool _pass;
        private readonly TimeSpan _reportInterval;
        private readonly Exception _throwEx;

        public TestReportProvider(bool pass, TimeSpan reportInterval, Exception throwEx = null)
        {
            _pass = throwEx == null && pass;
            _reportInterval = reportInterval;
            _throwEx = throwEx;
        }
        public IMetricsFilter Filter { get; set; }

        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory)
        {
            return new TestMetricReporter(_pass, _reportInterval, _throwEx);
        }
    }
}