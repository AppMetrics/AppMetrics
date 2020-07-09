// <copyright file="GcEventListener.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics.Tracing;
using System.Linq;
using App.Metrics.Extensions.Collectors.MetricsRegistries;

namespace App.Metrics.Extensions.Collectors.EventListeners
{
     public class GcEventListener : EventListener
    {
        // https://docs.microsoft.com/en-us/dotnet/framework/performance/garbage-collection-etw-events
        private const int GC_KEYWORD = 0x0000001;
        private readonly IMetrics _metrics;

        public GcEventListener(IMetrics metrics)
        {
            _metrics = metrics;
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
            {
                EnableEvents(eventSource, EventLevel.Informational, (EventKeywords) GC_KEYWORD);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            switch (eventData.EventName)
            {
                case "GCHeapStats_V1":
                    ProcessHeapStats(eventData);
                    break;
            }
        }

        private void ProcessHeapStats(EventWrittenEventArgs eventData)
        {
            if (eventData?.Payload == null || eventData.Payload.Count == 0)
            {
                return;
            }

            ulong gen0HeapSize = 0;
            ulong gen0Promoted = 0;
            ulong gen1HeapSize = 0;
            ulong gen1Promoted = 0;
            ulong gen2HeapSize = 0;
            ulong gen2Survived = 0;
            ulong lohSize = 0;
            ulong lohSurvived = 0;

            if (eventData.Payload.ElementAtOrDefault(0) != null && eventData.Payload[0] != null)
            {
                gen0HeapSize = (ulong)eventData.Payload[0];
            }
            
            if (eventData.Payload.ElementAtOrDefault(1) != null && eventData.Payload[1] != null)
            {
                gen0Promoted = (ulong)eventData.Payload[1];
            }
            
            if (eventData.Payload.ElementAtOrDefault(2) != null && eventData.Payload[2] != null)
            {
                gen1HeapSize = (ulong)eventData.Payload[2];
            }
            
            if (eventData.Payload.ElementAtOrDefault(3) != null && eventData.Payload[3] != null)
            {
                gen1Promoted = (ulong)eventData.Payload[3];
            }
            
            if (eventData.Payload.ElementAtOrDefault(4) != null && eventData.Payload[4] != null)
            {
                gen2HeapSize = (ulong)eventData.Payload[4];
            }
            
            if (eventData.Payload.ElementAtOrDefault(5) != null && eventData.Payload[5] != null)
            {
                gen2Survived = (ulong)eventData.Payload[5];
            }
            
            if (eventData.Payload.ElementAtOrDefault(6) != null && eventData.Payload[6] != null)
            {
                lohSize = (ulong)eventData.Payload[6];
            }
            
            if (eventData.Payload.ElementAtOrDefault(7) != null && eventData.Payload[7] != null)
            {
                lohSurvived = (ulong)eventData.Payload[7];
            }
           
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.Gen0HeapSize, gen0HeapSize);
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.BytesPromotedFromGen0, gen0Promoted);
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.Gen1HeapSize, gen1HeapSize);
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.BytesPromotedFromGen1, gen1Promoted);
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.Gen2HeapSize, gen2HeapSize);
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.BytesSurvivedFromGen2, gen2Survived);
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.LargeObjectHeapSize, lohSize);
            _metrics.Measure.Gauge.SetValue(GcMetricsRegistry.Gauges.BytesSurvivedLargeObjectHeap, lohSurvived);

        }
    }
}