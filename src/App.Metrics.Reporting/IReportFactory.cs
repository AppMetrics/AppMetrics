using System;

namespace App.Metrics.Reporting
{
    public interface IReportFactory : IDisposable
    {
        IReporter CreateReporter(string name);

        void AddProvider(IReporterProvider provider);
    }
}