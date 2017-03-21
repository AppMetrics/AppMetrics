using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core.Options;
using App.Metrics.Sandbox.JustForTesting;
using App.Metrics.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace App.Metrics.Sandbox.Controllers
{
    public static class Registry
    {
        public static HistogramOptions One = new HistogramOptions
                                      {
                                          Name = "test1",
                                          MeasurementUnit = Unit.Bytes,
                                          Context = "test"
                                      };

        public static HistogramOptions Two = new HistogramOptions
                                             {
                                                 Name = "test2",
                                                 MeasurementUnit = Unit.Bytes,
                                                 Context = "test"
                                             };

        public static HistogramOptions Three = new HistogramOptions
                                             {
                                                 Name = "test3",
                                                 MeasurementUnit = Unit.Bytes,
                                                 Context = "test"
                                             };
    }

    [Route("api/[controller]")]
    public class SatisfyingController : Controller
    {
        private readonly RequestDurationForApdexTesting _durationForApdexTesting;
        private static readonly Random Rnd = new Random();
        private readonly IMetrics _metrics;

        public SatisfyingController(IMetrics metrics, RequestDurationForApdexTesting durationForApdexTesting)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _durationForApdexTesting = durationForApdexTesting;
        }

        [HttpGet]
        public async Task<int> Get()
        {
            var duration = _durationForApdexTesting.NextSatisfiedDuration;

            //foreach (var i in Enumerable.Range(1, 500))
            //{
            //    var tags = new MetricTags($"key{i}", $"value{i}");

            //    _metrics.Measure.Histogram.Update(Registry.One, tags, Rnd.Next(1, 500));
            //    _metrics.Measure.Histogram.Update(Registry.Two, tags, Rnd.Next(1, 500));
            //    _metrics.Measure.Histogram.Update(Registry.Three, tags, Rnd.Next(1, 500));
            //}

            await Task.Delay(duration, HttpContext.RequestAborted);

            return duration;
        }
    }
}