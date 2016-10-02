using System;
using System.Diagnostics;

namespace App.Metrics
{
    public class AppMetricsOptions
    {
        public AppMetricsOptions()
        {
            GlobalContextName = $@"{CleanName(Environment.MachineName)}.{CleanName(Process.GetCurrentProcess().ProcessName)}";
            CompletelyDisableMetrics = false;
        }

        public bool CompletelyDisableMetrics { get; set; }

        public string GlobalContextName { get; set; }

        private static string CleanName(string name)
        {
            return name.Replace('.', '_');
        }
    }
}