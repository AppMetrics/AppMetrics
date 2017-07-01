// <copyright file="AsciiHealthCheckResult.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;

namespace App.Metrics.Health.Formatters.Ascii
{
    public class AsciiHealthCheckResult
    {
        public AsciiHealthCheckResult(string name, string message, HealthCheckStatus status)
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

            textWriter.Write("# CHECK: ");
            textWriter.Write(Name);
            textWriter.Write('\n');
            textWriter.Write('\n');
            textWriter.Write(AsciiSyntax.FormatReadable("MESSAGE", Message));
            textWriter.Write('\n');
            textWriter.Write(AsciiSyntax.FormatReadable("STATUS", Status.ToString()));

            textWriter.Write("\n--------------------------------------------------------------");
        }
    }
}