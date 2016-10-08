using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public class TextFileReport : HumanReadableReport
    {
        private readonly string _fileName;
        private StringBuilder _buffer;

        public TextFileReport(string fileName,
            ILoggerFactory loggerFactory)
            :base(loggerFactory)

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
                Logger.LogError(new EventId(), x, "Error writing text file " + _fileName);
                //TODO: AH - Log internal metric error
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