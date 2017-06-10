// <copyright file="OutputLogger.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using BenchmarkDotNet.Loggers;
using Xunit.Abstractions;

namespace App.Metrics.Benchmarks.Support
{
    public class OutputLogger : AccumulationLogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private string _currentLine = string.Empty;

        public OutputLogger(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
        }

        public override void Write(LogKind logKind, string text)
        {
            _currentLine += RemoveInvalidChars(text);
            base.Write(logKind, text);
        }

        public override void WriteLine()
        {
            _testOutputHelper.WriteLine(_currentLine);
            _currentLine = string.Empty;
            base.WriteLine();
        }

        public override void WriteLine(LogKind logKind, string text)
        {
            _testOutputHelper.WriteLine(_currentLine + RemoveInvalidChars(text));
            _currentLine = string.Empty;
            base.WriteLine(logKind, text);
        }

        private static string RemoveInvalidChars(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace((char)0x1B, ' ');
        }
    }
}