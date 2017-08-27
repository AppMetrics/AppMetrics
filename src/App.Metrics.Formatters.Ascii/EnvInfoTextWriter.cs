// <copyright file="EnvInfoTextWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using App.Metrics.Formatters.Ascii.Internal;
using App.Metrics.Infrastructure;
using App.Metrics.Serialization;

namespace App.Metrics.Formatters.Ascii
{
    public class EnvInfoTextWriter : IEnvInfoWriter
    {
        private readonly int _padding;
        private readonly string _separator;
        private readonly TextWriter _textWriter;

        public EnvInfoTextWriter(
            TextWriter textWriter,
            string separator = MetricsTextFormatterConstants.OutputFormatting.Separator,
            int padding = MetricsTextFormatterConstants.OutputFormatting.Padding)
        {
            _textWriter = textWriter ?? throw new ArgumentNullException(nameof(textWriter));
            _separator = separator;
            _padding = padding;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Write(EnvironmentInfo envInfo)
        {
            _textWriter.Write(PaddedFormat("Assembly Name", envInfo.EntryAssemblyName));
            _textWriter.Write('\n');
            _textWriter.Write(PaddedFormat("Assembly Version", envInfo.EntryAssemblyVersion));
            _textWriter.Write('\n');
            _textWriter.Write(PaddedFormat("Framework Description", envInfo.FrameworkDescription));
            _textWriter.Write('\n');
            _textWriter.Write(PaddedFormat("Local Time", envInfo.LocalTimeString));
            _textWriter.Write('\n');
            _textWriter.Write(PaddedFormat("Machine Name", envInfo.MachineName));
            _textWriter.Write('\n');
            _textWriter.Write(PaddedFormat("OS Architecture", envInfo.OperatingSystemArchitecture));
            _textWriter.Write('\n');
            _textWriter.Write(PaddedFormat("OS Platform", envInfo.OperatingSystemPlatform));
            _textWriter.Write('\n');
            _textWriter.Write(PaddedFormat("OS Version", envInfo.OperatingSystemVersion));
            _textWriter.Write('\n');
            _textWriter.Write(PaddedFormat("Process Architecture", envInfo.ProcessArchitecture));
            _textWriter.Write('\n');
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
                _textWriter?.Dispose();
            }
        }

        private string PaddedFormat(string label, string value)
        {
            var pad = string.Empty;

            if (label.Length + 2 + _separator.Length < _padding)
            {
                pad = new string(' ', _padding - label.Length - 1 - _separator.Length);
            }

            return $"{pad}{label} {_separator} {value}";
        }
    }
}