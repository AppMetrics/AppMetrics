using System;
using Api.InfluxDB.Sample.ForTesting;
using App.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace Api.InfluxDB.Sample.Controllers
{
    [Route("api/[controller]")]
    public class ApdexSatisfiedController : Controller
    {
        private readonly RequestDurationForApdexTesting _durationForApdexTesting;

        private readonly IMetrics _metrics;

        public ApdexSatisfiedController(IMetrics metrics, RequestDurationForApdexTesting durationForApdexTesting)
        {
            if (metrics == null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }

            _metrics = metrics;
            _durationForApdexTesting = durationForApdexTesting;
        }

        [HttpGet]
        public int Get()
        {
            var duration = _durationForApdexTesting.NextSatisfiedDuration;
            _metrics.Advanced.Clock.Advance(TimeUnit.Milliseconds, duration);

            return duration;
        }
    }
}