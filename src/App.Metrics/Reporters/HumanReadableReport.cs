using System.Linq;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public abstract class HumanReadableReport : BaseReport
    {
        private readonly int padding;

        protected HumanReadableReport(int padding = 20)
        {
            this.padding = padding;
        }

        public void WriteValue(string label, string value, string sign = "=")
        {
            string pad = string.Empty;

            if (label.Length + 2 + sign.Length < padding)
            {
                pad = new string(' ', padding - label.Length - 1 - sign.Length);
            }

            this.WriteLine("{0}{1} {2} {3}", pad, label, sign, value);
        }

        protected abstract void WriteLine(string line, params string[] args);

        protected override void ReportCounter(string name, CounterValue value, Unit unit, MetricTags tags)
        {
            this.WriteMetricName(name);
            WriteValue("Count", unit.FormatCount(value.Count));
            if (value.Items.Length > 0)
            {
                WriteValue("Total Items", value.Items.Length.ToString());
            }
            for (int i = 0; i < value.Items.Length; i++)
            {
                var key = "Item " + i.ToString();
                var item = value.Items[i];
                var val = $"{item.Percent:00.00}% {item.Count,5} {unit.Name} [{item.Item}]";
                WriteValue(key, val);
            }
        }

        protected override void ReportGauge(string name, double value, Unit unit, MetricTags tags)
        {
            this.WriteMetricName(name);
            this.WriteValue("value", unit.FormatValue(value));
        }

        protected override void ReportHealth(HealthStatus status)
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

        protected override void ReportHistogram(string name, HistogramValue value, Unit unit, MetricTags tags)
        {
            this.WriteMetricName(name);
            this.WriteHistogram(value, unit);
        }

        protected override void ReportMeter(string name, MeterValue value, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            this.WriteMetricName(name);
            this.WriteMeter(value, unit, rateUnit);

            if (value.Items.Length > 0)
            {
                WriteValue("Total Items", value.Items.Length.ToString());
            }
            for (int i = 0; i < value.Items.Length; i++)
            {
                var key = "Item " + i.ToString();
                var item = value.Items[i];
                var val = $"{item.Percent:00.00}% {item.Value.Count,5} {unit.Name} [{item.Item}]";
                WriteValue(key, val);
                WriteMeter(item.Value, unit, rateUnit);
            }
        }

        protected override void ReportTimer(string name, TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            this.WriteMetricName(name);
            this.WriteValue("Active Sessions", value.ActiveSessions.ToString());
            this.WriteValue("Total Time", unit.FormatDuration(value.TotalTime, durationUnit));
            this.WriteMeter(value.Rate, unit, rateUnit);
            this.WriteHistogram(value.Histogram, unit, durationUnit);
        }

        protected override void StartMetricGroup(string metricType)
        {
            this.WriteLine();
            this.WriteLine();
            this.WriteLine("***** {0} - {1} *****", metricType, Clock.FormatTimestamp(this.CurrentContextTimestamp));
        }

        protected override void StartReport(string contextName)
        {
            this.WriteLine("{0} - {1}", contextName, Clock.FormatTimestamp(this.ReportTimestamp));
        }

        protected void WriteMetricName(string name)
        {
            this.WriteLine();
            this.WriteLine("    {0}", name);
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
            this.WriteLine(string.Empty);
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
}