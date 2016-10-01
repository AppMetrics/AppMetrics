using System;
using System.IO;
using System.Text;

namespace App.Metrics.Reporters
{
    public class TextFileReport : HumanReadableReport
    {
        private readonly string fileName;

        private StringBuilder buffer;

        public TextFileReport(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            this.fileName = fileName;
        }

        protected override void EndReport(string contextName)
        {
            try
            {
                File.WriteAllText(this.fileName, this.buffer.ToString());
            }
            catch (Exception x)
            {
                MetricsErrorHandler.Handle(x, "Error writing text file " + this.fileName);
            }

            base.EndReport(contextName);
            this.buffer = null;
        }

        protected override void StartReport(string contextName)
        {
            this.buffer = new StringBuilder();
            base.StartReport(contextName);
        }

        protected override void WriteLine(string line, params string[] args)
        {
            buffer.AppendFormat(line, args);
            buffer.AppendLine();
        }
    }
}