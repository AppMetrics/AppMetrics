// <copyright file="SatisfyingV2Controller.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using MetricsSandboxMvc.JustForTesting;
using Microsoft.AspNetCore.Mvc;

namespace MetricsSandboxMvc.Controllers
{
    [Route("api/v{version:apiVersion}/satisfying")]
    [ApiVersion("v2")]
    public class SatisfyingV2Controller : ControllerBase
    {
        private readonly RequestDurationForApdexTesting _durationForApdexTesting;

        private readonly IMetrics _metrics;

        public SatisfyingV2Controller(IMetrics metrics, RequestDurationForApdexTesting durationForApdexTesting)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _durationForApdexTesting = durationForApdexTesting;
        }

        public static Random Rnd { get; } = new Random();

        [HttpGet]
        [Route("{param}")]
        public async Task<int> GetWithParam(int param)
        {
            var duration = _durationForApdexTesting.NextSatisfiedDuration;

            foreach (var i in Enumerable.Range(1, 3))
            {
                var tags = new MetricTags(new[] { "version", $"key{i}" }, new[] { "2", $"value{i}" });

                _metrics.Measure.Histogram.Update(Registry.One, tags, Rnd.Next(1, 500));
                _metrics.Measure.Histogram.Update(Registry.Two, tags, Rnd.Next(1, 500));
                _metrics.Measure.Histogram.Update(Registry.Three, tags, Rnd.Next(1, 500));
            }

            await Task.Delay(duration, HttpContext.RequestAborted);

            return duration;
        }

        [HttpGet]
        public async Task<int> GetNoParam()
        {
            var duration = _durationForApdexTesting.NextSatisfiedDuration;

            foreach (var i in Enumerable.Range(1, 3))
            {
                var tags = new MetricTags(new[] { "version", $"key{i}" }, new[] { "2", $"value{i}" });

                _metrics.Measure.Histogram.Update(Registry.One, tags, Rnd.Next(1, 500));
                _metrics.Measure.Histogram.Update(Registry.Two, tags, Rnd.Next(1, 500));
                _metrics.Measure.Histogram.Update(Registry.Three, tags, Rnd.Next(1, 500));
            }

            await Task.Delay(duration, HttpContext.RequestAborted);

            return duration;
        }
    }
}