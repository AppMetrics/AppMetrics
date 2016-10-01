using System;

namespace App.Metrics.Reporters
{
    public class ConsoleReport : HumanReadableReport
    {
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