using App.Metrics;
using App.Metrics.Core;

namespace App.Sample
{
    public static class AppMetricsRegistry
    {
        public static class Gauges
        {
            public static GaugeOptions ApmGauge = new GaugeOptions
            {
                Name = "& ApmGauge",
                MeasurementUnit = Unit.None
            };

            public static GaugeOptions Errors = new GaugeOptions
            {
                Name = "Errors",
                MeasurementUnit = Unit.None
            };

            public static GaugeOptions GaugeWithNoValue = new GaugeOptions
            {
                Name = "Gauge With No Value",
                MeasurementUnit = Unit.None
            };

            public static GaugeOptions ParenthesisGauge = new GaugeOptions
            {
                Name = "()[]{} ParantesisGauge",
                MeasurementUnit = Unit.None
            };

            public static GaugeOptions PercentGauge = new GaugeOptions
            {
                Name = "% Percent/Gauge|test",
                MeasurementUnit = Unit.None
            };
        }
    }
}