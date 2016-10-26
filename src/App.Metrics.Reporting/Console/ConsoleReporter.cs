using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Internal;
using App.Metrics.MetricData;
using App.Metrics.Reporting._Legacy;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace App.Metrics.Reporting.Console
{
    #region old reporter

    public class ReadableReporter : BaseReporter
    {
        private readonly int _padding = 20;
        private bool _disposed = false;

        public ReadableReporter(ILoggerFactory loggerFactory,
            IMetricsFilter filter,
            IClock clock)
            : base(loggerFactory, clock, filter)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }        }

        public void WriteValue(string label, string value, string sign = "=")
        {
            var pad = string.Empty;

            if (label.Length + 2 + sign.Length < _padding)
            {
                pad = new string(' ', _padding - label.Length - 1 - sign.Length);
            }

            WriteLine("{0}{1} {2} {3}", pad, label, sign, value);
        }

        public void WriteLine(string line, params string[] args)
        {
            System.Console.WriteLine(line, args);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Release managed resources
                }

                // Release unmanaged resources.
                // Set large fields to null.
                _disposed = true;
            }

            base.Dispose(disposing);
        }

        public override void ReportCounter(string name, CounterValue value, Unit unit, MetricTags tags)
        {
            WriteMetricName(name);
            WriteValue("Count", unit.FormatCount(value.Count));
            if (value.Items.Length > 0)
            {
                WriteValue("Total Items", value.Items.Length.ToString());
            }
            for (var i = 0; i < value.Items.Length; i++)
            {
                var key = "Item " + i.ToString();
                var item = value.Items[i];
                var val = $"{item.Percent:00.00}% {item.Count,5} {unit.Name} [{item.Item}]";
                WriteValue(key, val);
            }
        }

        public override void ReportGauge(string name, double value, Unit unit, MetricTags tags)
        {
            WriteMetricName(name);
            WriteValue("value", unit.FormatValue(value));
        }

        public override void ReportHealth(HealthStatus status)
        {
            WriteLine();
            WriteValue("Is Healthy", status.IsHealthy ? "Yes" : "No");
            WriteLine();

            var unhealthy = status.Results.Where(r => !r.Check.IsHealthy);
            if (unhealthy.Any())
            {
                WriteMetricName("FAILED CHECKS");
                WriteLine();
                foreach (var result in unhealthy)
                {
                    WriteValue(result.Name, "FAILED: " + result.Check.Message);
                }
            }

            var healthy = status.Results.Where(r => r.Check.IsHealthy);
            if (healthy.Any())
            {
                WriteMetricName("PASSED CHECKS");
                WriteLine();
                foreach (var result in healthy)
                {
                    WriteValue(result.Name, "PASSED: " + result.Check.Message);
                }
            }
        }

        public override void ReportHistogram(string name, HistogramValue value, Unit unit, MetricTags tags)
        {
            WriteMetricName(name);
            WriteHistogram(value, unit);
        }

        public override void ReportMeter(string name, MeterValue value, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            WriteMetricName(name);
            WriteMeter(value, unit, rateUnit);

            if (value.Items.Length > 0)
            {
                WriteValue("Total Items", value.Items.Length.ToString());
            }
            for (var i = 0; i < value.Items.Length; i++)
            {
                var key = "Item " + i.ToString();
                var item = value.Items[i];
                var val = $"{item.Percent:00.00}% {item.Value.Count,5} {unit.Name} [{item.Item}]";
                WriteValue(key, val);
                WriteMeter(item.Value, unit, rateUnit);
            }
        }

        public override void ReportTimer(string name, TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            WriteMetricName(name);
            WriteValue("Active Sessions", value.ActiveSessions.ToString());
            WriteValue("Total Time", unit.FormatDuration(value.TotalTime, durationUnit));
            WriteMeter(value.Rate, unit, rateUnit);
            WriteHistogram(value.Histogram, unit, durationUnit);
        }

        public override void StartMetricGroup(string metricType)
        {
            WriteLine();
            WriteLine();
            WriteLine("***** {0} - {1} *****", metricType, Clock.FormatTimestamp(CurrentContextTimestamp));

            base.StartMetricGroup(metricType);
        }

        public override void StartReport(string contextName)
        {
            WriteLine("{0} - {1}", contextName, Clock.FormatTimestamp(ReportTimestamp));

            base.StartReport(contextName);
        }

        public void WriteMetricName(string name)
        {
            WriteLine();
            WriteLine("    {0}", name);
        }

        private void WriteHistogram(HistogramValue value, Unit unit, TimeUnit? durationUnit = null)
        {
            WriteValue("Count", unit.FormatCount(value.Count));
            WriteValue("Last", unit.FormatDuration(value.LastValue, durationUnit));

            if (!string.IsNullOrWhiteSpace(value.LastUserValue))
            {
                WriteValue("Last User Value", value.LastUserValue);
            }

            WriteValue("Min", unit.FormatDuration(value.Min, durationUnit));

            if (!string.IsNullOrWhiteSpace(value.MinUserValue))
            {
                WriteValue("Min User Value", value.MinUserValue);
            }

            WriteValue("Max", unit.FormatDuration(value.Max, durationUnit));

            if (!string.IsNullOrWhiteSpace(value.MaxUserValue))
            {
                WriteValue("Max User Value", value.MaxUserValue);
            }

            WriteValue("Mean", unit.FormatDuration(value.Mean, durationUnit));
            WriteValue("StdDev", unit.FormatDuration(value.StdDev, durationUnit));
            WriteValue("Median", unit.FormatDuration(value.Median, durationUnit));
            WriteValue("75%", unit.FormatDuration(value.Percentile75, durationUnit), sign: "<=");
            WriteValue("95%", unit.FormatDuration(value.Percentile95, durationUnit), sign: "<=");
            WriteValue("98%", unit.FormatDuration(value.Percentile98, durationUnit), sign: "<=");
            WriteValue("99%", unit.FormatDuration(value.Percentile99, durationUnit), sign: "<=");
            WriteValue("99.9%", unit.FormatDuration(value.Percentile999, durationUnit), sign: "<=");
        }

        private void WriteLine()
        {
            WriteLine(string.Empty);
        }

        private void WriteMeter(MeterValue value, Unit unit, TimeUnit rateUnit)
        {
            WriteValue("Count", unit.FormatCount(value.Count));
            WriteValue("Mean Value", unit.FormatRate(value.MeanRate, rateUnit));
            WriteValue("1 Minute Rate", unit.FormatRate(value.OneMinuteRate, rateUnit));
            WriteValue("5 Minute Rate", unit.FormatRate(value.FiveMinuteRate, rateUnit));
            WriteValue("15 Minute Rate", unit.FormatRate(value.FifteenMinuteRate, rateUnit));
        }
    }

    #endregion

    public class ConsoleReporter : IReporter
    {
        private ReadableReporter _reporter;
        public ConsoleReporter(string name, TimeSpan interval,
            bool isEnabled, IMetricsFilter filter)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Filter = filter ?? new NoOpFilter();
            Interval = interval;
            IsEnabled = isEnabled;
            _reporter = new ReadableReporter(new LoggerFactory(), filter, Clock.Default);

        }

        public string Name { get; }

        public TimeSpan Interval { get; }

        public IMetricsFilter Filter { get;  }

        public bool IsEnabled { get; }

        public async Task RunReports(IMetricsContext context, CancellationToken token)
        {
            var data = await context.Advanced.DataManager.GetMetricsDataAsync();
            _reporter.RunReport(context, token);
        }

    }
}