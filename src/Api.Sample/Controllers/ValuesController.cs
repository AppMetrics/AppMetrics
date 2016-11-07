using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Core;
using Microsoft.AspNetCore.Mvc;

namespace Api.Sample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private static readonly Random Rnd = new Random();

        private readonly IMetricsContext _metricsContext;

        public ValuesController(IMetricsContext metricsContext)
        {
            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            _metricsContext = metricsContext;
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

            _metricsContext.Increment(Metrics.Counters.TestCounter);
            _metricsContext.Increment(Metrics.Counters.TestCounter, 4);
            _metricsContext.Decrement(Metrics.Counters.TestCounter, 2);

            var histogram = _metricsContext.Advanced.Histogram(Metrics.Histograms.TestHAdvancedistogram);
            histogram.Update(Rnd.Next(1, 20));

            _metricsContext.Update(Metrics.Histograms.TestHistogram, Rnd.Next(20, 40));

            _metricsContext.Time(Metrics.Timers.TestTimer, () => Thread.Sleep(15));
            _metricsContext.Time(Metrics.Timers.TestTimer, () => Thread.Sleep(20), "value1");
            _metricsContext.Time(Metrics.Timers.TestTimer, () => Thread.Sleep(25), "value2");     
            
            using (_metricsContext.Time(Metrics.Timers.TestTimerTwo))
            {
                Thread.Sleep(15);
            }

            using (_metricsContext.Time(Metrics.Timers.TestTimerTwo, "value1"))
            {
                Thread.Sleep(20);
            }            

            using (_metricsContext.Time(Metrics.Timers.TestTimerTwo, "value2"))
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

        [HttpGet("error")]
        public IActionResult Get500()
        {
            return StatusCode(500);
        }

        [HttpGet("unauth")]
        public IActionResult Get401()
        {
            return StatusCode(401);
        }

        [HttpGet("bad")]
        public IActionResult Get400()
        {
            return StatusCode(400);
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