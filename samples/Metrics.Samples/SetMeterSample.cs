using System;
using App.Metrics;

namespace Metrics.Samples
{
    public class SetMeterSample
    {
        private readonly IMeter _errorMeter;
        private readonly IMeter _commandMeter;
        private static IMetrics _metrics;

        public SetMeterSample(IMetrics metrics)
        {
            _metrics = metrics;

            _errorMeter = _metrics.Advanced.Meter(SampleMetricsRegistry.Meters.Errors);
            _commandMeter = _metrics.Advanced.Meter(SampleMetricsRegistry.Meters.CommandMeter);
        }

        public void Process(ICommand command)
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

        private void ActualCommandProcessing(ICommand command)
        {
            _commandMeter.Mark(command.GetType().Name);
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