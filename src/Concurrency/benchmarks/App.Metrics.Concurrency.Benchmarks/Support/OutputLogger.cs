// <copyright file="OutputLogger.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using BenchmarkDotNet.Loggers;
using Xunit.Abstractions;

namespace App.Metrics.Concurrency.Benchmarks.Support
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

        /// <summary>
        ///     Workaround for xunit bug: https://github.com/xunit/xunit/issues/876
        /// </summary>
        /// <param name="text">string to remove invalid chars</param>
        /// <returns>string without invalid chars</returns>
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