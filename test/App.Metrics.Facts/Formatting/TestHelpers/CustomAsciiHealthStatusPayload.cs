// <copyright file="CustomAsciiHealthStatusPayload.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics.Core.Internal;
using App.Metrics.Health;

namespace App.Metrics.Facts.Formatting.TestHelpers
{
    public class CustomAsciiHealthStatusPayload
    {
        private readonly List<CustomAsciiHealthCheckResult> _results = new List<CustomAsciiHealthCheckResult>();

        public void Add(CustomAsciiHealthCheckResult result)
        {
            if (result == null)
            {
                return;
            }

            _results.Add(result);
        }

        public void Format(TextWriter textWriter)
        {
            if (textWriter == null)
            {
                return;
            }

            var results = _results.ToList();

            var status = GetOverallStatus(results);

            textWriter.Write($"Overall: {status}");
            textWriter.Write('\n');

            foreach (var result in results)
            {
                result.Format(textWriter);
                textWriter.Write('\n');
            }
        }

        private static string GetOverallStatus(IReadOnlyCollection<CustomAsciiHealthCheckResult> results)
        {
            var status = Constants.Health.DegradedStatusDisplay;

            var failed = results.Any(c => c.Status == HealthCheckStatus.Unhealthy);
            var degraded = results.Any(c => c.Status == HealthCheckStatus.Degraded);

            if (!degraded && !failed)
            {
                status = Constants.Health.HealthyStatusDisplay;
            }

            if (failed)
            {
                status = Constants.Health.UnhealthyStatusDisplay;
            }

            return status;
        }
    }
}