using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace App.Metrics
{
    public class MetricsErrorHandler
    {
        private static readonly Meter ErrorMeter = Metric.Internal.Meter("Metrics Errors", Unit.Errors);

        //TODO: AH - Inject logger
        private static readonly ILogger Log = new LoggerFactory().CreateLogger(typeof(MetricsErrorHandler));

        private readonly ConcurrentBag<Action<Exception, string>> handlers = new ConcurrentBag<Action<Exception, string>>();

        private MetricsErrorHandler()
        {
            AddHandler((x, msg) => Log.LogError("Metrics: Unhandled exception in App.Metrics Library {0} {1}", x, msg, x.Message));
            AddHandler((x, msg) => Log.LogError("Metrics: Unhandled exception in App.Metrics Library " + x.ToString()));

            //TODO: AH - Console logging if UserInteractive
            //if (Environment.UserInteractive)
            //{
            //    AddHandler((x, msg) => Console.WriteLine("Metrics: Unhandled exception in App.Metrics Library {0} {1}", msg, x.ToString()));
            //}
        }

        internal static MetricsErrorHandler Handler { get; } = new MetricsErrorHandler();

        public static void Handle(Exception exception)
        {
            Handle(exception, string.Empty);
        }

        public static void Handle(Exception exception, string message)
        {
            Handler.InternalHandle(exception, message);
        }

        internal void AddHandler(Action<Exception> handler)
        {
            AddHandler((x, msg) => handler(x));
        }

        internal void AddHandler(Action<Exception, string> handler)
        {
            handlers.Add(handler);
        }

        internal void ClearHandlers()
        {
            while (!handlers.IsEmpty)
            {
                Action<Exception, string> item;
                handlers.TryTake(out item);
            }
        }

        private void InternalHandle(Exception exception, string message)
        {
            ErrorMeter.Mark();

            foreach (var handler in handlers)
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