using System;
using App.Metrics;

namespace Metrics.Samples
{
    public class SetCounterSample
    {
        private readonly ICounter _commandCounter;
        private static IMetricsContext _metricsContext;

        public SetCounterSample(IMetricsContext metricsContext)
        {
            _metricsContext = metricsContext;

            _commandCounter = _metricsContext.Counter("Command Counter", Unit.Custom("Commands"));
        }

        public void Process(Command command)
        {
            
            _commandCounter.Increment(command.GetType().Name);
        }

        public void RunSomeRequests()
        {
            for (var i = 0; i < 30; i++)
            {
                var commandIndex = new Random().Next() % 5;
                if (commandIndex == 0) new SetCounterSample(_metricsContext).Process(new SendEmail());
                if (commandIndex == 1) new SetCounterSample(_metricsContext).Process(new ShipProduct());
                if (commandIndex == 2) new SetCounterSample(_metricsContext).Process(new BillCustomer());
                if (commandIndex == 3) new SetCounterSample(_metricsContext).Process(new MakeInvoice());
                if (commandIndex == 4) new SetCounterSample(_metricsContext).Process(new MarkAsPreffered());
            }
        }

        public interface Command
        {
        }

        public class BillCustomer : Command
        {
        }

        public class MakeInvoice : Command
        {
        }

        public class MarkAsPreffered : Command
        {
        }

        public class SendEmail : Command
        {
        }

        public class ShipProduct : Command
        {
        }
    }
}