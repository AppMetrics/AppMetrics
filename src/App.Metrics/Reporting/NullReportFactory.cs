using App.Metrics.Scheduling;

namespace App.Metrics.Reporting
{
    internal sealed class NullReportFactory : IReportFactory
    {
        public void Dispose()
        {
        }

        public void AddProvider(IReporterProvider provider)
        {
        }

        public IReporter CreateReporter(IScheduler scheduler)
        {
            return new NullReporter();
        }

        public IReporter CreateReporter()
        {
            return new NullReporter();
        }
    }
}