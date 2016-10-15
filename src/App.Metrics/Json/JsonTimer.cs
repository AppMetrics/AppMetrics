using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public sealed class JsonTimer : JsonMetric
    {
        public long ActiveSessions { get; set; }

        public long Count { get; set; }

        public string DurationUnit { get; set; }

        public HistogramData Histogram { get; set; }

        public RateData Rate { get; set; }

        public string RateUnit { get; set; }

        public long TotalTime { get; set; }

        public static JsonTimer FromTimer(TimerValueSource timer)
        {
            return new JsonTimer
            {
                Name = timer.Name,
                Count = timer.Value.Rate.Count,
                ActiveSessions = timer.Value.ActiveSessions,
                TotalTime = timer.Value.TotalTime,
                Rate = ToRate(timer.Value.Rate),
                Histogram = ToHistogram(timer.Value.Histogram),
                Unit = timer.Unit.Name,
                RateUnit = timer.RateUnit.Unit(),
                DurationUnit = timer.DurationUnit.Unit(),
                Tags = timer.Tags
            };
        }

        public IEnumerable<JsonProperty> ToJsonProperties()
        {
            yield return new JsonProperty("Name", Name);
            yield return new JsonProperty("Count", Count);
            yield return new JsonProperty("ActiveSessions", ActiveSessions);
            yield return new JsonProperty("TotalTime", TotalTime);

            yield return new JsonProperty("Rate", ToJsonProperties(Rate));
            yield return new JsonProperty("Histogram", ToJsonProperties(Histogram));

            yield return new JsonProperty("Unit", Unit);
            yield return new JsonProperty("RateUnit", RateUnit);
            yield return new JsonProperty("DurationUnit", DurationUnit);

            if (Tags.Length > 0)
            {
                yield return new JsonProperty("Tags", Tags);
            }
        }

        public JsonObject ToJsonTimer()
        {
            return new JsonObject(ToJsonProperties());
        }

        public TimerValueSource ToValueSource()
        {
            var rateUnit = TimeUnitExtensions.FromUnit(RateUnit);
            var durationUnit = TimeUnitExtensions.FromUnit(DurationUnit);

            var rateValue = new MeterValue(Count, Rate.MeanRate, Rate.OneMinuteRate, Rate.FiveMinuteRate,
                Rate.FifteenMinuteRate, rateUnit);
            var histogramValue = new HistogramValue(Count,
                Histogram.LastValue, Histogram.LastUserValue,
                Histogram.Max, Histogram.MaxUserValue, Histogram.Mean,
                Histogram.Min, Histogram.MinUserValue, Histogram.StdDev, Histogram.Median,
                Histogram.Percentile75, Histogram.Percentile95, Histogram.Percentile98,
                Histogram.Percentile99, Histogram.Percentile999, Histogram.SampleSize);

            var timerValue = new TimerValue(rateValue, histogramValue, ActiveSessions, TotalTime, durationUnit);

            return new TimerValueSource(Name, ConstantValue.Provider(timerValue), Unit, rateUnit, durationUnit, Tags);
        }

        private static HistogramData ToHistogram(HistogramValue histogram)
        {
            return new HistogramData
            {
                LastValue = histogram.LastValue,
                LastUserValue = histogram.LastUserValue,
                Max = histogram.Max,
                MaxUserValue = histogram.MaxUserValue,
                Mean = histogram.Mean,
                Min = histogram.Min,
                MinUserValue = histogram.MinUserValue,
                StdDev = histogram.StdDev,
                Median = histogram.Median,
                Percentile75 = histogram.Percentile75,
                Percentile95 = histogram.Percentile95,
                Percentile98 = histogram.Percentile98,
                Percentile99 = histogram.Percentile99,
                Percentile999 = histogram.Percentile999,
                SampleSize = histogram.SampleSize,
            };
        }

        private static IEnumerable<JsonProperty> ToJsonProperties(RateData rate)
        {
            yield return new JsonProperty("MeanRate", rate.MeanRate);
            yield return new JsonProperty("OneMinuteRate", rate.OneMinuteRate);
            yield return new JsonProperty("FiveMinuteRate", rate.FiveMinuteRate);
            yield return new JsonProperty("FifteenMinuteRate", rate.FifteenMinuteRate);
        }

        private static IEnumerable<JsonProperty> ToJsonProperties(HistogramData histogram)
        {
            var hasUserValues = histogram.LastUserValue != null || histogram.MinUserValue != null || histogram.MaxUserValue != null;

            yield return new JsonProperty("LastValue", histogram.LastValue);

            if (hasUserValues)
            {
                yield return new JsonProperty("LastUserValue", histogram.LastUserValue);
            }
            yield return new JsonProperty("Min", histogram.Min);
            if (hasUserValues)
            {
                yield return new JsonProperty("MinUserValue", histogram.MinUserValue);
            }
            yield return new JsonProperty("Mean", histogram.Mean);
            if (hasUserValues)
            {
                yield return new JsonProperty("MaxUserValue", histogram.MaxUserValue);
            }

            yield return new JsonProperty("StdDev", histogram.StdDev);
            yield return new JsonProperty("Median", histogram.Median);
            yield return new JsonProperty("Percentile75", histogram.Percentile75);
            yield return new JsonProperty("Percentile95", histogram.Percentile95);
            yield return new JsonProperty("Percentile98", histogram.Percentile98);
            yield return new JsonProperty("Percentile99", histogram.Percentile99);
            yield return new JsonProperty("Percentile999", histogram.Percentile999);
            yield return new JsonProperty("SampleSize", histogram.SampleSize);
        }

        private static RateData ToRate(MeterValue rate)
        {
            return new RateData
            {
                MeanRate = rate.MeanRate,
                OneMinuteRate = rate.OneMinuteRate,
                FiveMinuteRate = rate.FiveMinuteRate,
                FifteenMinuteRate = rate.FifteenMinuteRate
            };
        }

        public class HistogramData
        {
            public string LastUserValue { get; set; }

            public double LastValue { get; set; }

            public double Max { get; set; }

            public string MaxUserValue { get; set; }

            public double Mean { get; set; }

            public double Median { get; set; }

            public double Min { get; set; }

            public string MinUserValue { get; set; }

            public double Percentile75 { get; set; }

            public double Percentile95 { get; set; }

            public double Percentile98 { get; set; }

            public double Percentile99 { get; set; }

            public double Percentile999 { get; set; }

            public int SampleSize { get; set; }

            public double StdDev { get; set; }
        }

        public class RateData
        {
            public double FifteenMinuteRate { get; set; }

            public double FiveMinuteRate { get; set; }

            public double MeanRate { get; set; }

            public double OneMinuteRate { get; set; }
        }
    }
}