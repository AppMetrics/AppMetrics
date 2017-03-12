using System;
using BenchmarkDotNet.Loggers;
using Xunit.Abstractions;

namespace App.Metrics.Benchmarks.Support
{
    public class OutputLogger : AccumulationLogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private string _currentLine = "";

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
            _currentLine = "";
            base.WriteLine();
        }

        public override void WriteLine(LogKind logKind, string text)
        {
            _testOutputHelper.WriteLine(_currentLine + RemoveInvalidChars(text));
            _currentLine = "";
            base.WriteLine(logKind, text);
        }

        #region Xunit bug workaround

        /// <summary>
        ///     Workaround for xunit bug: https://github.com/xunit/xunit/issues/876
        /// </summary>
        private static string RemoveInvalidChars(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace((char)0x1B, ' ');
        }

        #endregion
    }
}