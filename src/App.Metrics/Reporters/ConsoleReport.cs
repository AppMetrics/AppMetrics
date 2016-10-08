using System;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public class ConsoleReport : HumanReadableReport
    {
        public ConsoleReport(ILoggerFactory loggerFactory) 
            : base(loggerFactory)
        {
            
        }
        protected override void StartReport(string contextName)
        {
            Console.Clear();
        }

        protected override void WriteLine(string line, params string[] args)
        {
            Console.WriteLine(line, args);
        }
    }
}