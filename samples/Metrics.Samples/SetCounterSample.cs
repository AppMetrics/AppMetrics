using System;
using App.Metrics;

namespace Metrics.Samples
{
    public class SetCounterSample
    {
        private readonly ICounter _commandCounter;
        private readonly ICounter _commandCounterNotReset;
        private readonly ICounter _commandCounterNoPercentages;
        private readonly ICounter _commandCounterNoReportSetItems;
        private static IMetrics _metrics;

        public SetCounterSample(IMetrics metrics)
        {
            _metrics = metrics;

            _commandCounter = _metrics.Advanced.Counter(SampleMetricsRegistry.Counters.CommandCounter);
            _commandCounterNoPercentages = _metrics.Advanced.Counter(SampleMetricsRegistry.Counters.CommandCounterNoPercentages);
            _commandCounterNotReset = _metrics.Advanced.Counter(SampleMetricsRegistry.Counters.CommandCounterNotReset);
            _commandCounterNoReportSetItems = _metrics.Advanced.Counter(SampleMetricsRegistry.Counters.CommandCounterDontReportSetItems);
        }

        public void Process(ICommand command)
        {
            _commandCounterNotReset.Increment(command.GetType().Name);
            _commandCounter.Increment(command.GetType().Name);
            _commandCounterNoPercentages.Increment(command.GetType().Name);
            _commandCounterNoReportSetItems.Increment(command.GetType().Name);
        }

        public void RunSomeRequests()
        {
            for (var i = 0; i < 30; i++)
            {
                var commandIndex = new Random().Next() % 5;
                if (commandIndex == 0) Process(new SendEmail());
                if (commandIndex == 1) Process(new ShipProduct());
                if (commandIndex == 2) Process(new BillCustomer());
                if (commandIndex == 3) Process(new MakeInvoice());
                if (commandIndex == 4) Process(new MarkAsPreffered());
            }
        }

        public interface ICommand
        {
        }

        public class BillCustomer : ICommand
        {
        }

        public class MakeInvoice : ICommand
        {
        }

        public class MarkAsPreffered : ICommand
        {
        }

        public class SendEmail : ICommand
        {
        }

        public class ShipProduct : ICommand
        {
        }
    }
}