// <copyright file="TextFileMetricsReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Logging;
using App.Metrics.Reporting.Base;

namespace App.Metrics.Reporting.TextFile
{
    public class TextFileMetricsReporter : BaseMetricsReporter
    {
        private static readonly ILog Logger = LogProvider.For<TextFileMetricsReporter>();
        private static readonly int _defaultBufferSize = 4096;
        private readonly bool _appendMode;
        private readonly string _output = @".\metrics.txt";

        // ReSharper disable UnusedMember.Global
        public TextFileMetricsReporter()
            : base()
            // ReSharper restore UnusedMember.Global
        {
            Logger.Info($"Using Text File Metrics Reporter: {this}. AppendMode: {_appendMode}. Output: {_output}");
        }

        public TextFileMetricsReporter(MetricsReportingTextFileOptions options)
            : base(options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrWhiteSpace(options.OutputPathAndFileName))
            {
                throw new InvalidOperationException($"{nameof(MetricsReportingTextFileOptions.OutputPathAndFileName)} cannot be null or empty");
            }

            _appendMode = options.AppendMetricsToTextFile;
            _output = options.OutputPathAndFileName;

            var fileInfo = new FileInfo(options.OutputPathAndFileName);

            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }

            Logger.Info($"Using Text File Metrics Reporter: {this}. AppendMode: {_appendMode}. Output: {_output}");
        }

        /// <inheritdoc />
        public override async Task<bool> FlushImplAsync(MemoryStream stream, CancellationToken cancellationToken)
        {
            var outputStream = stream.ToArray();

            using (var sourceStream = new FileStream(
                _output,
                _appendMode ? FileMode.Append : FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: _appendMode ? _defaultBufferSize : 4096,
                useAsync: true))
            {
                sourceStream.Seek(0, SeekOrigin.End);
                await sourceStream.WriteAsync(outputStream, 0, outputStream.Length, cancellationToken);
            }

            return true;
        }
    }
}