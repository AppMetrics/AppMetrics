// <copyright file="AsciiEnvironmentInfoFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using App.Metrics.Core.Infrastructure;

namespace App.Metrics.Core.Formatting.Ascii
{
    public class AsciiEnvironmentInfoFormatter
    {
        private const int Padding = 20;
        private EnvironmentInfo _environmentInfo;

        public AsciiEnvironmentInfoFormatter(EnvironmentInfo environmentInfo) { _environmentInfo = environmentInfo; }

        public void Format(TextWriter textWriter)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            textWriter.Write(PaddedFormat("Assembly Name", _environmentInfo.EntryAssemblyName));
            textWriter.Write('\n');
            textWriter.Write(PaddedFormat("Assembly Version", _environmentInfo.EntryAssemblyVersion));
            textWriter.Write('\n');
            textWriter.Write(PaddedFormat("Host Name", _environmentInfo.HostName));
            textWriter.Write('\n');
            textWriter.Write(PaddedFormat("Local Time", _environmentInfo.LocalTimeString));
            textWriter.Write('\n');
            textWriter.Write(PaddedFormat("Machine Name", _environmentInfo.MachineName));
            textWriter.Write('\n');
            textWriter.Write(PaddedFormat("OS", _environmentInfo.OperatingSystem));
            textWriter.Write('\n');
            textWriter.Write(PaddedFormat("OS Version", _environmentInfo.OperatingSystemVersion));
            textWriter.Write('\n');
            textWriter.Write(PaddedFormat("Process Name", _environmentInfo.ProcessName));
            textWriter.Write('\n');
        }

        private static string PaddedFormat(string label, string value, string sign = "=")
        {
            var pad = string.Empty;

            if (label.Length + 2 + sign.Length < Padding)
            {
                pad = new string(' ', Padding - label.Length - 1 - sign.Length);
            }

            return $"{pad}{label} {sign} {value}";
        }
    }
}