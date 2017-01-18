using App.Metrics.Configuration;
using App.Metrics.Core.Options;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Startup
{
    public class FitleredMetricsEndpointStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            SetupAppBuilder(app, env, loggerFactory);

            RecordSomeMetrics();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsOptions = new AppMetricsOptions
            {
                DefaultSamplingType = SamplingType.LongTerm,
                MetricsEnabled = true,
            };

            var aspNetMetricsOptions = new AspNetMetricsOptions();

            SetupServices(services, appMetricsOptions, aspNetMetricsOptions,
                new DefaultMetricsFilter().WhereType(MetricType.Counter));
        }

        private void RecordSomeMetrics()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test_counter",
                MeasurementUnit = Unit.Requests,
                Tags = new MetricTags().With("tag1", "value")
            };

            var meterOptions = new MeterOptions
            {
                Name = "test_meter",
                MeasurementUnit = Unit.None,
                Tags = new MetricTags().With("tag2", "value")
            };

            var timerOptions = new TimerOptions
            {
                Name = "test_timer",
                MeasurementUnit = Unit.Requests
            };

            var histogramOptions = new HistogramOptions
            {
                Name = "test_histogram",
                MeasurementUnit = Unit.Requests
            };

            var gaugeOptions = new GaugeOptions
            {
                Name = "test_gauge"
            };

            Metrics.Counter.Increment(counterOptions);
            Metrics.Meter.Mark(meterOptions);
            Metrics.Timer.Time(timerOptions, () => Metrics.Clock.Advance(TimeUnit.Milliseconds, 10));
            Metrics.Histogram.Update(histogramOptions, 5);
            Metrics.Gauge.SetValue(gaugeOptions, () => 8);
        }
    }
}