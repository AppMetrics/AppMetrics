using System;
using System.Collections.Generic;
using System.Net;
using App.Metrics.Logging;
using App.Metrics.Meter;
using App.Metrics.Timer;
using NServiceBus;
using NServiceBus.Features;

namespace App.Metrics.Extensions.NServiceBus
{
    public class AppMetricsFeature : Feature
    {
        private static readonly ILog Logger = LogProvider.For<AppMetricsFeature>();

        private static readonly string[] Tags =
        {
            "endpoint",
            "hostname",
            "endpointdiscriminator",
            "endpointqueue"
        };

        private static readonly Dictionary<string, TimerOptions> DurationMapping =
            new Dictionary<string, TimerOptions>
            {
                {
                    "Critical Time",
                    NServiceBusMetricsRegistry.Timers.CriticalTime
                },
                {
                    "Processing Time",
                    NServiceBusMetricsRegistry.Timers.ProcessingTime
                }
            };

        private static readonly Dictionary<string, MeterOptions> SignalMapping =
            new Dictionary<string, MeterOptions>
            {
                {
                    "# of msgs failures / sec",
                    NServiceBusMetricsRegistry.Meters.FailureRate
                },
                {
                    "# of msgs successfully processed / sec",
                    NServiceBusMetricsRegistry.Meters.SuccessRate
                },
                {
                    "# of msgs pulled from the input queue /sec",
                    NServiceBusMetricsRegistry.Meters.FetchRate
                },
                {
                    "Retries",
                    NServiceBusMetricsRegistry.Meters.Retries
                }
            };

        private global::NServiceBus.MetricsOptions _metricsOptions;

        public AppMetricsFeature()
        {
            Defaults(settings => { _metricsOptions = settings.EnableMetrics(); });
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var settings = context.Settings;
            var logicalAddress = settings.LogicalAddress();
            var discriminator = logicalAddress.EndpointInstance.Discriminator ?? "none";

            var tagValues = new[]
            {
                settings.EndpointName(),
                Dns.GetHostName(),
                discriminator,
                settings.LocalAddress()
            };

            _metricsOptions.RegisterObservers(probeContext => RegisterProbes(probeContext, tagValues));
        }

        public void RegisterProbes(ProbeContext context, string[] tagValues)
        {
            var tags = new MetricTags(Tags, tagValues);

            foreach (var duration in context.Durations)
            {
                if (!DurationMapping.ContainsKey(duration.Name))
                {
                    Logger.WarnFormat("Unsupported duration probe {0}", duration.Name);
                    continue;
                }

                var timer = DurationMapping[duration.Name];
                duration.Register((ref DurationEvent @event) =>
                    Metrics.Instance.Measure.Timer.Time(timer, tags, Convert.ToInt64(@event.Duration.TotalMilliseconds)));
            }

            foreach (var signal in context.Signals)
            {
                if (!SignalMapping.ContainsKey(signal.Name))
                {
                    Logger.WarnFormat("Unsupported signal probe {0}", signal.Name);
                    continue;
                }

                var meter = SignalMapping[signal.Name];

                signal.Register((ref SignalEvent @event) =>
                {
                    if (string.IsNullOrWhiteSpace(@event.MessageType))
                    {
                        Metrics.Instance.Measure.Meter.Mark(meter, tags);
                    }
                    else
                    {
                        var tagsWithMessageType = tags.ToDictionary();
                        tagsWithMessageType.Add("messagetype", @event.MessageType);
                        Metrics.Instance.Measure.Meter.Mark(meter, tagsWithMessageType.FromDictionary());
                    }
                });
            }
        }
    }
}