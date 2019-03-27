// <copyright file="SetCounterSample.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Counter;

namespace ReportingSandbox.JustForTesting
{
    // ReSharper disable MemberCanBePrivate.Global
    public class SetCounterSample
    {
        private readonly ICounter _commandCounter;
        private readonly ICounter _commandCounterNoPercentages;
        private readonly ICounter _commandCounterNoReportSetItems;
        private readonly ICounter _commandCounterNotReset;

        public SetCounterSample(IMetrics metrics)
        {
            _commandCounter = metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.CommandCounter);
            _commandCounterNoPercentages = metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.CommandCounterNoPercentages);
            _commandCounterNotReset = metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.CommandCounterNotReset);
            _commandCounterNoReportSetItems = metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.CommandCounterDontReportSetItems);
        }

        public void Process(ICommand command)
        {
            _commandCounterNotReset.Increment(command.GetType().Name);
            _commandCounter.Increment(command.GetType().Name);
            _commandCounterNoPercentages.Increment(command.GetType().Name);
            _commandCounterNoReportSetItems.Increment(command.GetType().Name);
        }

        public void Run()
        {
            for (var i = 0; i < 30; i++)
            {
                var commandIndex = new Random().Next() % 5;
                if (commandIndex == 0)
                {
                    Process(new SendEmail());
                }

                if (commandIndex == 1)
                {
                    Process(new ShipProduct());
                }

                if (commandIndex == 2)
                {
                    Process(new BillCustomer());
                }

                if (commandIndex == 3)
                {
                    Process(new MakeInvoice());
                }

                if (commandIndex == 4)
                {
                    Process(new MarkAsPreffered());
                }
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

    // ReSharper restore MemberCanBePrivate.Global
}