// <copyright file="SetMeterSample.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Meter;

namespace ReportingSandbox.JustForTesting
{
    // ReSharper disable MemberCanBePrivate.Global
    public class SetMeterSample
    {
        private readonly IMeter _commandMeter;
        private readonly IMeter _errorMeter;

        public SetMeterSample(IMetrics metrics)
        {
            _errorMeter = metrics.Provider.Meter.Instance(SampleMetricsRegistry.Meters.Errors);
            _commandMeter = metrics.Provider.Meter.Instance(SampleMetricsRegistry.Meters.CommandMeter);
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

        private void ActualCommandProcessing(ICommand command) { _commandMeter.Mark(command.GetType().Name); }

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