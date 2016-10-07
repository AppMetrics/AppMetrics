
using System;
using App.Metrics;

namespace Metrics.Samples
{
    public class SetMeterSample
    {
        private readonly IMeter errorMeter = Metric.Meter("Errors", Unit.Errors);

        public interface Command { }
        public class SendEmail : Command { }
        public class ShipProduct : Command { }
        public class BillCustomer : Command { }
        public class MakeInvoice : Command { }
        public class MarkAsPreffered : Command { }

        public void Process(Command command)
        {
            try
            {
                ActualCommandProcessing(command);
            }
            catch
            {
                errorMeter.Mark(command.GetType().Name);
            }
        }

        private void ActualCommandProcessing(Command command)
        {
            //throw new DivideByZeroException();
        }

        public static void RunSomeRequests()
        {
            for (int i = 0; i < 30; i++)
            {
                var commandIndex = new Random().Next() % 5;
                if (commandIndex == 0) new SetMeterSample().Process(new SendEmail());
                if (commandIndex == 1) new SetMeterSample().Process(new ShipProduct());
                if (commandIndex == 2) new SetMeterSample().Process(new BillCustomer());
                if (commandIndex == 3) new SetMeterSample().Process(new MakeInvoice());
                if (commandIndex == 4) new SetMeterSample().Process(new MarkAsPreffered());
            }
        }
    }
}
