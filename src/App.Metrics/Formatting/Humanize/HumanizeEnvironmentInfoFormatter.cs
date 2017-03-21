// <copyright file="HumanizeEnvironmentInfoFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Text;
using App.Metrics.Infrastructure;

namespace App.Metrics.Formatting.Humanize
{
    public sealed class HumanizeEnvironmentInfoFormatter : ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (!(arg is EnvironmentInfo))
            {
                return arg.ToString();
            }

            var environmentInfo = (EnvironmentInfo)arg;

            var sb = new StringBuilder();

            sb.AppendLine("Assembly Name".FormatReadableMetricValue(environmentInfo.EntryAssemblyName));
            sb.AppendLine("Assembly Version".FormatReadableMetricValue(environmentInfo.EntryAssemblyVersion));
            sb.AppendLine("Host Name".FormatReadableMetricValue(environmentInfo.HostName));
            sb.AppendLine("Local Time".FormatReadableMetricValue(environmentInfo.LocalTimeString));
            sb.AppendLine("Machine Name".FormatReadableMetricValue(environmentInfo.MachineName));
            sb.AppendLine("OS".FormatReadableMetricValue(environmentInfo.OperatingSystem));
            sb.AppendLine("OS Version".FormatReadableMetricValue(environmentInfo.OperatingSystemVersion));
            sb.AppendLine("Process Name".FormatReadableMetricValue(environmentInfo.ProcessName));

            return sb.ToString();
        }
    }
}