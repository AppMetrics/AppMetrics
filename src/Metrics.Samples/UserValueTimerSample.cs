
using System;
using System.Threading;
using App.Metrics;

namespace Metrics.Samples
{
    public class UserValueTimerSample
    {
        private readonly App.Metrics.Timer timer =
            Metric.Timer("Requests", Unit.Requests);

        public void Process(string documentId)
        {
            using (var context = timer.NewContext(documentId))
            {
                ActualProcessingOfTheRequest(documentId);

                // if needed elapsed time is available in context.Elapsed 
            }
        }

        private void LogDuration(TimeSpan time)
        {
        }

        private void ActualProcessingOfTheRequest(string documentId)
        {
            Thread.Sleep(new Random().Next(1000));
        }

        public static void RunSomeRequests()
        {
            for (int i = 0; i < 30; i++)
            {
                var documentId = new Random().Next(10);
                new UserValueTimerSample().Process("document-" + documentId.ToString());
            }
        }
    }
}
