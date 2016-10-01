using System;
using System.IO;
using System.Text;

namespace App.Metrics.Reporters
{
    public class TextFileReport : HumanReadableReport
    {
        private readonly string _fileName;

        private StringBuilder _buffer;

        public TextFileReport(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            _fileName = fileName;
        }

        protected override void EndReport(string contextName)
        {
            try
            {
                File.WriteAllText(_fileName, _buffer.ToString());
            }
            catch (Exception x)
            {
                MetricsErrorHandler.Handle(x, "Error writing text file " + _fileName);
            }

            base.EndReport(contextName);
            _buffer = null;
        }

        protected override void StartReport(string contextName)
        {
            _buffer = new StringBuilder();
            base.StartReport(contextName);
        }

        protected override void WriteLine(string line, params string[] args)
        {
            _buffer.AppendFormat(line, args);
            _buffer.AppendLine();
        }
    }
}