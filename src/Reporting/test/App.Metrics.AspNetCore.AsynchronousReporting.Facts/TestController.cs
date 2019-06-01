using Microsoft.AspNetCore.Mvc;

namespace App.Metrics.AspNetCore.AsynchronousReporting.Facts
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IMetrics _metrics;

        public TestController(IMetrics metrics) { _metrics = metrics; }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}