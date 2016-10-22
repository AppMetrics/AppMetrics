using System;
using System.Threading;
using App.Metrics;

namespace Metrics.Samples
{
    public class UserValueTimerSample
    {
        private readonly ITimer _timer;

        private static IMetricsContext _metricsContext;

        public UserValueTimerSample(IMetricsContext metricsContext)
        {
            _metricsContext = metricsContext;

            _timer = _metricsContext.Advanced.Timer(SampleMetricsRegistry.Timers.Requests);
        }

        public void Process(string documentId)
        {
            using (var context = _timer.NewContext(documentId))
            {
                ActualProcessingOfTheRequest(documentId);

                // if needed elapsed time is available in context.Elapsed 
            }
        }

        public void RunSomeRequests()
        {
            for (var i = 0; i < 30; i++)
            {
                var documentId = new Random().Next(10);
                new UserValueTimerSample(_metricsContext).Process("document-" + documentId.ToString());
            }
        }

        private void ActualProcessingOfTheRequest(string documentId)
        {
            Thread.Sleep(new Random().Next(1000));
        }

        private void LogDuration(TimeSpan time)
        {
        }
    }
}