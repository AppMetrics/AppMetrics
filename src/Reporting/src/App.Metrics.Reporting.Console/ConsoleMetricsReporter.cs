// <copyright file="ConsoleMetricsReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Logging;
using App.Metrics.Reporting.Base;

using static System.Console;

namespace App.Metrics.Reporting.Console
{
    public class ConsoleMetricsReporter : BaseMetricsReporter
    {
        private static readonly ILog Logger = LogProvider.For<ConsoleMetricsReporter>();

        // ReSharper disable UnusedMember.Global
        public ConsoleMetricsReporter()
            : base()
            // ReSharper restore UnusedMember.Global
        {
            Logger.Info($"Using Console Metrics Reporter: {this}");
        }

        public ConsoleMetricsReporter(MetricsReportingConsoleOptions options)
            : base(options)
        {
            Logger.Info($"Using Console Metrics Reporter: {this}");
        }

        public override async Task<bool> FlushImplAsync(MemoryStream stream, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var array = stream.ToArray();
            var output = Encoding.UTF8.GetString(array);
            WriteLine(output);
            return true;
        }
    }
}