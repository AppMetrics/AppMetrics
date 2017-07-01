// <copyright file="CustomAsciiHealthCheckResult.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;

namespace App.Metrics.Health.Formatters.Ascii.Facts.TestHelpers
{
    public class CustomAsciiHealthCheckResult
    {
        public CustomAsciiHealthCheckResult(string name, string message, HealthCheckStatus status)
        {
            Name = name;
            Message = message;
            Status = status;
        }

        public string Message { get; }

        public string Name { get; }

        public HealthCheckStatus Status { get; }

        public void Format(TextWriter textWriter)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            textWriter.Write(Name);
            textWriter.Write(' ');
            textWriter.Write(Message);
            textWriter.Write(' ');
            textWriter.Write(Status.ToString());
        }
    }
}