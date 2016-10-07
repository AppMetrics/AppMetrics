using System;
using App.Metrics;

namespace Metrics.Samples
{
    public class SetMeterSample
    {
        private readonly IMeter _errorMeter;
        private static IMetricsContext _metricsContext;

        public SetMeterSample(IMetricsContext metricsContext)
        {
            _metricsContext = metricsContext;

            _errorMeter = _metricsContext.Meter("Errors", Unit.Errors);
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
                if (commandIndex == 0) new SetMeterSample(_metricsContext).Process(new SendEmail());
                if (commandIndex == 1) new SetMeterSample(_metricsContext).Process(new ShipProduct());
                if (commandIndex == 2) new SetMeterSample(_metricsContext).Process(new BillCustomer());
                if (commandIndex == 3) new SetMeterSample(_metricsContext).Process(new MakeInvoice());
                if (commandIndex == 4) new SetMeterSample(_metricsContext).Process(new MarkAsPreffered());
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