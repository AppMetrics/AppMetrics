using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Metrics.Facts
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly TestContext _metricsContext;

        public TestController(TestContext metricsContext)
        {
            _metricsContext = metricsContext;
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
        public async Task<IActionResult> Get300ms()
        {
            _metricsContext.Clock.Advance(TimeUnit.Milliseconds, 300);
            return StatusCode(200);
        }

        [HttpGet("30ms")]
        public async Task<IActionResult> Get30ms()
        {
            _metricsContext.Clock.Advance(TimeUnit.Milliseconds, 30);

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