
using System;
using App.Metrics;

namespace Metrics.Samples
{
    public class SetCounterSample
    {
        private readonly Counter commandCounter = Metric.Counter("Command Counter", Unit.Custom("Commands"));

        public interface Command { }
        public class SendEmail : Command { }
        public class ShipProduct : Command { }
        public class BillCustomer : Command { }
        public class MakeInvoice : Command { }
        public class MarkAsPreffered : Command { }

        public void Process(Command command)
        {
            this.commandCounter.Increment(command.GetType().Name);

            // do actual command processing
        }

        public static void RunSomeRequests()
        {
            for (int i = 0; i < 30; i++)
            {

                var commandIndex = new Random().Next() % 5;
                if (commandIndex == 0) new SetCounterSample().Process(new SendEmail());
                if (commandIndex == 1) new SetCounterSample().Process(new ShipProduct());
                if (commandIndex == 2) new SetCounterSample().Process(new BillCustomer());
                if (commandIndex == 3) new SetCounterSample().Process(new MakeInvoice());
                if (commandIndex == 4) new SetCounterSample().Process(new MarkAsPreffered());
            }
        }
    }
}
