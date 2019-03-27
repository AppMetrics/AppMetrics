// <copyright file="UserValueTimerSample.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using App.Metrics;
using App.Metrics.Timer;

namespace ReportingSandbox.JustForTesting
{
    public class UserValueTimerSample
    {
        private readonly IMetrics _metrics;
        private readonly ITimer _timer;

        public UserValueTimerSample(IMetrics metrics)
        {
            _metrics = metrics;
            _timer = metrics.Provider.Timer.Instance(SampleMetricsRegistry.Timers.Requests);
        }

        public void Run()
        {
            for (var i = 0; i < 30; i++)
            {
                var documentId = new Random().Next(10);
                new UserValueTimerSample(_metrics).Process("document-" + documentId);
            }
        }

        private static void ActualProcessingOfTheRequest() { Thread.Sleep(new Random().Next(1000)); }

        private void Process(string documentId)
        {
            using (_timer.NewContext(documentId))
            {
                ActualProcessingOfTheRequest();
            }
        }
    }
}