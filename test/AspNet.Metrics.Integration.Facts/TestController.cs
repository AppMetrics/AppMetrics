using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Metrics.Integration.Facts
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IMetrics _metrics;

        public TestController(IMetrics metrics)
        {
            _metrics = metrics;
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

        [HttpGet("300ms")]
        public async Task<IActionResult> Get300ms()
        {
            _metrics.Advanced.Clock.Advance(TimeUnit.Milliseconds, 300);
            await Task.FromResult(0);
            return StatusCode(200);
        }

        [HttpGet("30ms")]
        public async Task<IActionResult> Get30ms()
        {
            _metrics.Advanced.Clock.Advance(TimeUnit.Milliseconds, 30);
            await Task.FromResult(0);
            return StatusCode(200);
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