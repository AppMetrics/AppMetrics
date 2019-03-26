// <copyright file="HealthStatusTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics.Health.Formatters.Ascii.Internal;
using App.Metrics.Health.Serialization;

namespace App.Metrics.Health.Formatters.Ascii
{
    public class HealthStatusTextWriter : IHealthStatusWriter
    {
        private readonly int _padding;
        private readonly string _separator;
        private readonly TextWriter _textWriter;

        public HealthStatusTextWriter(
            TextWriter textWriter,
            string separator = HealthStatusFormatterConstants.OutputFormatting.Separator,
            int padding = HealthStatusFormatterConstants.OutputFormatting.Padding)
        {
            _textWriter = textWriter ?? throw new ArgumentNullException(nameof(textWriter));

            _padding = padding;
            _separator = separator;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Write(HealthStatus healthStatus)
        {
            var status = GetOverallStatus(healthStatus.Results);

            _textWriter.Write($"# OVERALL STATUS: {status}");
            _textWriter.Write("\n--------------------------------------------------------------\n");

            foreach (var result in healthStatus.Results.OrderBy(r => (int)r.Check.Status))
            {
                WriteCheckResult(result);
            }
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                #if NET452
                _textWriter?.Close();
                #endif
                _textWriter?.Dispose();
            }
        }

        private string FormatReadable(string label, string value)
        {
            var pad = string.Empty;

            if (label.Length + 2 + _separator.Length < _padding)
            {
                pad = new string(' ', _padding - label.Length - 1 - _separator.Length);
            }

            return $"{pad}{label} {_separator} {value}";
        }

        private string GetOverallStatus(IEnumerable<HealthCheck.Result> results)
        {
            var status = HealthConstants.DegradedStatusDisplay;

            var enumerable = results as HealthCheck.Result[] ?? results.ToArray();
            var failed = enumerable.Any(c => c.Check.Status == HealthCheckStatus.Unhealthy);
            var degraded = enumerable.Any(c => c.Check.Status == HealthCheckStatus.Degraded);

            if (!degraded && !failed)
            {
                status = HealthConstants.HealthyStatusDisplay;
            }

            if (failed)
            {
                status = HealthConstants.UnhealthyStatusDisplay;
            }

            return status;
        }

        private void WriteCheckResult(HealthCheck.Result checkResult)
        {
            _textWriter.Write("# CHECK: ");
            _textWriter.Write(checkResult.Name);
            _textWriter.Write('\n');
            _textWriter.Write('\n');
            _textWriter.Write(FormatReadable("MESSAGE", checkResult.IsFromCache ? $"[Cached] {checkResult.Check.Message}" : checkResult.Check.Message));
            _textWriter.Write('\n');
            _textWriter.Write(FormatReadable("STATUS", HealthConstants.HealthStatusDisplay[checkResult.Check.Status]));
            _textWriter.Write("\n--------------------------------------------------------------");
            _textWriter.Write('\n');
        }
    }
}