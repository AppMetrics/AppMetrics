using System;

namespace App.Metrics.Reporting
{
    public interface IReporterProvider : IDisposable
    {
        IReporter CreateReporter(string name);
    }
}