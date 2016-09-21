using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metrics;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Metrics.Facts
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly TestContext _metricContext;

        public TestController(TestContext context)
        {
            _metricContext = context;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

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

        [HttpGet("300ms")]
        public IActionResult Get300ms()
        {
            _metricContext.Clock.Advance(TimeUnit.Milliseconds, 300);

            return StatusCode(200);
        }

        [HttpGet("30ms")]
        public IActionResult Get30ms()
        {
            _metricContext.Clock.Advance(TimeUnit.Milliseconds, 30);

            return StatusCode(200);
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

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}