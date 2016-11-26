using System;
using System.Collections.Generic;
using System.Threading;
using App.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Sample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private static readonly Random Rnd = new Random();

        private readonly IMetrics _metrics;

        public ValuesController(IMetrics metrics)
        {
            if (metrics == null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }

            _metrics = metrics;
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _metrics.Increment(MetricsRegistry.Counters.TestCounter);
            _metrics.Increment(MetricsRegistry.Counters.TestCounter, 4);
            _metrics.Decrement(MetricsRegistry.Counters.TestCounter, 2);

            var histogram = _metrics.Advanced.Histogram(MetricsRegistry.Histograms.TestHAdvancedistogram);
            histogram.Update(Rnd.Next(1, 20));

            _metrics.Update(MetricsRegistry.Histograms.TestHistogram, Rnd.Next(20, 40));

            _metrics.Time(MetricsRegistry.Timers.TestTimer, () => Thread.Sleep(15));
            _metrics.Time(MetricsRegistry.Timers.TestTimer, () => Thread.Sleep(20), "value1");
            _metrics.Time(MetricsRegistry.Timers.TestTimer, () => Thread.Sleep(25), "value2");

            using (_metrics.Time(MetricsRegistry.Timers.TestTimerTwo))
            {
                Thread.Sleep(15);
            }

            using (_metrics.Time(MetricsRegistry.Timers.TestTimerTwo, "value1"))
            {
                Thread.Sleep(20);
            }

            using (_metrics.Time(MetricsRegistry.Timers.TestTimerTwo, "value2"))
            {
                Thread.Sleep(25);
            }

            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("bad")]
        public IActionResult Get400()
        {
            return StatusCode(400);
        }

        [HttpGet("unauth")]
        public IActionResult Get401()
        {
            return StatusCode(401);
        }

        [HttpGet("error")]
        public IActionResult Get500()
        {
            return StatusCode(500);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}