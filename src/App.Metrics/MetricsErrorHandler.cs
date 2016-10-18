using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace App.Metrics
{
    public class MetricsErrorHandler
    {
        private readonly IMeter _errorMeter;
        private readonly ConcurrentBag<Action<Exception, string>> _handlers = new ConcurrentBag<Action<Exception, string>>();
        private readonly ILogger _logger;

        public MetricsErrorHandler(ILoggerFactory loggerFactory, IMetricsContext metricsContext)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            _logger = loggerFactory.CreateLogger<MetricsErrorHandler>();

            AddHandler((x, msg) => _logger.LogError("Metrics: Unhandled exception in App.Metrics Library {0} {1}", x, msg, x.Message));
            AddHandler((x, msg) => _logger.LogError("Metrics: Unhandled exception in App.Metrics Library " + x.ToString()));

            //TODO: Review enableing internal metrics
            //if (Environment.UserInteractive)
            //{
            //    AddHandler((x, msg) => Console.WriteLine("Metrics: Unhandled exception in App.Metrics Library {0} {1}", msg, x.ToString()));
            //}
        }

        public void Handle(Exception exception)
        {
            Handle(exception, string.Empty);
        }

        public void Handle(Exception exception, string message)
        {
            InternalHandle(exception, message);
        }

        internal void AddHandler(Action<Exception> handler)
        {
            AddHandler((x, msg) => handler(x));
        }

        internal void AddHandler(Action<Exception, string> handler)
        {
            _handlers.Add(handler);
        }

        internal void ClearHandlers()
        {
            while (!_handlers.IsEmpty)
            {
                Action<Exception, string> item;
                _handlers.TryTake(out item);
            }
        }

        private void InternalHandle(Exception exception, string message)
        {
            _errorMeter.Mark();

            foreach (var handler in _handlers)
            {
                try
                {
                    handler(exception, message);
                }
                catch
                {
                    // error handler throw-ed on us, hope you have a debugger attached.
                }
            }
        }
    }
}