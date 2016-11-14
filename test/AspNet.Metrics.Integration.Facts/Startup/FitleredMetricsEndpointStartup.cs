using App.Metrics;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Internal;
using AspNet.Metrics.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Integration.Facts.Startup
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

            Metrics.Increment(counterOptions);
            Metrics.Mark(meterOptions);
            Metrics.Time(timerOptions, () => Metrics.Advanced.Clock.Advance(TimeUnit.Milliseconds, 10));
            Metrics.Update(histogramOptions, 5);
            Metrics.Gauge(gaugeOptions, () => 8);
        }
    }
}