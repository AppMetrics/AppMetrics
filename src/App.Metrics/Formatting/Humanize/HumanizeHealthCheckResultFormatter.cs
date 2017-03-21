// <copyright file="HumanizeHealthCheckResultFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Text;
using App.Metrics.Health;

namespace App.Metrics.Formatting.Humanize
{
    public sealed class HumanizeHealthCheckResultFormatter : ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (!(arg is HealthCheck.Result))
            {
                return arg.ToString();
            }

            var healthCheckResult = (HealthCheck.Result)arg;

            var sb = new StringBuilder();
            sb.AppendLine();

            if (healthCheckResult.Check.Status.IsUnhealthy())
            {
                sb.AppendLine(healthCheckResult.Name.FormatReadableMetricValue("FAILED: " + healthCheckResult.Check.Message));
                return sb.ToString();
            }

            if (healthCheckResult.Check.Status.IsHealthy())
            {
                sb.AppendLine(
                    "\t" + Environment.NewLine + healthCheckResult.Name.FormatReadableMetricValue("PASSED: " + healthCheckResult.Check.Message));
                return sb.ToString();
            }

            sb.AppendLine(healthCheckResult.Name.FormatReadableMetricValue("DEGRADED: " + healthCheckResult.Check.Message));
            return sb.ToString();
        }
    }
}