// <copyright file="EnvInfoTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Formatters.Ascii.Internal;
using App.Metrics.Infrastructure;
using App.Metrics.Serialization;
using System;
using System.IO;
using System.Threading.Tasks;

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
        public async ValueTask Write(EnvironmentInfo envInfo)
        {
            await _textWriter.WriteAsync(PaddedFormat("Assembly Name", envInfo.EntryAssemblyName));
            await _textWriter.WriteAsync('\n');
            await _textWriter.WriteAsync(PaddedFormat("Assembly Version", envInfo.EntryAssemblyVersion));
            await _textWriter.WriteAsync('\n');
            await _textWriter.WriteAsync(PaddedFormat("Framework Description", envInfo.FrameworkDescription));
            await _textWriter.WriteAsync('\n');
            await _textWriter.WriteAsync(PaddedFormat("Local Time", envInfo.LocalTimeString));
            await _textWriter.WriteAsync('\n');
            await _textWriter.WriteAsync(PaddedFormat("Machine Name", envInfo.MachineName));
            await _textWriter.WriteAsync('\n');
            await _textWriter.WriteAsync(PaddedFormat("OS Architecture", envInfo.OperatingSystemArchitecture));
            await _textWriter.WriteAsync('\n');
            await _textWriter.WriteAsync(PaddedFormat("OS Platform", envInfo.OperatingSystemPlatform));
            await _textWriter.WriteAsync('\n');
            await _textWriter.WriteAsync(PaddedFormat("OS Version", envInfo.OperatingSystemVersion));
            await _textWriter.WriteAsync('\n');
            await _textWriter.WriteAsync(PaddedFormat("Process Architecture", envInfo.ProcessArchitecture));
            await _textWriter.WriteAsync('\n');
        }

#if NETSTANDARD2_1
        public async ValueTask DisposeAsync()
        {
            await (_textWriter?.DisposeAsync() ?? default);
        }
#else
        public ValueTask DisposeAsync() 
        {
            _textWriter?.Dispose();
            return default;
        }
#endif

        public void Dispose()
        {
            _textWriter?.Dispose();
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