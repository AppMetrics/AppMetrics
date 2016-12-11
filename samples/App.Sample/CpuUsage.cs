using System;
using System.Diagnostics;

namespace App.Sample
{
    public class CpuUsage
    {
        public static DateTime StartTime = DateTime.UtcNow;
        private static DateTime _lastMonitorTime = DateTime.UtcNow;
        private static TimeSpan _oldCpuTime = new TimeSpan(0);
        private static TimeSpan _start;

        public double CpuUsageLastMinute { get; private set; }

        public double CpuUsageTotal { get; private set; }

        public void CallCpu()
        {
            var newCpuTime = Process.GetCurrentProcess().TotalProcessorTime - _start;
            CpuUsageLastMinute = (newCpuTime - _oldCpuTime).TotalSeconds /
                                 (Environment.ProcessorCount * DateTime.UtcNow.Subtract(_lastMonitorTime).TotalSeconds);
            _lastMonitorTime = DateTime.UtcNow;
            CpuUsageTotal = newCpuTime.TotalSeconds / (Environment.ProcessorCount * DateTime.UtcNow.Subtract(StartTime).TotalSeconds);
            _oldCpuTime = newCpuTime;
        }

        public void Start()
        {
            _start = Process.GetCurrentProcess().TotalProcessorTime;
        }
    }
}