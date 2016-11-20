using System;
using App.Metrics;

namespace Metrics.Samples
{
    public class SetMeterSample
    {
        private readonly IMeter _errorMeter;
        private static IMetrics _metrics;

        public SetMeterSample(IMetrics metrics)
        {
            _metrics = metrics;

            _errorMeter = _metrics.Advanced.Meter(SampleMetricsRegistry.Meters.Errors);
        }

        public void Process(Command command)
        {
            try
            {
                ActualCommandProcessing(command);
            }
            catch
            {
                _errorMeter.Mark(command.GetType().Name);
            }
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

        private void ActualCommandProcessing(Command command)
        {
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