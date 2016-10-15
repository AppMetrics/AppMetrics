using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public sealed class TextFileReport : HumanReadableReport
    {
        private readonly string _fileName;
        private StringBuilder _buffer;
        private bool _disposed = false;

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
                //TODO: Review enableing internal metrics
            }

            base.EndReport(contextName);
            _buffer = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Release managed resources                    
                }

                // Release unmanaged resources.
                // Set large fields to null.
                _buffer = null;
                _disposed = true;
            }

            base.Dispose(disposing);
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